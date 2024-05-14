using LXP.Common.Entities;
using LXP.Data.DBContexts;
using LXP.Data.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.InMemory;

namespace LXP.Data.Tests
{
    [TestFixture]

    public class LXPTestDbContext : DbContext
    {
        public LXPTestDbContext(DbContextOptions<LXPTestDbContext> options) : base(options)
        {
        }

        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("LXPTestDb");
            }
        }
    }
    public class EnrollmentRepositoryTests
    {
        private LXPDbContext _dbContext;
        private EnrollmentRepository _enrollmentRepository;

        [SetUp]
        public void Setup()
        {
            // Create an in-memory database context for testing
            var options = new DbContextOptionsBuilder<LXPDbContext>()
                .UseInMemoryDatabase(databaseName: "LXPTestDb")
                .Options;

            var builder = new DbContextOptionsBuilder<LXPDbContext>();

            _dbContext = new LXPDbContext(builder.UseInMemoryDatabase("LXPTestDb").Options);
            _enrollmentRepository = new EnrollmentRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task Addenroll_ShouldAddEnrollmentToDatabase()
        {
            // Arrange
            var enrollment = new Enrollment
            {
                LearnerId = Guid.NewGuid(),
                CourseId = Guid.NewGuid(),
                EnrollmentDate = DateTime.Now
            };

            // Act
            await _enrollmentRepository.Addenroll(enrollment);

            // Assert
            var addedEnrollment = await _dbContext.Enrollments.FirstOrDefaultAsync(e => e.LearnerId == enrollment.LearnerId && e.CourseId == enrollment.CourseId);
            Assert.IsNotNull(addedEnrollment);
            Assert.AreEqual(enrollment.LearnerId, addedEnrollment.LearnerId);
            Assert.AreEqual(enrollment.CourseId, addedEnrollment.CourseId);
            Assert.AreEqual(enrollment.EnrollmentDate, addedEnrollment.EnrollmentDate);
        }

        [Test]
        public void AnyEnrollmentByLearnerAndCourse_ShouldReturnTrue_WhenEnrollmentExists()
        {
            // Arrange
            var learnerId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var enrollment = new Enrollment
            {
                LearnerId = learnerId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now
            };
            _dbContext.Enrollments.Add(enrollment);
            _dbContext.SaveChanges();

            // Act
            var result = _enrollmentRepository.AnyEnrollmentByLearnerAndCourse(learnerId, courseId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AnyEnrollmentByLearnerAndCourse_ShouldReturnFalse_WhenEnrollmentDoesNotExist()
        {
            // Arrange
            var learnerId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            // Act
            var result = _enrollmentRepository.AnyEnrollmentByLearnerAndCourse(learnerId, courseId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}