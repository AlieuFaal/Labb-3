using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Labb_3.Model
{
    public class Question : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public ObjectId QuestionPackId { get; set; }

        private string _query = string.Empty;
        public string Query
        {
            get => _query;
            set
            {
                if (_query != value)
                {
                    _query = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private string _correctAnswer = string.Empty;
        public string CorrectAnswer
        {
            get => _correctAnswer;
            set
            {
                if (_correctAnswer != value)
                {
                    _correctAnswer = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _incorrectAnswer1 = string.Empty;
        public string IncorrectAnswer1
        {
            get => _incorrectAnswer1;
            set
            {
                if (_incorrectAnswer1 != value)
                {
                    _incorrectAnswer1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _incorrectAnswer2 = string.Empty;
        public string IncorrectAnswer2
        {
            get => _incorrectAnswer2;
            set
            {
                if (_incorrectAnswer2 != value)
                {
                    _incorrectAnswer2 = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _incorrectAnswer3 = string.Empty;
        public string IncorrectAnswer3
        {
            get => _incorrectAnswer3;
            set
            {
                if (_incorrectAnswer3 != value)
                {
                    _incorrectAnswer3 = value;
                    OnPropertyChanged();
                }
            }
        }

        private string[] _incorrectAnswers = Array.Empty<string>();
        public string[] IncorrectAnswers
        {
            get => _incorrectAnswers;
            set
            {
                if (_incorrectAnswers != value)
                {
                    _incorrectAnswers = value;
                    OnPropertyChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Question(string query, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2, string incorrectAnswer3)
        {
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = new string[3] { incorrectAnswer1, incorrectAnswer2, incorrectAnswer3 };
        }

        public List<string> ShuffleAnswers()
        {
            var answers = IncorrectAnswers.Append(CorrectAnswer).ToList();
            return answers.OrderBy(_ => Guid.NewGuid()).ToList();
        }
    }
}