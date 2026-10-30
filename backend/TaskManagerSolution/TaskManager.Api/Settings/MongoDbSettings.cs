namespace TaskManager.Api.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string TasksCollectionName { get; set; } = string.Empty;
}