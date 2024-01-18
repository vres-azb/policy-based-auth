namespace PolicyBased.Infra.Persistence.Context
{
    public interface IDataContextFactory
    {
        PolicyTestDBContext CreateContext();
    }
}
