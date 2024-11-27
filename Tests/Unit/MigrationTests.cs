using Microsoft.EntityFrameworkCore;
using Server.Database;
using Shared.Models;
using Xunit;

namespace Server.Tests
{
    public class MigrationTests
    {
        private readonly ReadingSpeedDbContext _context;

        public MigrationTests()
        {
            var options = new DbContextOptionsBuilder<ReadingSpeedDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ReadingSpeedDbContext(options);
        }

        [Fact]
        public void Database_ShouldHaveCorrectTablesWithoutMigration()
        {
            // Act
            var paragraphTableExists = _context.Model.FindEntityType(typeof(ParagraphEntity)) != null;
            var questionTableExists = _context.Model.FindEntityType(typeof(QuestionEntity)) != null;

            // Assert
            Assert.True(paragraphTableExists, "ParagraphEntity table should exist.");
            Assert.True(questionTableExists, "QuestionEntity table should exist.");
        }
    }
}
