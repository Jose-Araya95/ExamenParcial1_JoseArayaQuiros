using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TaskManager.Api.DTOs;
using TaskManager.Api.Models;
using TaskManager.Api.Settings;

namespace TaskManager.Api.Services;

public class TaskService : ITaskService
{
    private readonly IMongoCollection<TaskItem> _tasksCollection;

    public TaskService(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _tasksCollection = mongoDatabase.GetCollection<TaskItem>(settings.Value.TasksCollectionName);
    }

    public async Task<List<TaskItem>> GetAllTasksAsync(bool? isCompleted, string? priority, string? sortBy, string? search)
    {
        IQueryable<TaskItem> query = _tasksCollection.AsQueryable();

        // LINQ Filtering: Status
        if (isCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        }

        // LINQ Filtering: Priority
        if (!string.IsNullOrWhiteSpace(priority))
        {
            query = query.Where(t => t.Priority.ToLower() == priority.ToLower());
        }

        // LINQ Search: Title or Description keyword match
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(term) || t.Description.ToLower().Contains(term));
        }

        // LINQ Sorting
        query = sortBy?.ToLower() switch
        {
            "priority" => query.OrderByDescending(t => t.Priority),
            "duedate" => query.OrderBy(t => t.DueDate),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };

        return await System.Threading.Tasks.Task.FromResult(query.ToList());
    }

    public async Task<TaskItem?> GetTaskByIdAsync(string id)
    {
        return await _tasksCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task<TaskItem> CreateTaskAsync(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Priority = dto.Priority,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await _tasksCollection.InsertOneAsync(task);
        return task;
    }

    public async Task<bool> UpdateTaskAsync(string id, UpdateTaskDto dto)
    {
        var filter = Builders<TaskItem>.Filter.Eq(t => t.Id, id);
        var update = Builders<TaskItem>.Update
            .Set(t => t.Title, dto.Title)
            .Set(t => t.Description, dto.Description)
            .Set(t => t.IsCompleted, dto.IsCompleted)
            .Set(t => t.DueDate, dto.DueDate)
            .Set(t => t.Priority, dto.Priority);

        var result = await _tasksCollection.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteTaskAsync(string id)
    {
        var result = await _tasksCollection.DeleteOneAsync(t => t.Id == id);
        return result.DeletedCount > 0;
    }
}