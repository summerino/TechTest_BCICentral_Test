using Moq;
using TechTest_BCICentral.Controllers;
using TechTest_BCICentral.Models;
using TechTest_BCICentral.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TechTest_BCICentral_Test
{
    public class ConstructionProjectsControllerTests
    {
        private readonly Mock<IConstructionProjectService> _mockService;
        private readonly ConstructionProjectsController _controller;

        public ConstructionProjectsControllerTests()
        {
            _mockService = new Mock<IConstructionProjectService>();
            _controller = new ConstructionProjectsController(_mockService.Object);
        }

        [Fact]
        public async Task GetListConstructionProject_ReturnsOkResult_WithListOfProjects()
        {
            // Arrange
            var projects = new List<ConstructionProject> { new ConstructionProject(), new ConstructionProject() };
            _mockService.Setup(service => service.GetData()).ReturnsAsync(projects);

            // Act
            var result = await _controller.GetListConstructionProject();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProjects = Assert.IsAssignableFrom<IEnumerable<ConstructionProject>>(okResult.Value);
            Assert.Equal(2, returnProjects.Count());
        }

        [Fact]
        public async Task GetConstructionProjectById_ReturnsOkResult_WithProject()
        {
            // Arrange
            var projectId = "1";
            var project = new ConstructionProject { ProjectId = projectId };
            _mockService.Setup(service => service.GetDataById(projectId)).ReturnsAsync(project);

            // Act
            var result = await _controller.GetConstructionProjectById(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProject = Assert.IsType<ConstructionProject>(okResult.Value);
            Assert.Equal(projectId, returnProject.ProjectId);
        }

        [Fact]
        public async Task GetConstructionProjectById_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = "1";
            _mockService.Setup(service => service.GetDataById(projectId)).ReturnsAsync((ConstructionProject)null);

            // Act
            var result = await _controller.GetConstructionProjectById(projectId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task InsertConstructionProject_ReturnsCreatedAtAction_WhenModelIsValid()
        {
            // Arrange
            var project = new ConstructionProject();
            _mockService.Setup(service => service.InsertData(project)).ReturnsAsync(project);

            // Act
            var result = await _controller.InsertConstructionProject(project);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("InsertConstructionProject", createdResult.ActionName);
        }

        [Fact]
        public async Task InsertConstructionProject_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var project = new ConstructionProject();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.InsertConstructionProject(project);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task UpdateConstructionProject_ReturnsOkResult_WhenModelIsValid()
        {
            // Arrange
            var project = new ConstructionProject { ProjectId = "1" };
            _mockService.Setup(service => service.UpdateData(project)).ReturnsAsync(project);

            // Act
            var result = await _controller.UpdateConstructionProject(project);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateConstructionProject_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var project = new ConstructionProject();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.UpdateConstructionProject(project);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteConstructionProject_ReturnsOkResult_WhenProjectIsDeleted()
        {
            // Arrange
            var projectId = "1";
            _mockService.Setup(service => service.DeleteData(projectId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteConstructionProject(projectId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteConstructionProject_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = "1";
            _mockService.Setup(service => service.DeleteData(projectId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteConstructionProject(projectId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}