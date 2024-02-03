using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class Dictionaries
    {
        private Dictionary<string, string> piecenames; //Dictionary for each piecename to use for reference
        private Dictionary<(int,int), string> board; //Dictionary for board
        public Dictionaries()
        {
            piecenames = new Dictionary<string, string>() //Create dictionary for abbreviations
            {
                { "BR", "blackrook" },
                { "BH", "blackknight" },
                { "BB", "blackbishop" },
                { "BQ", "blackqueen" },
                { "BK", "blackking" },
                { "BP", "blackpawn" },
                { "WR", "whiterook" },
                { "WH", "whiteknight" },
                { "WB", "whitebishop" },
                { "WQ", "whitequeen" },
                { "WK", "whiteking" },
                { "WP", "whitepawn" },
                { "", ""}
            };
            board = new Dictionary<(int, int), string>(); //Create empty dictionary for board
            for (int i=0;i<8;i++) //Nested for loop
            {
                for (int j=0;j<8;j++)
                {
                    board.Add((i, j), ""); //Fill dictionary coords with blank
                }
            }
        }
        public Dictionary<string, string> Piecenames
        {
            get { return piecenames; }
            set { piecenames = value; }
        }
        public bool SetBoard(int x, int y, string value)
        {
            if (y<0 || y>7||x<0||x>7)
            {
                throw new ArgumentOutOfRangeException("Parameter index is out of range.");
            }
            string[] possiblevalues = new string[] { "BR", "BH", "BB", "BQ", "BK", "BP", "WR", "WH", "WB", "WQ", "WK", "WK", "WP", "","WKIC","BKIC" }; 
            //Possible abbreviations
            if (!possiblevalues.Contains(value)) return false; //Piece not possible
            return true;
        }
    }
}
