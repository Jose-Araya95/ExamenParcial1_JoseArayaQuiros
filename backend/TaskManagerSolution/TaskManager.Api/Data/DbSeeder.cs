using MongoDB.Driver;
using TaskManager.Api.Models;
using TaskManager.Api.Settings;

namespace TaskManager.Api.Data;

public static class DbSeeder
{
    public static void SeedData(IMongoDatabase database, MongoDbSettings settings)
    {
        var collection = database.GetCollection<TaskItem>(settings.TasksCollectionName);

        if (collection.CountDocuments(FilterDefinition<TaskItem>.Empty) == 0)
        {
            var sampleTasks = new List<TaskItem>
            {
                new() { Title = "Complete System Architecture Diagram", Description = "Draw C4 component diagrams for the REST API.", DueDate = DateTime.UtcNow.AddDays(2), IsCompleted = false, Priority = "High" },
                new() { Title = "Configure MongoDB Atlas Cluster", Description = "Set up database users and network access IP whitelist.", DueDate = DateTime.UtcNow.AddDays(-1), IsCompleted = true, Priority = "High" },
                new() { Title = "Implement JWT Authentication", Description = "Add token authorization middleware to HTTP request pipeline.", DueDate = DateTime.UtcNow.AddDays(5), IsCompleted = false, Priority = "Medium" },
                new() { Title = "Design Angular Dashboard UI", Description = "Create task listing layout with filtering toolbar.", DueDate = DateTime.UtcNow.AddDays(3), IsCompleted = false, Priority = "Medium" },
                new() { Title = "Write API Integration Tests", Description = "Verify CRUD operations against local Mongo instance.", DueDate = DateTime.UtcNow.AddDays(7), IsCompleted = false, Priority = "Low" },
                new() { Title = "Refactor LINQ Query Handlers", Description = "Optimize query expressions for sorting and text search.", DueDate = DateTime.UtcNow.AddDays(-3), IsCompleted = true, Priority = "Medium" },
                new() { Title = "Prepare Final Project Documentation", Description = "Draft academic report in Spanish following APA style rules.", DueDate = DateTime.UtcNow.AddDays(4), IsCompleted = false, Priority = "High" },
                new() { Title = "Audit Repository Dependencies", Description = "Verify clean gitignore patterns for build binaries.", DueDate = DateTime.UtcNow.AddDays(10), IsCompleted = false, Priority = "Low" },
                new() { Title = "Setup CORS Middleware", Description = "Allow front-end dev server domain http://localhost:4200.", DueDate = DateTime.UtcNow.AddDays(-2), IsCompleted = true, Priority = "High" },
                new() { Title = "Deploy Backend API Service", Description = "Verify production environment variables and container build.", DueDate = DateTime.UtcNow.AddDays(12), IsCompleted = false, Priority = "Low" }
            };

            collection.InsertMany(sampleTasks);
        }
    }
}