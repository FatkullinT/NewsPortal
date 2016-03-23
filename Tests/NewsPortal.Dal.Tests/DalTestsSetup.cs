using NewsPortal.Dal.Context;
using NewsPortal.Dal.UnitOfWork;
using NewsPortal.Domain.Dal.UnitOfWork;
using NUnit.Framework;

namespace NewsPortal.Dal.Tests
{
    [SetUpFixture]
    public class DalTestsSetup
    {
        private static ContextProvider _contextProvider;

        private static IUnitOfWorkFactory _unitOfWorkFactory;

        internal static ContextProvider ContextProvider
        {
            get
            {
                return _contextProvider ?? (_contextProvider = new ContextProvider("TestConnection"));
            }
        }

        internal static IUnitOfWorkFactory UnitOfWorkFactory
        {
            get
            {
                return _unitOfWorkFactory ?? (_unitOfWorkFactory = new UnitOfWorkFactory(ContextProvider));
            }
        }

        [OneTimeSetUp]
        protected void BeforeTestExecuting()
        {
            using (UnitOfWorkFactory.Create())
            {
                ContextProvider.Context.Database.Delete();
                ContextProvider.Context.Database.Create();
            }
        }

        [OneTimeTearDown]
        protected void AfterTestExecuting()
        {
            using (UnitOfWorkFactory.Create())
            {
                ContextProvider.Context.Database.Delete();
            }
        }
    }
}