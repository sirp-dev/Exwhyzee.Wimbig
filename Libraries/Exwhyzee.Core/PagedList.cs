using System;
using System.Collections.Generic;
using System.Linq;

namespace Exwhyzee
{
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [Serializable]
    public class PagedList<T> : IPagedList<T> 
    {
        /**
         * NOTE: DONOT CHNAGE THIS, Rather adjust the caller object or method to allign with this implementation
         * 
         * Removed inheritance from IList: This defeats the pagination result value : 16/12/2018
         * Removed the addRange implementation on all constructor, because it converts the result to a list of List<pagedList<T>> rather 
         * than a PagedList<T> having list of T objects and other needed properties. Result Tested and returns the appropraite result.
         **/

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int total = source.Count();
            this.TotalCount = total;
            this.Source = source;
            this.TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / pageSize;
            Source = source;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int filteredCount, int totalCount)
        {
            FilteredCount = filteredCount;
            TotalCount = totalCount;
            Source = source;

            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
        }

        /// <summary>
        ///  Recieves a List of entities
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="filteredCount"></param>
        /// <param name="totalCount"></param>
        public PagedList(IList<T> source, int pageIndex, int pageSize, int filteredCount, int totalCount)
        {
            FilteredCount = filteredCount;
            TotalCount = totalCount;
            Source = source;

            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
      
        }
        public IEnumerable<T> Source { get; set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int FilteredCount { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }
       
           
    }    
}
