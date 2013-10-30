using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Minimalist.Model
{
    public abstract class BaseQuote
    {
        public String UserName { get; set; }
        public String FullName { get; set; }
        public Boolean IsLoggedIn { get { return !String.IsNullOrWhiteSpace(UserName); } }
    }
}
