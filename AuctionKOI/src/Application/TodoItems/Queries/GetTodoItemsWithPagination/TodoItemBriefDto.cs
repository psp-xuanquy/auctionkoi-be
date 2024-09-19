using AuctionKOI.Domain.Entities;

namespace AuctionKOI.Application.TodoItems.Queries.GetTodoItemsWithPagination;
public class TodoItemBriefDto
{
    public required string Id { get; init; }

    public required string ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TodoItem, TodoItemBriefDto>();
        }
    }
}
