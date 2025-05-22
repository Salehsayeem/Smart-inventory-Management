using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sims.Api.Helper
{
    public class PagingList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public PagingList(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPage = (int)Math.Ceiling(totalCount / (double)pageSize);
            AddRange(items);
        }
        public static PagingList<T> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var totalCount = 0;
            if (source.Any())
            {
                { totalCount = source.Count(); }

            }
            var items = source.Skip(((int)pageNumber - 1) * (int)pageSize).Take((int)pageSize).ToList();

            return new PagingList<T>(items, totalCount, pageNumber, pageSize);
        }
    }
}