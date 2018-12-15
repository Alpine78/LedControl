using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarjoitustyoLed
{
    class Details
    {
        public int sequenceId { get; set; }
        public int timeRowId { get; set; }
        public int time { get; set; }
        public int ledRowId { get; set; }
        public string ledName1 { get; set; }
        public int pinId1 { get; set; }
        public int status1 { get; set; }
        public string ledName2 { get; set; }
        public int pinId2 { get; set; }
        public int status2 { get; set; }
    }
}
