using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1y
{
    internal class EnPassantData
    {
        //Store the coordinates of the empty square
        public (int, int) Empty { get; set; }
        //Store the coordinates to actually take
        public (int,int) Take { get; set; }
        public int MoveCount { get; set; }
        public EnPassantData((int,int)empty, (int,int)take)
        {
            //Set class variables to params
            this.Empty = empty;
            this.Take = take;
            this.MoveCount = 0;
        }
    }
}
