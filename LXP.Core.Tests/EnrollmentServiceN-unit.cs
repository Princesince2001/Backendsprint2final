using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.Services;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq.Language.Flow;

namespace LXP.Core.Tests
{
    [TestFixture]
    public class EnrollmentServiceN_unit
    {
        [Test]
        public async Task Addenroll_NewEnrollment_ReturnsTrue()
        {
            // Arrange
            var mockEnrollmentRepo = new Mock<IEnrollmentRepository>();
            mockEnrollmentRepo.Setup(repo => repo.AnyEnrollmentByLearnerAndCourse(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((false));
            var mockLearnerRepo = new Mock<ILearnerRepository>();
            var mockCourseRepo = new Mock<ICourseRepository>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var service = new EnrollmentService(mockEnrollmentRepo.Object, mockLearnerRepo.Object, mockCourseRepo.Object, mockWebHostEnvironment.Object, mockHttpContextAccessor.Object);
            var enrollment = new EnrollmentViewModel
            {
                LearnerId = Guid.NewGuid(),
                CourseId = Guid.NewGuid(),
            };

            // Act
            var result = await service.Addenroll(enrollment);

            // Assert
            Assert.IsTrue(result);
            mockEnrollmentRepo.Verify(repo => repo.Addenroll(It.IsAny<Enrollment>()), Times.Once);
        }

        [Test]
        public async Task Addenroll_ExistingEnrollment_ReturnsFalse()
        {
            // Arrange
            var mockEnrollmentRepo = new Mock<IEnrollmentRepository>();
            mockEnrollmentRepo.Setup(repo => repo.AnyEnrollmentByLearnerAndCourse(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((true));
            var mockLearnerRepo = new Mock<ILearnerRepository>();
            var mockCourseRepo = new Mock<ICourseRepository>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var service = new EnrollmentService(mockEnrollmentRepo.Object, mockLearnerRepo.Object, mockCourseRepo.Object, mockWebHostEnvironment.Object, mockHttpContextAccessor.Object);
            var enrollment = new EnrollmentViewModel
            {
                LearnerId = Guid.NewGuid(),
                CourseId =  Guid.NewGuid()
            };

            // Act
            var result = await service.Addenroll(enrollment);

            // Assert
            Assert.IsFalse(result);
            mockEnrollmentRepo.Verify(repo => repo.Addenroll(It.IsAny<Enrollment>()), Times.Never);
        }
    }
}

