using airbnb.api.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace airbnb.api.Service
{
    public class ListingDataService
    {
        private readonly IMongoCollection<Listing> _listingsCollection;

        public ListingDataService(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(
                configuration["ConnectionString:MongoDb"]);

            var mongoDatabase = mongoClient.GetDatabase(configuration["ConnectionString:DatabaseName"]);

            _listingsCollection = mongoDatabase.GetCollection<Listing>(
                configuration["ConnectionString:CollectionName"]);
        }

        public async Task<List<Listing>> GetAsync(int pageSize, int offset) =>
            await _listingsCollection.Find(_ => true).Skip(offset).Limit(pageSize).ToListAsync();

        public async Task<Listing?> GetAsync(string id) =>
            await _listingsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Listing newListing) =>
            await _listingsCollection.InsertOneAsync(newListing);

        public async Task UpdateAsync(string id, Listing updatedListing) =>
            await _listingsCollection.ReplaceOneAsync(x => x.Id == id, updatedListing);

        public async Task RemoveAsync(string id) =>
            await _listingsCollection.DeleteOneAsync(x => x.Id == id);

    }
}
