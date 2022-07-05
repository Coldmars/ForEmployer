using System;
using System.Collections.Generic;
using Xunit;
using ParticipantsViewer.Formatters;
using ParticipantsViewer.Import;
using ParticipantsViewer.Model;

namespace ParticipantViewerTests
{
    public class FormattersTests
    {
        private readonly List<Participant> expectedParticipants = new()
        {
            new Participant
            {
                FirstName = "Дмитрий",
                LastName = "Елисеев",
                RegistrationDate = new DateTime(2020, 1, 1, 12, 35, 00),
            },
            new Participant
            {
                FirstName = "Роман",
                LastName = "Сидоров",
                RegistrationDate = new DateTime(2020, 1, 22, 16, 04, 00)
            },
            new Participant
            {
                FirstName = "Юрий",
                LastName = "Галкин",
                RegistrationDate = new DateTime(2019, 12, 5, 18, 20, 00)
            }
        };

        [Fact]
        public void JsonFormatter_Test()
        {
            // Arrange
            List<Participant> actualParticipants = new();
            JsonFormatter json = new();
            Importer impoter = new();

            // Act
            impoter.ImportDataToRepository(actualParticipants, json, @"./test.json");

            // Assert
            Assert.Equal(expectedParticipants, actualParticipants);
        }

        [Fact]
        public void XmlFormatter_Test()
        {
            // Arrange
            List<Participant> actualParticipants = new();
            XmlFormatter xml = new();
            Importer impoter = new();

            // Act
            impoter.ImportDataToRepository(actualParticipants, xml, @"./test.xml");

            // Assert
            Assert.Equal(expectedParticipants, actualParticipants);
        }

        [Fact]
        public void CsvFormatter_Test()
        {
            // Arrange
            List<Participant> actualParticipants = new();
            CsvFormatter csv = new();
            Importer impoter = new();

            // Act
            impoter.ImportDataToRepository(actualParticipants, csv, @"./test.csv");

            // Assert
            Assert.Equal(expectedParticipants, actualParticipants);
        }
    }
}
