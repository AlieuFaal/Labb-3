using Labb_3.Model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

public class QuizDbContext : DbContext
{

    public DbSet<QuestionPack> QuestionPacks { get; set; }
    public DbSet<Question> Questions { get; set; }
    public IMongoCollection<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#error  var connectionString = "Insert your connection string here";
        var collection = "AlieuFaalDb";

        optionsBuilder.UseMongoDB(connectionString, collection);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>()
            .HasKey(q => q.Id);

        modelBuilder.Entity<QuestionPack>()
            .HasKey(qp => qp.Id);
    }

    public QuizDbContext()
    {
        var client = new MongoClient("mongodb://localhost:27017/");
        var database = client.GetDatabase("AlieuFaalDb");

        var questionPackCollection = database.GetCollection<QuestionPack>("QuestionPacks");
        var questionCollection = database.GetCollection<Question>("Questions");
        Categories = database.GetCollection<Category>("Categories");

        this.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }
}