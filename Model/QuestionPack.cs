using System.Collections.ObjectModel;

namespace Labb_3.Model
{
    public enum Difficulty {Easy, Medium, Hard}
    
    public class QuestionPack
    {
        public string Name { get; set; }

        public Difficulty Difficulty { get; set; }

        public int TimeLimit { get; set; }

        public List<Question> Questions { get; set; }

        public QuestionPack()
        {
            Name = string.Empty;
            Difficulty = Difficulty.Medium;
            TimeLimit = 30;
            Questions = new List<Question>();
        }

        public QuestionPack(string name, Difficulty difficulty = Difficulty.Medium, int timeLimit = 30)
        {
            Name = name;
            Difficulty = difficulty;
            TimeLimit = timeLimit;
            Questions = new List<Question>();
        }
    }
}
