using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace Agile.Minimalist.Modules
{
    public class CardModule : NancyModule
    {
        public CardModule()
        {
            Get["/card/{id}"] = _ =>
            {
                return "You want card id " + _.id;
            };
        }
    }
}
