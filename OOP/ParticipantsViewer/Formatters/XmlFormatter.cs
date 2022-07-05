using ParticipantsViewer.Model;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ParticipantsViewer.Extensions;

namespace ParticipantsViewer.Formatters
{
    public class XmlFormatter : IFormatter
    {
        private List<Participant> _xmlParticipants;

        public XmlFormatter()
        {
            _xmlParticipants = new List<Participant>();
            ProviderName = "Сервис №2";
        }

        public string ProviderName { get; }
        public IEnumerable<Participant> Participants => _xmlParticipants;

        public void GetDataFromProvider(string path)
        {
            DeserializeFile(path);
            Participants.AddProviderName(ProviderName);
        }

        private void DeserializeFile(string path)
        {
            using StreamReader file = File.OpenText(path);
            XmlSerializer serializer = new(typeof(List<Participant>));
            _xmlParticipants = (List<Participant>)serializer.Deserialize(file);
        }
    }
}

