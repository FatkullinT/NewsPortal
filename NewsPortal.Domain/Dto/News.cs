using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Domain.Dto
{
    public class News : Entity<Guid>
    {
        public DateTime Date
        {
            get; 
            set; 
        }

        public bool IsActive
        {
            get;
            set;
        }

        public ICollection<NewsText> NewsTexts
        {
            get;
            set;
        }

        public bool AllowAnonymous
        {
            get;
            set;
        }
    }
}
