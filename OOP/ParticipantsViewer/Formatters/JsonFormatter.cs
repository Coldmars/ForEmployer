using ParticipantsViewer.Model;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ParticipantsViewer.Extensions;

namespace ParticipantsViewer.Formatters
{
    public class JsonFormatter : IFormatter
    {
        private List<Participant> _jsonParticipants;

        public JsonFormatter()
        {
            _jsonParticipants = new List<Participant>();
        }

        public string ProviderName => "Сервис №1";
        public IEnumerable<Participant> Participants => _jsonParticipants;

        public void GetDataFromProvider(string path)
        {
            DeserializeFile(path);
            Participants.AddProviderName(ProviderName);   
        }

        private void DeserializeFile(string path)
        {
            using StreamReader file = File.OpenText(path);
            JsonSerializer serializer = new();
            _jsonParticipants = (List<Participant>)serializer.Deserialize(file, typeof(List<Participant>));
        }
    }
}
