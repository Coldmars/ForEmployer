using ParticipantsViewer.Model;
using System.Collections.Generic;
using System.Linq;

namespace ParticipantsViewer.Extensions
{
    public static class SearchExtension
    {
        public static IEnumerable<T> Search<T>(this IEnumerable<T> source, string query)
            where T : Participant =>
            source.Where(participant => participant.FirstName.ToLower().Contains(query.ToLower()) ||
                                        participant.LastName.ToLower().Contains(query.ToLower()));
    }
}