using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Domain.Dto
{
    public class NewsText : Entity<Guid>
    {
        public News News
        {
            get;
            set;
        }

        public Language Language
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
    }
}
