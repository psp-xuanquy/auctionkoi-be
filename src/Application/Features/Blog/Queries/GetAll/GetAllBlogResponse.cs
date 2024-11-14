using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Enums;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;

namespace Application.Features.Blog.Queries.GetAll;
public class GetAllBlogResponse : IMapFrom<BlogEntity>
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? UrlImage { get; set; }
    public DateTime? PostedDate { get; set; }
    public string? AuthorName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<BlogEntity, GetAllBlogResponse>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName));
            
    }
}
