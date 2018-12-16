using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarjoitustyoLed
{
    public class PlayList
    {
        public List<PlayListRow> PlayListRows;

        public PlayList()
        {
            PlayListRows = new List<PlayListRow>();
        }

        public void addRow(PlayListRow playListRow)
        {
            PlayListRows.Add(playListRow);
        }
    }

    public class PlayListRow
    {
        public int Time { get; set; }
        public int PinId1 { get; set; }
        public int Status1 { get; set; }
        public int PinId2 { get; set; }
        public int Status2 { get; set; }


        public PlayListRow()
        {
        }

        public PlayListRow(int time, int pinId1, int status1, int pinId2, int status2)
        {
            Time = time;
            PinId1 = pinId1;
            Status1 = status1;
            PinId2 = pinId2;
            Status2 = status2;
        }


    }

}
