using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarjoitustyoLed
{
    public class PlayList
    {
        public IList<LedPlay> LedPlays { get; set; }
    }

    public class LedPlay
    {
        public int Time { get; set; }
        public int PinId1 { get; set; }
        public int Status1 { get; set; }
        public int PinId2 { get; set; }
        public int Status2 { get; set; }

    }

}
