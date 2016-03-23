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
    public class AccountController : BaseController
    {
        private IAccountLogic _accountLogic;

        public AccountController(IAccountLogic accountLogic, IAuthenticationData authenticationData)
            : base(authenticationData)
        {
            _accountLogic = accountLogic;
        }
        
        /// <summary>
        /// Перейти в настройки пользователя
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateAccount()
        {
            Account account = _accountLogic.GetAccount();
            return account == null
                ? (ActionResult)RedirectToAction("Login", "Authentification")
                : View("UpdateAccount", new UserParameters(account));
        }

        /// <summary>
        /// Сохранить настройки пользователя
        /// </summary>
        /// <param name="userParameters"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateAccount(UserParameters userParameters)
        {
            _accountLogic.SaveAccount(userParameters.ToUserDataRecord());
            return RedirectToAction("Index", "NewsFeed");
        }
    }
}
