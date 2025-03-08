using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Persistence.Data;

public class UserDbContext
{
    private readonly IMongoDatabase _database;

    public UserDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDbConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("MongoDB connection string is missing or empty in configuration.");
        }
            
        var databaseName = configuration["MongoDatabases:UserDatabase"];
        if (string.IsNullOrEmpty(databaseName))
        {
            throw new ArgumentException("MongoDB database name is missing or empty in configuration.");
        }

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
}