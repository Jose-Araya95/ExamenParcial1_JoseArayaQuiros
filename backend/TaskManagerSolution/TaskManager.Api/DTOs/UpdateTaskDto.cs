using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.DTOs;

public class UpdateTaskDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    [RegularExpression("Low|Medium|High")]
    public string Priority { get; set; } = "Medium";
}