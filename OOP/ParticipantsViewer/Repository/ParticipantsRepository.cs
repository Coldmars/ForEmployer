using ParticipantsViewer.Model;
using System.Collections.Generic;

namespace ParticipantsViewer.Repository
{
    public class ParticipantsRepository : IParticipantsRepository
    {
        public List<Participant> Participants { get; set; }

        public ParticipantsRepository()
        {
            Participants = new List<Participant>();
        }
    }
}
