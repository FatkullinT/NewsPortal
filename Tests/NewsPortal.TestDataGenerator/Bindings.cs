using NewsPortal.Dal.Context;
using NewsPortal.Dal.Repository;
using NewsPortal.Dal.UnitOfWork;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Logic;
using NewsPortal.Domain.Security;
using NewsPortal.Logic;
using NewsPortal.Security;
using Ninject.Modules;

namespace NewsPortal.TestDataGenerator
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<INewsLogic>().To<NewsLogic>().InThreadScope();
            Bind<IAccountLogic>().To<AccountLogic>().InThreadScope();
            Bind<IAuthenticationData>().To<AuthenticationData>().InThreadScope();
            Bind<IAuthentication>().To<Authentication>().InThreadScope();
            Bind<IAuthenticationService>().To<AuthenticationService>().InThreadScope();
            Bind<IOAuthInintializer>().To<OAuthInintializer>().InThreadScope();
            Bind<IRoleProvider>().To<RoleProvider>().InThreadScope();
            Bind<IUserRegistrationService>().To<UserRegistrationService>().InThreadScope();
            Bind<IOAuthService>().To<OAuthService>().InThreadScope();
            Bind<ContextProvider>()
                .ToSelf()
                .InThreadScope()
                .WithConstructorArgument("connectionStringName", "DefaultConnection");
            Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>().InThreadScope();
            Bind<IUserRepository>().To<UserRepository>().InThreadScope();
            Bind<IRoleRepository>().To<RoleRepository>().InThreadScope();
            Bind<IAccountRepository>().To<AccountRepository>().InThreadScope();
            Bind<INewsRepository>().To<NewsRepository>().InThreadScope();
            Bind<INewsTextRepository>().To<NewsTextRepository>().InThreadScope();
            Bind<IMembershipRepository>().To<MembershipRepository>().InThreadScope();
            Bind<IOAuthMembershipRepository>().To<OAuthMembershipRepository>().InThreadScope();
        }
    }
}
