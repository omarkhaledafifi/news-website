using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.DTOs.UserRequests
{
    public class ArticleQueryParameters
    {
        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;
        public string? AuthorId { get; set; }
        public ArticleSortingOptions Options { get; set; }
        public string? SearchInTitles { get; set; }
        public string? SearchInAothors { get; set; }
        public string? SearchInSummary { get; set; }
        public string? SearchBetweenDateBegin { get; set; }
        public string? SearchBetweenDateEnd { get; set; }
        public int PageIndex { get; set; } = 1;
        private int _pageSize = DefaultPageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 0 && value < MaxPageSize ? value : DefaultPageSize;
        }
    }
}
