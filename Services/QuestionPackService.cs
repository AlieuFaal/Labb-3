using System.Collections.ObjectModel;
using Labb_3.Model;
using Labb_3.ViewModel;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Labb_3.Services
{
    public class QuestionPackService
    {
        private readonly QuizDbContext _context;

        public QuestionPackService()
        {
            _context = new QuizDbContext();
        }


        public async Task SaveQuestionPacksAsync(ObservableCollection<QuestionPackViewModel> packs)
        {
            var existingPacks = await LoadQuestionPacksAsync();
            foreach (var pack in packs)
            {
                if (!existingPacks.Any(p => p.Model.Name == pack.Model.Name))
                {
                   _context.QuestionPacks.Add(pack.Model);
                }
            }
            await _context.SaveChangesAsync();
        }


        public async Task<ObservableCollection<QuestionPackViewModel>> LoadQuestionPacksAsync()
        {
            var questionPacks = await _context.QuestionPacks.ToListAsync();

            var viewModelPacks = new ObservableCollection<QuestionPackViewModel>();

            foreach (var pack in questionPacks)
            {
                var questions = await _context.Questions.Where(q => q.QuestionPackId == pack.Id).ToListAsync();
                pack.Questions = questions;
                viewModelPacks.Add(new QuestionPackViewModel(pack, new ObservableCollection<QuestionPackViewModel>()));
            }

            return viewModelPacks;
        }


        public async Task RemoveQuestionPackAsync(QuestionPackViewModel packToRemove)
        {
            var pack = await _context.QuestionPacks.FirstOrDefaultAsync(p => p.Name == packToRemove.Model.Name);
            if (pack != null)
            {
                _context.QuestionPacks.Remove(pack);
                await _context.SaveChangesAsync();
            }
        }


        public async Task RemoveQuestionAsync(QuestionPackViewModel pack, Question questionToRemove)
        {
            var packToUpdate = await _context.QuestionPacks.FirstOrDefaultAsync(p => p.Name == pack.Model.Name);
            if (packToUpdate != null)
            {
                var question = await _context.Questions.FirstOrDefaultAsync(q => q.Query == questionToRemove.Query && q.QuestionPackId == packToUpdate.Id);
                if (question != null)
                {
                    _context.Questions.Remove(question);
                    await _context.SaveChangesAsync();
                }
            }
        }


        public async Task UpdateQuestionPackAsync(QuestionPackViewModel updatedPack, string originalName)
        {
            var pack = await _context.QuestionPacks.FirstOrDefaultAsync(p => p.Name == originalName);
            if (pack != null)
            {
                pack.Name = updatedPack.Model.Name;
                pack.Difficulty = updatedPack.Model.Difficulty;
                pack.TimeLimit = updatedPack.Model.TimeLimit;
                pack.Questions = updatedPack.Model.Questions;
                pack.Category = updatedPack.Model.Category;

                await _context.SaveChangesAsync();
            }

        }


        public async Task SaveQuestionsAsync(List<Question> questions, QuestionPackViewModel packName)
        {
            var packToUpdate = await _context.QuestionPacks.FirstOrDefaultAsync(p => p.Name == packName.Model.Name);
            if (packToUpdate != null)
            {
                var existingQuestions = await _context.Questions.Where(q => q.QuestionPackId == packToUpdate.Id).ToListAsync();

                foreach (var question in questions)
                {
                    var existingQuestion = existingQuestions.FirstOrDefault(q => q.Query == question.Query);
                    if (existingQuestion != null)
                    {
                        existingQuestion.CorrectAnswer = question.CorrectAnswer;
                        existingQuestion.IncorrectAnswer1 = question.IncorrectAnswer1;
                        existingQuestion.IncorrectAnswer2 = question.IncorrectAnswer2;
                        existingQuestion.IncorrectAnswer3 = question.IncorrectAnswer3;
                        existingQuestion.IncorrectAnswers = question.IncorrectAnswers;
                    }
                    else
                    {
                        question.QuestionPackId = packToUpdate.Id;
                        question.Id = ObjectId.GenerateNewId();
                        _context.Questions.Add(question);
                    }
                }

                var questionsToRemove = existingQuestions.Where(eq => !questions.Any(q => q.Query == eq.Query)).ToList();
                _context.Questions.RemoveRange(questionsToRemove);

                packToUpdate.Questions = questions;
            }
            else
            {
                var newPack = new QuestionPack
                {
                    Name = packName.Model.Name,
                    Difficulty = packName.Model.Difficulty,
                    TimeLimit = packName.Model.TimeLimit,
                    Questions = questions,
                    Category = packName.Model.Category
                };
                _context.QuestionPacks.Add(newPack);

                foreach (var question in questions)
                {
                    question.QuestionPackId = newPack.Id;
                    question.Id = ObjectId.GenerateNewId();
                    _context.Questions.Add(question);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task SaveCategoriesAsync(ObservableCollection<string> categories)
        {
            var existingCategories = await LoadCategoriesAsync();
            var categoriesToAdd = categories.Except(existingCategories.Select(c => c.Name)).Select(name => new Category(name)).ToList();
            var categoriesToRemove = existingCategories.Where(c => !categories.Contains(c.Name)).ToList();

            if (categoriesToAdd.Any())
            {
                await _context.Categories.InsertManyAsync(categoriesToAdd);
            }

            if (categoriesToRemove.Any())
            {
                var filter = Builders<Category>.Filter.In(c => c.Id, categoriesToRemove.Select(c => c.Id));
                await _context.Categories.DeleteManyAsync(filter);
            }

            QuestionPack.Categories.Clear();
            foreach (var category in categories)
            {
                QuestionPack.Categories.Add(category);
            }
        }

        public async Task<ObservableCollection<Category>> LoadCategoriesAsync()
        {
            var categories = await _context.Categories.Find(_ => true).ToListAsync();
            var allCategories = new ObservableCollection<Category>(categories);

            foreach (var defaultCategory in QuestionPack.Categories)
            {
                if (!allCategories.Any(c => c.Name == defaultCategory))
                {
                    allCategories.Add(new Category(defaultCategory));
                }
            }

            return allCategories;
        }
    }
}