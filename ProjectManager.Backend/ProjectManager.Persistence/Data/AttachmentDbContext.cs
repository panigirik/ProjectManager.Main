using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Persistence.Data;

public class AttachmentDbContext
{
    private readonly IMongoDatabase _database;

    public AttachmentDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDbConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("MongoDB connection string is missing or empty in configuration.");
        }
            
        var databaseName = configuration["MongoDatabases:AttachmentsDatabase"];
        if (string.IsNullOrEmpty(databaseName))
        {
            throw new ArgumentException("MongoDB database name is missing or empty in configuration.");
        }

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Attachment> Attachments => _database.GetCollection<Attachment>("Attachments");
}