using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Logic;
using NewsPortal.Domain.Security;
using NewsPortal.Web.Classes;
using NewsPortal.Web.Models;

namespace NewsPortal.Web.Controllers
{
    public class NewsFeedController : BaseController
    {
        private INewsLogic _newsLogic;

        public NewsFeedController(INewsLogic newsLogic, IAuthenticationData authenticationData)
            : base(authenticationData)
        {
            _newsLogic = newsLogic;
        }
        
        /// <summary>
        /// Главная
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(AuthenticationData.IsAdministrator);
        }

        /// <summary>
        /// Запрос страницы новостей
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public ActionResult NewsPage(int pageNumber, Language? language = null)
        {
            NewsPage page = _newsLogic.GetNewsPage(pageNumber, language ?? Language);
            return PartialView(new NewsPageModel(page, AuthenticationData.IsAdministrator));
        }

        /// <summary>
        /// Изменить новость
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdministratorOnly]
        public ActionResult Update(Guid id)
        {
            News newsRecord = _newsLogic.GetNewsRecord(id);
            ViewBag.Title = "Редактирование новостей";
            return View("NewsEdit", new NewsUpdateModel(newsRecord));
        }

        /// <summary>
        /// Удалить новость
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdministratorOnly]
        public ActionResult Delete(Guid id)
        {
            _newsLogic.DeactivateNewsRecord(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Добавить новость
        /// </summary>
        /// <returns></returns>
        [AdministratorOnly]
        public ActionResult Create()
        {
            News newsRecord = _newsLogic.GetEmptyNewsRecord();
            ViewBag.Title = "Добавление новостей";
            return View("NewsEdit", new NewsUpdateModel(newsRecord));
        }

        /// <summary>
        /// Сохранить новость
        /// </summary>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AdministratorOnly]
        public ActionResult SaveNews(NewsUpdateModel updateModel)
        {
            _newsLogic.SaveNewsRecord(updateModel.ToNewsRecord());
            return RedirectToAction("Index");
        }
    }
}
