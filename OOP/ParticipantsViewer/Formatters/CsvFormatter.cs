using ParticipantsViewer.Model;
using System.Collections.Generic;
using ParticipantsViewer.Extensions;
using Microsoft.VisualBasic.FileIO;
using System;

namespace ParticipantsViewer.Formatters
{
    public class CsvFormatter : IFormatter
    {
        private List<Participant> _csvParticipants;

        public CsvFormatter()
        {
            _csvParticipants = new List<Participant>();
        }

        public string ProviderName => "Сервис №3";
        public IEnumerable<Participant> Participants => _csvParticipants;

        public void GetDataFromProvider(string path)
        {
            DeserializeFile(path);
            Participants.AddProviderName(ProviderName);
        }

        private void DeserializeFile(string path)
        {
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    var p = new Participant()
                    {
                        FirstName = fields[0],
                        LastName = fields[1],
                        RegistrationDate = DateTime.Parse(fields[2])
                    };
                    _csvParticipants.Add(p);
                }
            }
        }
    }
}
