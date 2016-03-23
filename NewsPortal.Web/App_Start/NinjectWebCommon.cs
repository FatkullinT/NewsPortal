using System.Reflection;
using System.Web.Mvc;
using NewsPortal.Dal.Context;
using NewsPortal.Dal.Repository;
using NewsPortal.Dal.UnitOfWork;

using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Logic;
using NewsPortal.Domain.Security;
using NewsPortal.Logic;
using NewsPortal.Security;
using NewsPortal.Web.Classes;
using Ninject.Web.Mvc.FilterBindingSyntax;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NewsPortal.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NewsPortal.App_Start.NinjectWebCommon), "Stop")]

namespace NewsPortal.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
            bootstrapper.Kernel.Get<IOAuthInintializer>().Initialize();
            SetDefaultUsersAndRoles();
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void SetDefaultUsersAndRoles()
        {
            IUserRegistrationService registrationService = bootstrapper.Kernel.Get<IUserRegistrationService>();
            IRoleProvider roleProvider = bootstrapper.Kernel.Get<IRoleProvider>();
            Guid userId = registrationService.Register("Timur", "Admin", false);
            roleProvider.CreateRole("Administrator", false);
            roleProvider.AddUserToRole(userId, "Administrator");
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.Bind<ContextProvider>()
                .To<ContextProvider>()
                .InRequestScope()
                .WithConstructorArgument("connectionStringName", "DefaultConnection");
            kernel.Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>().InRequestScope();
            kernel.Bind<IAuthenticationService>().To<AuthenticationService>().InRequestScope();
            kernel.Bind<IOAuthInintializer>()
                .To<OAuthInintializer>()
                .InRequestScope();
            kernel.Bind<IUserRegistrationService>().To<UserRegistrationService>().InRequestScope();
            kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            kernel.Bind<IRoleRepository>().To<RoleRepository>().InRequestScope();
            kernel.Bind<IUserDataRepository>().To<UserDataRepository>().InRequestScope();
            kernel.Bind<INewsRepository>().To<NewsRepository>().InRequestScope();
            kernel.Bind<INewsTextRepository>().To<NewsTextRepository>().InRequestScope();
            kernel.Bind<IMembershipRepository>().To<MembershipRepository>().InRequestScope();
            kernel.Bind<IOAuthMembershipRepository>().To<OAuthMembershipRepository>().InRequestScope();
            kernel.Bind<IRoleProvider>().To<RoleProvider>().InRequestScope();
            kernel.Bind<IOAuthService>().To<OAuthService>().InRequestScope();
            kernel.Bind<INewsLogic>().To<NewsLogic>().InRequestScope();
            kernel.Bind<IAccountLogic>().To<AccountLogic>().InRequestScope();
            kernel.BindFilter<AdministratorOnlyFilter>(FilterScope.Action, 0).WhenActionMethodHas<AdministratorOnlyAttribute>();
        }        
    }
}
