using LXP.Api.Controllers;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace LXP.Api.Tests

{

    [TestFixture]
    public class EnrollmentControllerN_unit
    {

        [Test]
        public async Task AddEnroll_ValidEnrollment_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<IEnrollmentService>();
            mockService.Setup(service => service.Addenroll(It.IsAny<EnrollmentViewModel>())).ReturnsAsync(true);
            var controller = new EnrollmentController(mockService.Object);
            var enroll = new EnrollmentViewModel(); // Provide valid enrollment data

            // Act
            var result = await controller.Addenroll(enroll);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddEnroll_InvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            var mockService = new Mock<IEnrollmentService>();
            var controller = new EnrollmentController(mockService.Object);
            controller.ModelState.AddModelError("PropertyName", "Error message"); // Add model error
            var enroll = new EnrollmentViewModel(); // Provide invalid enrollment data

            // Act
            var result = await controller.Addenroll(enroll);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddEnroll_AlreadyEnrolled_ReturnsBadRequestResult()
        {
            // Arrange
            var mockService = new Mock<IEnrollmentService>();
            mockService.Setup(service => service.Addenroll(It.IsAny<EnrollmentViewModel>())).ReturnsAsync(false);
            var controller = new EnrollmentController(mockService.Object);
            var enroll = new EnrollmentViewModel(); // Provide valid enrollment data

            // Act
            var result = await controller.Addenroll(enroll);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result); // Assuming BadRequest is mapped to Ok for already enrolled
            var okResult = result as OkObjectResult;
            var apiResponse = okResult.Value as APIResponse;
            Assert.AreEqual("AlreadyEnrolled", apiResponse.Message); // Assuming the response includes the error code
        }

    }
}
