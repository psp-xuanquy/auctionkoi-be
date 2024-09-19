using AuctionKOI.Domain.Entities;

namespace AuctionKOI.Application.Common.Models;
public class LookupDto
{
    public required string Id { get; init; }

    public string? Title { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TodoList, LookupDto>();
            CreateMap<TodoItem, LookupDto>();
        }
    }
}
