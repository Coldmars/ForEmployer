using ParticipantsViewer.Model;
using System.Collections.Generic;

namespace ParticipantsViewer.Repository
{
    public interface IParticipantsRepository
    {
        List<Participant> Participants { get; set; }
    }
}
