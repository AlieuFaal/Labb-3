using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.ObjectModel;

namespace Labb_3.Model
{
    public enum Difficulty {Easy, Medium, Hard}

    //public enum Category
    //{
    //    General, 
    //    Sports,
    //    Entertainment,
    //    Vehicles,
    //    History,
    //    Geography,
    //    Science
    //}
    
    public class QuestionPack
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public string Name { get; set; }

        public Difficulty Difficulty { get; set; }

        public int TimeLimit { get; set; }

        public string Category { get; set; }

        public List<Question> Questions { get; set; }

        public static ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>
        {
            "General",
            "Sports",
            "Entertainment",
            "Vehicles",
            "History",
            "Geography",
            "Science"
        };

        public QuestionPack()
        {
            Name = string.Empty;
            Difficulty = Difficulty.Medium;
            TimeLimit = 30;
            Questions = new List<Question>();
            Category = "General";
        }

        public QuestionPack(string name, Difficulty difficulty = Difficulty.Medium, int timeLimit = 30, string category = "General")
        {
            Name = name;
            Difficulty = difficulty;
            TimeLimit = timeLimit;
            Questions = new List<Question>();
            Category = category;
        }
    }
}
