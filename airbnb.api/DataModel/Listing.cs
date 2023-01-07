using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace airbnb.api.DataModel
{
    public class Address
    {
        [BsonElement("street")]
        [JsonPropertyName("street")]
        public string Street { get; set; }
        [BsonElement("suburb")]
        [JsonPropertyName("suburb")]
        public string Suburb { get; set; }
        [BsonElement("government_area")]
        [JsonPropertyName("government_area")]
        public string GovernmentArea { get; set; }
        [JsonPropertyName("market")]
        [BsonElement("market")]
        public string Market { get; set; }
        [BsonElement("country")]
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [BsonElement("country_code")]
        [JsonPropertyName("country_code")]
        public string CountryCode{ get; set; }
        [BsonElement("location")]
        [JsonPropertyName("location")]
        public Location Location { get; set; }
    }

    public class Availability
    {
        [JsonPropertyName("availability_30")]
        [BsonElement("availability_30")]
        public int? Availability30Day { get; set; }
        [BsonElement("availability_60")]
        [JsonPropertyName("availability_60")]
        public int? Availability60Day { get; set; }
        [BsonElement("availability_90")]
        [JsonPropertyName("availability_90")]
        public int? Availability90Day { get; set; }
        [BsonElement("availability_365")]
        [JsonPropertyName("availability_365")]
        public int? Availability365Day { get; set; }
    }

    public class Host
    {
        [JsonPropertyName("host_id")]
        [BsonElement("host_id")]
        public string HostId { get; set; }
        [JsonPropertyName("host_url")]
        [BsonElement("host_url")]
        public string HostUrl { get; set; }
        [JsonPropertyName("host_name")]
        [BsonElement("host_name")]
        public string HostName { get; set; }
        [JsonPropertyName("host_location")]
        [BsonElement("host_location")]
        public string HostLocation { get; set; }
        [JsonPropertyName("host_about")]
        [BsonElement("host_about")]
        public string HostAbout { get; set; }
        [JsonPropertyName("host_response_time")]
        [BsonElement("host_response_time")]
        public string HostResponseTime { get; set; }
        public string host_thumbnail_url { get; set; }
        public string host_picture_url { get; set; }
        public string host_neighbourhood { get; set; }
        public int? host_response_rate { get; set; }
        public bool host_is_superhost { get; set; }
        public bool host_has_profile_pic { get; set; }
        public bool host_identity_verified { get; set; }
        public int? host_listings_count { get; set; }
        public int? host_total_listings_count { get; set; }
        public List<string> host_verifications { get; set; }
    }
    public class Images
    {
        public string thumbnail_url { get; set; }
        public string medium_url { get; set; }
        public string picture_url { get; set; }
        public string xl_picture_url { get; set; }
    }
    public class Location
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
        public bool is_location_exact { get; set; }
    }
    public class Review
    {
        public string _id { get; set; }
        public DateTime date { get; set; }
        public string listing_id { get; set; }
        public string reviewer_id { get; set; }
        public string reviewer_name { get; set; }
        public string comments { get; set; }
    }
    public class ReviewScores
    {
        public decimal? review_scores_accuracy { get; set; }
        public decimal? review_scores_cleanliness { get; set; }
        public decimal? review_scores_checkin { get; set; }
        public decimal? review_scores_communication { get; set; }
        public decimal? review_scores_location { get; set; }
        public decimal? review_scores_value { get; set; }
        public decimal? review_scores_rating { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Listing : BaseEntity
    {
        [BsonId]
        public string Id { get; set; }
        public string listing_url { get; set; }
        public string name { get; set; }
        public string summary { get; set; }
        public string space { get; set; }
        public string description { get; set; }
        public string neighborhood_overview { get; set; }
        public string notes { get; set; }
        public string transit { get; set; }
        public string access { get; set; }
        public string interaction { get; set; }
        public string house_rules { get; set; }
        public string property_type { get; set; }
        public string room_type { get; set; }
        public string bed_type { get; set; }
        public string minimum_nights { get; set; }
        public string maximum_nights { get; set; }
        public string cancellation_policy { get; set; }
        public DateTime last_scraped { get; set; }
        public DateTime calendar_last_scraped { get; set; }
        public DateTime first_review { get; set; }
        public DateTime last_review { get; set; }
        public int? accommodates { get; set; }
        public int? bedrooms { get; set; }
        public int? beds { get; set; }
        public int? number_of_reviews { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? bathrooms { get; set; }
        public List<string> amenities { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        //https://stackoverflow.com/questions/43127406/mongodb-linq-provider-incorrect-behavior-for-fields-of-type-decimal
        public decimal? price { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? weekly_price { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? monthly_price { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? security_deposit { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? cleaning_fee { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? extra_people { get; set; }
        public int? guests_included { get; set; }
        public Images images { get; set; }
        public Host host { get; set; }
        public Address address { get; set; }
        public Availability availability { get; set; }
        public ReviewScores review_scores { get; set; }
        public List<Review> reviews { get; set; }
        public int? reviews_per_month{ get; set; }

        //[BsonExtraElements]
        //public BsonDocument CatchAll { get; set; }

    }


}
