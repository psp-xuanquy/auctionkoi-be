using System.Data.Common;

namespace AuctionKOI.Application.FunctionalTests;
public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetConnection();

    Task ResetAsync();

    Task DisposeAsync();
}
