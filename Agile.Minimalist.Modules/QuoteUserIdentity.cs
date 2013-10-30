using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Security;

namespace Agile.Minimalist.Repository
{
    public class QuoteUserIdentity : IUserIdentity
    {
        public IEnumerable<string> Claims { get; set; }
        public string UserName { get; set; }
    }
}
