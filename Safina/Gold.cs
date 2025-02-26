using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Safina.Program;
using System.Xml.Linq;

namespace Safina
{
    public class Gold : Item
    {
        public int Amount { get; set; }
        public Gold(int amoint) 
        {
            Name = "Золото";
            Amount = amoint;
        }
    }
}
