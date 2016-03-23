using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Logic;

namespace NewsPortal.Web.Controllers
{
    /// <summary>
    /// Базовый контроллер
    /// </summary>
    public abstract class BaseController : Controller
    {
        protected IAuthenticationData AuthenticationData;

        protected BaseController(IAuthenticationData authenticationData)
        {
            AuthenticationData = authenticationData;
        }

        private Language? _language;

        /// <summary>
        /// Язык
        /// </summary>
        protected Language Language
        {
            get
            {
                if (!_language.HasValue)
                {
                    _language = (Language?)Session["Language"] ?? (Language)(Session["Language"] = Language.Rus);
                }
                return _language.Value;
            }
            set
            {
                _language = value;
                Session["Language"] = value;
            }
        }
    }
}
