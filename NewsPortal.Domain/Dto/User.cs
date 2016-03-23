﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Domain.Dto
{
    public class User : Entity<Guid>
    {
        public string Name
        {
            get; 
            set;
        }

        public ICollection<Role> Roles
        {
            get;
            set;
        }
    }
}
