using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labb_3.Model
{
    public class Category
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public string Name { get; set; }

        public Category(string name)
        {
            Name = name;
        }
    }
}
