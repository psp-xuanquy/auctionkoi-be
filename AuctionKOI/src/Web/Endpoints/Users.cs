using AuctionKOI.Domain.Entities;
using AuctionKOI.Infrastructure.Identity;

namespace AuctionKOI.Web.Endpoints;
public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapIdentityApi<ApplicationUser>();
    }
}
