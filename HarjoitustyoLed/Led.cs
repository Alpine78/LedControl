using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarjoitustyoLed
{
    class Led
    {
        public int pinId { get; set; }
        public int status { get; set; }
        public string nimi { get; set; }

        public Led(int pinId, string nimi)
        {
            this.pinId = pinId;
            this.status = 0;
            this.nimi = nimi;
        }
    }
}
