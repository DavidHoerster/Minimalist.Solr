using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Minimalist.Eventing.Baseball;

namespace Agile.Minimalist.Eventing.Handler.Baseball
{
    public class PlayerHandler : IHandle<PlayerCreated>,
                                 IHandle<PlayerYearAdded>
    {
        public void Handle(PlayerCreated theEvent)
        {
            //do something
        }

        public void Handle(PlayerYearAdded theEvent)
        {
            //do something else
        }
    }
}
