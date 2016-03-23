using System.Web.Mvc;
using NewsPortal.Domain;
using NewsPortal.Domain.Logic;
using NewsPortal.Domain.Security;

namespace NewsPortal.Web.Classes
{
    /// <summary>
    /// Перенаправление на страницу аутентификации OAuth
    /// </summary>
    public class OAuthRedirect : ActionResult
    {
        private IAuthentication _authentication;

        private string _providerName;
        private string _callBackUrl;

        public OAuthRedirect(IAuthentication authentication, string providerName, string callBackUrl)
        {
            _authentication = authentication;
            _providerName = providerName;
            _callBackUrl = callBackUrl;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            _authentication.OAuthRedirect(_providerName, _callBackUrl);
        }
    }
}