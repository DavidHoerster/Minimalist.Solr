using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Minimalist.Commands.Baseball
{
    public class CreatePlayer : CommandBase
    {
        public Int32 LahmanId { get; set; }
        public String PlayerId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String NickName { get; set; }
        public String College { get; set; }
        public String Bats { get; set; }
        public String Throws { get; set; }
        public Int32 Height { get; set; }
        public Int32 Weight { get; set; }
        public DateTime? Debut { get; set; }
        public DateTime? FinalGame { get; set; }
    }
}
