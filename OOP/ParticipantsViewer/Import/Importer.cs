using System.Collections.Generic;
using ParticipantsViewer.Formatters;
using ParticipantsViewer.Model;

namespace ParticipantsViewer.Import
{
    public class Importer
    {
        private IFormatter _formatter;

        public void ImportDataToRepository(List<Participant> repos, IFormatter formatter, string path)
        {
            SetFormatter(formatter);
            Import(repos, path);
        }

        private void SetFormatter(IFormatter formatter)
        {
            _formatter = formatter;
        }

        private void Import(List<Participant> repos, string path)
        {
            _formatter.GetDataFromProvider(path);
            var importingParticipants = _formatter.Participants;
            repos.AddRange(importingParticipants);
        }
    }
}
