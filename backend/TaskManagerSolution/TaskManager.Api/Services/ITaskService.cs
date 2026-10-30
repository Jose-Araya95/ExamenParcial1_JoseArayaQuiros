using TaskManager.Api.DTOs;
using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

public interface ITaskService
{
    Task<List<TaskItem>> GetAllTasksAsync(bool? isCompleted, string? priority, string? sortBy, string? search);
    Task<TaskItem?> GetTaskByIdAsync(string id);
    Task<TaskItem> CreateTaskAsync(CreateTaskDto dto);
    Task<bool> UpdateTaskAsync(string id, UpdateTaskDto dto);
    Task<bool> DeleteTaskAsync(string id);
}