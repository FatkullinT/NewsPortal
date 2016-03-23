using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Web.Models
{
    public class NewsPageModel
    {
        public NewsPageModel(NewsPage newsPage, bool isAdministrator)
        {
            Language = newsPage.Language;
            News = newsPage.Select(newsRecord=>new NewsTextModel(newsRecord)).ToArray();
            MoreRecords = newsPage.MoreRecords;
            NextPageNumber = newsPage.PageNumber;
            IsAdministrator = isAdministrator;
        }

        public NewsTextModel[] News
        {
            get;
            set;
        }

        public bool MoreRecords
        {
            get;
            set;
        }

        public int NextPageNumber
        {
            get; 
            set;
        }

        public Language Language
        {
            get; 
            set;
        }

        public bool IsAdministrator
        {
            get;
            set;
        }
    }
}