using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ParticipantsViewer.Model
{
    public class ParticipantComparer : IEqualityComparer<Participant>
    {
        public bool Equals(Participant x, Participant y) =>
            (x.FirstName == y.FirstName) &&
            (x.LastName == y.LastName);

        public int GetHashCode([DisallowNull] Participant obj) =>
            (obj.FirstName + obj.LastName).GetHashCode();
    }
}