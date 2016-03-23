using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Logic;
using NewsPortal.Web.Models;

namespace NewsPortal.Web.Controllers
{
    public class LanguageController : BaseController
    {
        public LanguageController(IAuthenticationData authenticationData) : base(authenticationData)
        {}

        /// <summary>
        /// Панель выбора языка
        /// </summary>
        /// <returns></returns>
        public ActionResult LanguagePanel()
        {
            List<LanguageLinkData> languageLinks = new List<LanguageLinkData>()
            {
                new LanguageLinkData()
                {
                    Language = Language.Rus,
                    DisplayName = "Рус",
                    Selected = Language == Language.Rus
                },
                new LanguageLinkData()
                {
                    Language = Language.Eng,
                    DisplayName = "Eng",
                    Selected = Language == Language.Eng
                }
            };
            return PartialView("LanguagePanel", languageLinks);
        }

        /// <summary>
        /// Выбор языка
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public ActionResult SelectLanguage(Language language)
        {
            Language = language;
            return RedirectToAction("Index", "NewsFeed");
        }
    }
}
