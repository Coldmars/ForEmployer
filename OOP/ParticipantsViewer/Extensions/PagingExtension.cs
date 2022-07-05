using System.Collections.Generic;
using System.Linq;

namespace ParticipantsViewer.Extensions
{
    public static class PagingExtension
    {
        public static IEnumerable<TSource> Page<TSource>
            (this IEnumerable<TSource> source, int page, int pageSize) =>
            source.Skip((page - 1) * pageSize).Take(pageSize);
    }
}
