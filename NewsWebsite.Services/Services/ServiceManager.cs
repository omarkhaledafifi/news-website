using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NewsWebsite.DAL.Contracts;
using NewsWebsite.DAL.Etities.Identity;
using NewsWebsite.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Services.Services
{
    public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper,
                                UserManager<ApplicationUser> userManager,
                                IConfiguration configuration,
                                IOptions<JwtSettings> options) : IServiceManager
    {
        private readonly Lazy<ArticleService> _lazyArticleService = new Lazy<ArticleService>(()
                                                                        => new ArticleService(unitOfWork, mapper));
        private readonly Lazy<AuthenticationService> _lazyAuthenticationService =new Lazy<AuthenticationService>(() 
                                                    => new AuthenticationService(userManager, configuration, options));
        public IArticleService ArticleService => _lazyArticleService.Value;
        public IAuthenticationService AuthenticationService => _lazyAuthenticationService.Value;
    }
}