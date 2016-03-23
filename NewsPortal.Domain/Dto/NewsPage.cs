using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Domain.Dto
{
    public class NewsPage : List<NewsText>
    {
        public bool MoreRecords
        {
            get; 
            set;
        }

        public int PageNumber
        {
            get;
            set;
        }

        public Language Language
        {
            get; 
            set;
        }
    }
}
