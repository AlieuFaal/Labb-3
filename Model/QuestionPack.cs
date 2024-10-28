using System.Collections.ObjectModel;

namespace Labb_3.Model
{
    enum Difficulty {Easy, Medium, Hard}
    internal class QuestionPack
    {
        public string Name { get; set; }

        public Difficulty Difficulty { get; set; }

        public int TimeLimitInSeconds { get; set; }

        public ObservableCollection<Question> Questions { get; set;}

        public QuestionPack(string name, Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
        {
            Name = name;
            Difficulty = difficulty;
            TimeLimitInSeconds = timeLimitInSeconds;
            Questions = new ObservableCollection<Question>();
        }
    }
}
