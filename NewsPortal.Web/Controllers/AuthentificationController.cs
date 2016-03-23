using System.Web.Mvc;
using NewsPortal.Domain.Logic;
using NewsPortal.Web.Classes;
using NewsPortal.Web.Models;

namespace NewsPortal.Web.Controllers
{
    public class AuthentificationController : BaseController
    {
        private IAuthentication _authentication;

        public AuthentificationController(IAuthenticationData authenticationData, IAuthentication authentication)
            : base(authenticationData)
        {
            _authentication = authentication;
        }

        /// <summary>
        /// Приветствие
        /// </summary>
        /// <returns></returns>
        public ActionResult LoggedInUser()
        {
            return PartialView("AuthenticationDataForm", AuthenticationData.CurrentUserName);
        }

        /// <summary>
        /// Вход по логину/паролю
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            ViewData["ProviderNames"] = _authentication.OAuthProviderNames;
            return View("LoginForm");
        }

        /// <summary>
        /// Вход по логину/паролю
        /// </summary>
        /// <param name="loginDataModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginDataModel loginDataModel)
        {
            if (ModelState.IsValid)
            {
                if (_authentication.Login(loginDataModel.UserName, loginDataModel.Password, loginDataModel.RememberMe))
                {
                    return RedirectToAction("Index", "NewsFeed");
                }
                ModelState.AddModelError("", "Данные не верны");
            }
            ViewData["ProviderNames"] = _authentication.OAuthProviderNames;
            return View("LoginForm", loginDataModel);
        }

        /// <summary>
        /// Выход
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            _authentication.Logout();
            return RedirectToAction("Index", "NewsFeed");
        }

        /// <summary>
        /// Вход через OAuth
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public ActionResult OAuthLogin(string providerName)
        {
            return new OAuthRedirect(_authentication, providerName, Url.Action("OAuthLoginCallBack", "Authentification"));
        }

        /// <summary>
        /// Метод, вызываемый после прохождения аутентификации через OAuth
        /// </summary>
        /// <returns></returns>
        public ActionResult OAuthLoginCallBack()
        {
            bool newUser;
            if (_authentication.OAuthLogin(out newUser))
            {
                return newUser ? RedirectToAction("UpdateAccount", "Account") : RedirectToAction("Index", "NewsFeed");
            }
            else
            {
                ModelState.AddModelError("", "Аутентификация неудачна");
                return Login();
            }
        }
    }
}
