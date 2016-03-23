namespace NewsPortal.Dal.Context
{
    public class ContextProvider
    {
        private string _connectionStringName;


        public Context Context
        {
            get; 
            private set; 
        }

        public ContextProvider(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public void CreateContext()
        {
            DisposeContext();
            Context = new Context(_connectionStringName);
        }

        public void DisposeContext()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }
        }
    }
}
