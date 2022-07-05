using System.Collections.Generic;
using ParticipantsViewer.Model;

namespace ParticipantsViewer.Extensions
{
    public static class ProviderInfoExtension
    {
        public static IEnumerable<T> AddProviderName<T>(this IEnumerable<T> source, string providerName)
            where T : Participant
        {
            foreach (var participant in source)
            {
                participant.ProviderName = providerName;
            }
            return source;
        }
    }
}