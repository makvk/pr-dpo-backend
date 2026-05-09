namespace TaskManager.Models;

public class TaskUpdateModel
{
    public string title { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public bool isCompleted { get; set; }
}