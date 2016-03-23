
using System;

namespace NewsPortal.Domain.Dto
{
    public class Account : Entity<Guid>
    {
        public Guid UserId
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public bool SendNews
        {
            get;
            set;
        }
    }
}
