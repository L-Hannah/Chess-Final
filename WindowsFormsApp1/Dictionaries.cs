using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class Dictionaries
    {
        private Dictionary<(int, int), string> board; //Dictionary for board
        public Dictionaries()
        {
            board = new Dictionary<(int, int), string>(); //Create empty dictionary for board
            for (int i = 0; i < 8; i++) //Nested for loop
            {
                for (int j = 0; j < 8; j++)
                {
                    board.Add((i, j), ""); //Fill dictionary coords with blank
                }
            }
        }
        public bool SetBoard(int x, int y, string value)
        {
            if (y < 0 || y > 7 || x < 0 || x > 7)
            {
                throw new ArgumentOutOfRangeException("Parameter index is out of range.");
            }
            string[] possiblevalues = new string[] { "BR", "BH", "BB", "BQ", "BK", "BP", "WR", "WH", "WB", "WQ", "WK", "WK", "WP", "", "WKIC", "BKIC" };
            //Possible abbreviations
            if (!possiblevalues.Contains(value)) return false; //Piece not possible
            board[(x, y)] = value; //Set value
            return true;
        }
        public string GetPieceColour(string abbreviation)
        {
            if (abbreviation == "")//If piece is empty
            {
                return "";
            }
            string colour = abbreviation.Substring(0, 1).ToUpper(); //Get first character
            if (colour == "W") //Colour is white
            {
                return "white";
            }
            else //Must be black as empty string handled already.
            {
                return "black";
            }
        }
        public string GetPieceWithCoordString(string coordstring)
        {
            if (coordstring=="")
            {
                MessageBox.Show("Somehow coordstring is now empty??");
            }
            int i = (int)char.GetNumericValue(coordstring[0]); //Get numeric value of first character and set as i
            int j = (int)char.GetNumericValue(coordstring[1]); //Get numeric value of second character and set as i
            return GetBoard(i, j); //Return piece abbrevation from the coordinate
        }
        public string GetBoard(int x, int y)
        {
            if (y < 0 || y > 7 || x < 0 || x > 7) //Test coords
            {
                throw new ArgumentOutOfRangeException("Parameter index is out of range.");
            };
            return board[(x, y)]; //if coords valid, return piece at these coords
        }
        public string FindIndex(string pieceabbrev)
        {
            for (int i = 0; i < 8; i++) //Nested loop
            {
                for (int j = 0; j < 8; j++)
                {
                    //If piece found, return coordinates as string.
                    if (GetBoard(i, j) == pieceabbrev) { return i.ToString() + j.ToString(); }
                }
            }
            return ""; //Not found, return empty value.
        }
        public Dictionary<(int, int), string> Board
        {
            get { return  board; }
            set { board = value; }
        }
    }
}
