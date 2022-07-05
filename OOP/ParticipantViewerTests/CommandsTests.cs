using System;
using System.Collections.Generic;
using Xunit;
using ParticipantsViewer.Model;
using ParticipantsViewer.Repository;
using ParticipantsViewer.Commands;

namespace ParticipantViewerTests
{
    public class TestParticipantsRepos : IParticipantsRepository
    {
        public List<Participant> Participants { get; set; }

        public TestParticipantsRepos()
        {
            Participants = new List<Participant>()
            {
            new Participant
            {
                FirstName = "Дмитрий",
                LastName = "Елисеев",
                RegistrationDate = new DateTime(2020, 1, 1, 12, 35, 00),
            },
            new Participant
            {
                FirstName = "Дмитрий",
                LastName = "Елисеев",
                RegistrationDate = new DateTime(2019, 1, 1, 12, 35, 00),
            },
            new Participant
            {
                FirstName = "Дмитрий",
                LastName = "Елисеев",
                RegistrationDate = new DateTime(2018, 1, 1, 12, 35, 00),
            }
            };
        }
    }
    public class CommandsTests
    {
        [Fact]
        public void GetPageCommand_Test()
        {
            // Arrange
            string page = "1";
            CommandsRepository commandsRepository = new();
            TestParticipantsRepos testParticipantsRepos = new();
            GetPageCommand getPageCommand = new(testParticipantsRepos, page);
            var expected = new List<Participant>()
            {
                new Participant {
                FirstName = "Дмитрий",
                LastName = "Елисеев",
                RegistrationDate = new DateTime(2018, 1, 1, 12, 35, 00),
                }
            };

            // Act
            getPageCommand.Execute();
            var actual = getPageCommand.Repository;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
