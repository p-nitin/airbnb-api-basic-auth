using airbnb.api.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Dynamic;
using System.Reflection;

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

        
        public async Task<List<Listing>> GetAsync(QueryFilter queryParameters, string sort, int pageSize, int offset)
        {

            var builder = Builders<Listing>.Filter;
            var filter = builder.Empty;

            SortDefinition<Listing>? sortFields = BuildSortFields(sort);

            if (!string.IsNullOrWhiteSpace(queryParameters.Name))
            {
                var nameFilter = builder.Where(x => x.name.ToLower().Contains(queryParameters.Name));
                if (filter == builder.Empty)
                {
                    filter = nameFilter;
                }
                else
                {
                    filter &= nameFilter;
                }
            }

            if (queryParameters.Accommodates != null)
            {
                var accommodatesFilter = builder.Eq(x => x.accommodates, queryParameters.Accommodates);
                if (filter == builder.Empty)
                {
                    filter = accommodatesFilter;
                }
                else
                {
                    filter &= accommodatesFilter;
                }
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.PropertyType))
            {
                var propertyFilter = builder.Where(x => x.property_type.ToLower().Contains(queryParameters.PropertyType.ToLower()));
                if (filter == builder.Empty)
                {
                    filter = propertyFilter;
                }
                else
                {
                    filter &= propertyFilter;
                }

            }

            if (queryParameters.Price != null)
            {
                var priceFilter = builder.Where(x => x.price == queryParameters.Price);
                if (filter == builder.Empty)
                {
                    filter = priceFilter;
                }
                else
                {
                    filter &= priceFilter;
                }

            }

            if(sortFields == null)
            {
                return await _listingsCollection.Find(filter)
                        .Skip(offset)
                        .Limit(pageSize)
                        .ToListAsync();
            }
            else
            {
                return await _listingsCollection.Find(filter)
                    .Sort(sortFields)
                    .Skip(offset)
                    .Limit(pageSize)
                    .ToListAsync();
            }

            //if (filter == builder.Empty)
            //{
            //    return await _listingsCollection.Find(_ => true)
            //    .Skip(offset)
            //    .Limit(pageSize)
            //    .ToListAsync();
            //}
            //else
            //{
            //    return await _listingsCollection.Find(filter)
            //    .Skip(offset)
            //    .Limit(pageSize)
            //    .ToListAsync();
            //}
        }

        public async Task<Listing?> GetAsync(string id) =>
            await _listingsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Listing newListing) =>
            await _listingsCollection.InsertOneAsync(newListing);

        public async Task UpdateAsync(string id, Listing updatedListing) =>
            await _listingsCollection.ReplaceOneAsync(x => x.Id == id, updatedListing);

        public async Task RemoveAsync(string id) =>
            await _listingsCollection.DeleteOneAsync(x => x.Id == id);


        private SortDefinition<Listing>? BuildSortFields(string sort)
        {
            var sortBuilder = Builders<Listing>.Sort;

            if (sort != null)
            {
                string[] sortFields = sort.Split(",");
                var sortDefinitions = sortFields.Select(x =>
                {
                    SortDefinition<Listing> sortDef;
                    var propName = x.Substring(1);
                    if (x.StartsWith("-"))
                        sortDef = sortBuilder.Descending(propName);
                    else
                        sortDef = sortBuilder.Ascending(propName);
                    return sortDef;
                });
                return sortBuilder.Combine(sortDefinitions);
            }

            return null;
        }
    }
}
