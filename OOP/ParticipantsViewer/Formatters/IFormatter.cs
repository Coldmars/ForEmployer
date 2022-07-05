using System.Collections.Generic;
using ParticipantsViewer.Model;

namespace ParticipantsViewer.Formatters
{
    public interface IFormatter
    {
        string ProviderName { get; }
        IEnumerable<Participant> Participants { get; }
        void GetDataFromProvider(string path);
    }
}
