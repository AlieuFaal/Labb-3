﻿namespace Labb_3.Model
{
    public class Question
    {
        public string Query { get; set; }
        public string CorrectAnswer { get; set; }
        public string IncorrectAnswer1 { get; }
        public string IncorrectAnswer2 { get; }
        public string IncorrectAnswer3 { get; }
        public string[] IncorrectAnswers { get; set; }

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