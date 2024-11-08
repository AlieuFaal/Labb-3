using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Labb_3.Model;

using Labb_3.ViewModel;

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

            var json = JsonSerializer.Serialize(questionPacks, new JsonSerializerOptions { WriteIndented = true});
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
    }
}