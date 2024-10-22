using System;
using MediatR;

namespace Application.Features.Koi.Commands.Delete
{
    public class DeleteKoiCommand : IRequest<string>
    {
        public string Id { get; set; }

        public DeleteKoiCommand(string id)
        {
            Id = id;
        }
    }
}
