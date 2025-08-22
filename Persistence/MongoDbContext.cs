using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Models;

namespace Persistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<FirewallRequestDto> FirewallRequests =>
            _database.GetCollection<FirewallRequestDto>("FirewallRequests");
        public IMongoCollection<UserDto> Users => _database.GetCollection<UserDto>("users");
    }
}
