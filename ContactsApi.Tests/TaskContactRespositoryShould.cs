using Xunit;
using Moq;
using ContactsApi.Data.Interfaces;
using ContactsApi.Models;

namespace ContactsApi.Tests
{
    public class TaskContactRespositoryShould
    {
        [Fact]
        public void CreateTask_Should_CallRepositoryAdd()
        {
            // Arrange
            var mockRepository = new Mock<ITaskRepositoryContact>();
            var taskService = new TaskServiceTesting(mockRepository.Object);
            var newTask = new Contact
            {
                Name = "Matias Espinosa",
                Company = "Company1",
                Email = "user@example.com",
                PhoneNumber = "1173683928",
                Address = "Street731 Beahan Canyon, Formosa, Argentina",
                ProfileImage = "string",
                BirthDate = DateOnly.Parse("1998-05-04"),
            };

            // Act
            taskService.Add(newTask);

            // Assert
            mockRepository.Verify(repo => repo.Add(newTask), Times.Once);
        }

        [Fact]
        public void UpdateTask_Should_CallRepositoryUpdate()
        {
            // Arrange
            var mockRepository = new Mock<ITaskRepositoryContact>();
            var taskService = new TaskServiceTesting(mockRepository.Object);
            var existingTask = new Contact
            {
                Name = "Matias Espinosa",
                Company = "Company378",
                PhoneNumber = "1122334455",
                Address = "Street7901 Stacie Neck X6120, Córdoba,Argentina",
                ProfileImage = "string",
                BirthDate = DateOnly.Parse("1998-05-04"),
            };
            // Act
            taskService.Update(existingTask);

            // Assert
            mockRepository.Verify(repo => repo.Update(existingTask), Times.Once);
        }
    }
    public class TaskServiceTesting
    {
        private readonly ITaskRepositoryContact _taskRepositoryContact;
        public TaskServiceTesting(ITaskRepositoryContact taskRepository)
        {
            _taskRepositoryContact = taskRepository;
        }
        public void Add(Contact contact)
        {
            _taskRepositoryContact.Add(contact);
        }

        public void Update(Contact contact)
        {
            _taskRepositoryContact.Update(contact);
        }
    }
}
