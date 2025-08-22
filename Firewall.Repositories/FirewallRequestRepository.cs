using Firewall.Services;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Persistence;

namespace Firewall.Repositories
{
    public class FirewallRequestRepository : IFirewallRequestRepository
    {
        private readonly IMongoCollection<FirewallRequestDto> _collection;
        public FirewallRequestRepository(MongoDbContext context)
        {
            _collection = context.FirewallRequests;
        }
        public async Task<IEnumerable<FirewallRequestDto>> GetAllRequestsAsync()
        {
            var filter = Builders<FirewallRequestDto>.Filter.Empty;
            var sort = Builders<FirewallRequestDto>.Sort.Descending(r => r.CreatedAt);
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<FirewallRequestDto> GetByIdAsync(string id)
        {
            //var filter = Builders<FirewallRequestDto>.Filter.Eq(r => r.Id, id);
            //return await _collection.Find(filter).FirstOrDefaultAsync();
            if (!ObjectId.TryParse(id, out var oid))
                return null;

            var filter = Builders<FirewallRequestDto>.Filter.Eq(r => r._id, oid);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
        

        public async Task<FirewallRequestDto> CreateAsync(FirewallRequestDto request)
        {
            request.CreatedAt = DateTime.UtcNow;
            request.Status = "Pending";

            await _collection.InsertOneAsync(request);
            return request;
        }

        public async Task<bool> UpdateStatusAsync(string id, string status)
        {

            //var filter = Builders<FirewallRequestDto>.Filter.Eq(r => r.Id, id);
            //var update = Builders<FirewallRequestDto>.Update.Set(r => r.Status, status);
            //var result = await _collection.UpdateOneAsync(filter, update);
            //return result.ModifiedCount > 0;
            if (!ObjectId.TryParse(id, out var oid))
                return false;

            var filter = Builders<FirewallRequestDto>.Filter.Eq(r => r._id, oid);
            var update = Builders<FirewallRequestDto>.Update.Set(r => r.Status, status);
            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;


        }
        public async Task<bool> UpdateStatusByIdOrStatusAsync(string id, string status = null, int? pstatusId = null)
        {
            var filter = Builders<FirewallRequestDto>.Filter.Eq(r => r.Id, id);

            var updateDefinitionBuilder = Builders<FirewallRequestDto>.Update;
            UpdateDefinition<FirewallRequestDto> update = null;

            if (status != null && pstatusId.HasValue)
            {
                update = updateDefinitionBuilder
                    .Set(r => r.Status, status)
                    .Set(r => r.PStatusId, pstatusId.Value);
            }
            else if (status != null)
            {
                update = updateDefinitionBuilder.Set(r => r.Status, status);
            }
            else if (pstatusId.HasValue)
            {
                update = updateDefinitionBuilder.Set(r => r.PStatusId, pstatusId.Value);
            }
            else
            {
                return false;
            }

            var result = await _collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
    }
}

