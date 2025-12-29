using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        //Hai biến tiện ích (helper). Frontend chỉ cần check if (HasNextPage == false) thì ẩn nút "Next" đi.

        public PagedResult(List<T> items, int count, int pageNumber, int pageSize)
        {
            items = items;
            TotalCount = count;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            //hàm Math.Ceiling (làm tròn lên) để tính.
        }

        public PagedResult()
        {
            
        }

        // Helper static để tạo nhanh
        //dùng Factory Pattern
        public static PagedResult<T> Create(List<T> items, int count, int pageNumber, int pageSize)
        {
            return new PagedResult<T>(items, count, pageNumber, pageSize);
        }
    }
}
