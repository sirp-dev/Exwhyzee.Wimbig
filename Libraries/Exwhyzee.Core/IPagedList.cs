using System.Collections.Generic;

namespace Exwhyzee
{
    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList<T> 
    {
        int PageIndex { get; }
        int PageSize { get; }
        int FilteredCount { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}
