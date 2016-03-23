using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NewsPortal.Logic;
using NewsPortal.Dal.Context;
using NewsPortal.Dal.Repository;
using NewsPortal.Dal.UnitOfWork;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Logger;
using NewsPortal.Domain.Logic;
using NewsPortal.Domain.Security;
using NewsPortal.Security;
using NewsPortal.Web.Classes;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Mvc.FilterBindingSyntax;

namespace NewsPortal.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Kernel.Get<IAuthentication>().Initialize();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Kernel.Get<ILogger>().Error(exception);
            Server.ClearError();
            Response.Redirect("/");
        }

        protected override IKernel CreateKernel()
        {
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            BindDataAccessLayer(kernel);
            BindSecurityServices(kernel);
            BindLogic(kernel);
            BindLogger(kernel);
            kernel.BindFilter<AdministratorOnlyFilter>(FilterScope.Action, 0)
                .WhenActionMethodHas<AdministratorOnlyAttribute>();
            return kernel;
        }

        /// <summary>
        /// Биндинг логики
        /// </summary>
        /// <param name="kernel"></param>
        private static void BindLogic(IKernel kernel)
        {
            kernel.Bind<INewsLogic>().To<NewsLogic>().InRequestScope();
            kernel.Bind<IAccountLogic>().To<AccountLogic>().InRequestScope();
            kernel.Bind<IAuthenticationData>().To<AuthenticationData>().InRequestScope();
            kernel.Bind<IAuthentication>().To<Authentication>().InRequestScope();
        }

        /// <summary>
        /// Биндинг сервисов аутентификации
        /// </summary>
        /// <param name="kernel"></param>
        private static void BindSecurityServices(IKernel kernel)
        {
            kernel.Bind<IAuthenticationService>().To<AuthenticationService>().InRequestScope();
            kernel.Bind<IOAuthInintializer>().To<OAuthInintializer>().InRequestScope();
            kernel.Bind<IRoleProvider>().To<RoleProvider>().InRequestScope();
            kernel.Bind<IUserRegistrationService>().To<UserRegistrationService>().InRequestScope();
            kernel.Bind<IOAuthService>().To<OAuthService>().InRequestScope();
        }

        /// <summary>
        /// Биндинг DAL
        /// </summary>
        /// <param name="kernel"></param>
        private static void BindDataAccessLayer(IKernel kernel)
        {
            kernel.Bind<ContextProvider>()
                .ToSelf()
                .InRequestScope()
                .WithConstructorArgument("connectionStringName", "DefaultConnection");
            kernel.Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>().InRequestScope();
            kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            kernel.Bind<IRoleRepository>().To<RoleRepository>().InRequestScope();
            kernel.Bind<IAccountRepository>().To<AccountRepository>().InRequestScope();
            kernel.Bind<INewsRepository>().To<NewsRepository>().InRequestScope();
            kernel.Bind<INewsTextRepository>().To<NewsTextRepository>().InRequestScope();
            kernel.Bind<IMembershipRepository>().To<MembershipRepository>().InRequestScope();
            kernel.Bind<IOAuthMembershipRepository>().To<OAuthMembershipRepository>().InRequestScope();
        }

        /// <summary>
        /// Биндинг логгера
        /// </summary>
        /// <param name="kernel"></param>
        private static void BindLogger(IKernel kernel)
        {
            kernel.Bind<ILogger>().To<Logger.Logger>().InRequestScope();
        } 
    }
}