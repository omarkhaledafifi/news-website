using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Services.Abstraction
{
    public interface IServiceManager
    {
        public IArticleService ArticleService { get; }
        public IAuthenticationService AuthenticationService { get; }
    }
}
