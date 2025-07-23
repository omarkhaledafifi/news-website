using AutoMapper;
using NewsWebsite.BBL.DTOs;
using NewsWebsite.BBL.DTOs.UserRequests;
using NewsWebsite.DAL.Etities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Services.MappingProfiles
{
    internal class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleResponse>();
            CreateMap<ArticleRequest, Article>().ReverseMap();

        }
    }
}
