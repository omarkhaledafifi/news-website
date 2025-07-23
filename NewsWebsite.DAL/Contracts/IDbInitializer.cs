using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.DAL.Contracts
{
    public interface IDbInitializer
    {
        public Task InitailizeAsync();
        public Task InitailizeIdentityAsync();
    }
}
