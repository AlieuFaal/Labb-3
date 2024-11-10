using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Labb_3.Model;

using Labb_3.ViewModel;
using static System.Windows.Forms.Design.AxImporter;

namespace Labb_3.Services
{
    public class QuestionPackService
    {
        private static readonly string QuestionPacksFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Quiz Configurator", "QuestionPacks.json");



        public async Task SaveQuestionPacksAsync(ObservableCollection<QuestionPackViewModel> packs)
        {
            var directory = Path.GetDirectoryName(QuestionPacksFilePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var existingPacks = await LoadQuestionPacksAsync();
            foreach (var pack in packs)
            {
                if (!existingPacks.Any(p => p.Model.Name == pack.Model.Name))
                {
                    existingPacks.Add(pack);
                }
            }

            var questionPacks = existingPacks.Select(p => p.Model).ToList();

            var json = JsonSerializer.Serialize(questionPacks, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(QuestionPacksFilePath, json);
        }



        public async Task<ObservableCollection<QuestionPackViewModel>> LoadQuestionPacksAsync()
        {
            if (!File.Exists(QuestionPacksFilePath))
            {
                return new ObservableCollection<QuestionPackViewModel>();
            }
            var json = await File.ReadAllTextAsync(QuestionPacksFilePath);
            var questionPacks = JsonSerializer.Deserialize<List<QuestionPack>>(json) ?? new List<QuestionPack>();

            var viewModelPacks = new ObservableCollection<QuestionPackViewModel>(questionPacks.Select(p => new QuestionPackViewModel(p)));
            return viewModelPacks;
        }



        public async Task RemoveQuestionPackAsync(QuestionPackViewModel packToRemove)
        {
            var packs = await LoadQuestionPacksAsync();
            var pack = packs.FirstOrDefault(p => p.Model.Name == packToRemove.Model.Name);
            if (pack != null)
            {
                packs.Remove(pack);
                var questionPacks = packs.Select(p => p.Model).ToList();
                var json = JsonSerializer.Serialize(questionPacks, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(QuestionPacksFilePath, json);
            }
        }


        public async Task RemoveQuestionAsync(QuestionPackViewModel pack, Question questionToRemove)
        {
            var packs = await LoadQuestionPacksAsync();
            var packToUpdate = packs.FirstOrDefault(p => p.Model.Name == pack.Model.Name);
            if (packToUpdate != null)
            {
                var question = packToUpdate.Model.Questions.FirstOrDefault(q => q.Query == questionToRemove.Query);
                if (question != null)
                {
                    packToUpdate.Model.Questions.Remove(question);
                    var questionPacks = packs.Select(p => p.Model).ToList();
                    var json = JsonSerializer.Serialize(questionPacks, new JsonSerializerOptions { WriteIndented = true });
                    await File.WriteAllTextAsync(QuestionPacksFilePath, json);
                }
            }
        }


        public async Task UpdateQuestionPackAsync(QuestionPackViewModel updatedPack, string originalName)
        {
            var packs = await LoadQuestionPacksAsync();
            var pack = packs.FirstOrDefault(p => p.Model.Name == originalName);
            if (pack != null)
            {
                pack.Model.Name = updatedPack.Model.Name;
                pack.Model.Difficulty = updatedPack.Model.Difficulty;
                pack.Model.TimeLimit = updatedPack.Model.TimeLimit;
                pack.Model.Questions = updatedPack.Model.Questions;

                var questionPacks = packs.Select(p => p.Model).ToList();
                var json = JsonSerializer.Serialize(questionPacks, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(QuestionPacksFilePath, json);
            }

        }

        public async Task SaveQuestionsAsync(List<Question> questions, QuestionPackViewModel packName)
        {
            var existingPacks = await LoadQuestionPacksAsync();

            var packToUpdate = existingPacks.FirstOrDefault(p => p.Model.Name == packName.Model.Name);
            if (packToUpdate != null)
            {
                packToUpdate.Model.Questions = questions;
            }
            else
            {
                var newPack = new QuestionPackViewModel(new QuestionPack
                {
                    Name = packName.Model.Name,
                    Difficulty = packName.Model.Difficulty,
                    TimeLimit = packName.Model.TimeLimit,
                    Questions = questions
                });
                existingPacks.Add(newPack);
            }

            var questionPacks = existingPacks.Select(p => p.Model).ToList();
            var json = JsonSerializer.Serialize(questionPacks, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(QuestionPacksFilePath, json);
        }
    }
}