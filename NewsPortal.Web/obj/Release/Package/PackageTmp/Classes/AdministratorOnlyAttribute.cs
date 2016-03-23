using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using NewsPortal.Domain.Security;

namespace NewsPortal.Web.Classes
{
    public class AdministratorOnlyAttribute : ActionFilterAttribute  
    {}
}