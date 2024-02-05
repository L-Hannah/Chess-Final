using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WindowsFormsApp1
{
    public partial class Chess2 : Form
    {
        string username;
        string colour;
        string currentlyselected=""; //Holds the currently selected piece.
        string turn = "white";
        Dictionaries dictionaries;
        public Chess2(string uname)
        {
            username = uname;
            InitializeComponent();
        }

        private void Chess2_Load(object sender, EventArgs e)
        {
            colour = "white";
            PlayGame();
        }
        private void PlayGame()
        {
            Controls.Clear();
            dictionaries = new Dictionaries(); //Initialize new dictionary
            Text = $"Chess Project - Signed in as {username}"; //Change title of window
            Size = new Size(816, 838); //Set size
            MaximumSize = new Size(816, 838); //Don't allow resizing
            MinimumSize = new Size(816, 838); //Don't allow resizing
            BackgroundImage = Resources.board; //Set background image to board
            InitializeDictionary();
            CreateButtons(); //Create the buttons
        }
        private void InitializeDictionary()
        {
            if (colour == "white")
            {
                //Set black pieces at the top
                dictionaries.SetBoard(0, 0, "BR");
                dictionaries.SetBoard(1, 0, "BH");
                dictionaries.SetBoard(2, 0, "BB");
                dictionaries.SetBoard(3, 0, "BQ");
                dictionaries.SetBoard(4, 0, "BK");
                dictionaries.SetBoard(5, 0, "BB");
                dictionaries.SetBoard(6, 0, "BH");
                dictionaries.SetBoard(7, 0, "BR");
                for (int i = 0; i < 8; i++)
                {
                    dictionaries.SetBoard(i, 1, "BP");
                }
                //Set white pieces at bottom
                dictionaries.SetBoard(0, 7, "WR");
                dictionaries.SetBoard(1, 7, "WH");
                dictionaries.SetBoard(2, 7, "WB");
                dictionaries.SetBoard(3, 7, "WQ");
                dictionaries.SetBoard(4, 7, "WK");
                dictionaries.SetBoard(5, 7, "WB");
                dictionaries.SetBoard(6, 7, "WH");
                dictionaries.SetBoard(7, 7, "WR");
                for (int i = 0; i < 8; i++)
                {
                    dictionaries.SetBoard(i, 6, "WP");
                }
            }
            else
            {
                //Do the code to set layout for black
                //Set the black pieces at the top
                dictionaries.SetBoard(0, 0, "WR");
                dictionaries.SetBoard(1, 0, "WH");
                dictionaries.SetBoard(2, 0, "WB");
                dictionaries.SetBoard(3, 0, "WK");
                dictionaries.SetBoard(4, 0, "WQ");
                dictionaries.SetBoard(5, 0, "WB");
                dictionaries.SetBoard(6, 0, "WH");
                dictionaries.SetBoard(7, 0, "WR");
                for (int i = 0; i < 8; i++)
                {
                    dictionaries.SetBoard(i, 1, "WP");
                }
                //Set white pieces at bottom
                dictionaries.SetBoard(0, 7, "BR");
                dictionaries.SetBoard(1, 7, "BH");
                dictionaries.SetBoard(2, 7, "BB");
                dictionaries.SetBoard(3, 7, "BK");
                dictionaries.SetBoard(4, 7, "BQ");
                dictionaries.SetBoard(5, 7, "BB");
                dictionaries.SetBoard(6, 7, "BH");
                dictionaries.SetBoard(7, 7, "BR");
                for (int i = 0; i < 8; i++)
                {
                    dictionaries.SetBoard(i, 6, "BP");
                }
            }

            }
        private void CreateButtons()
        {
            for (int i = 0; i <8;i++)
            {
                for (int j = 0; j < 8;j++)
                {
                    Button TempButton = new Button() //Create temporary button
                    {
                        Text = "", //Ensure text is empty
                        Name = i.ToString() + j.ToString(), //Set name to coordinate string
                        Size = new Size(100, 100), //Set size
                        Visible = true, //Make button visible
                        Location = new Point(i*100,j*100), //Set position dynamically 
                        FlatStyle= FlatStyle.Flat, //Style of button
                        BackColor= Color.Transparent, //Set background to transparent
                    };
                    TempButton.FlatAppearance.MouseOverBackColor = Color.Transparent; //No colour when hovering
                    TempButton.FlatAppearance.MouseDownBackColor = Color.Aqua; //Aqua when clicked
                    SetButtonImage(TempButton, i.ToString() + j.ToString());
                    TempButton.Click += new EventHandler(SquareButton_Click); //Add event handler
                    Controls.Add(TempButton); //Add button to form controls
                    TempButton.BringToFront(); //Bring button forward
                }
            }
        }
        private void SquareButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender; //Turn sender into button object
            string coordstring = btn.Name;//Get coordstring using the name of the button
            int i = (int)char.GetNumericValue(coordstring[0]); //Get the horizontal value using first char
            int j = (int)char.GetNumericValue(coordstring[1]); //Get the vertical value using second char
            string piece = dictionaries.GetBoard(i, j);
            string piececolour = dictionaries.GetPieceColour(piece); //Get colour
            //MessageBox.Show($"You selected the piece at ({i},{j}). This piece is a {dictionaries.GetBoard(i, j)}");
            if (currentlyselected == "") //No piece selected
            {
                if (piece==""||piececolour!=turn) { return; } //No piece at place or not possible place, nothing to select
                currentlyselected = coordstring; //Set currently selected to the coordinate
                btn.BackColor = Color.ForestGreen; //Set back colour to show the selected piece
                btn.FlatAppearance.MouseOverBackColor = Color.ForestGreen; //Set the mouse over back colour
            } else if (piece != "") //Piece already selected, piece at new coord isnt blank
            {
                //Attempt to take the piece at this coordinate.
                if (piececolour==dictionaries.GetPieceColour(dictionaries.GetPieceWithCoordString(currentlyselected)))
                {
                    //If colour of new piece is same as colour of currently selected, new piece is now selected
                    currentlyselected = coordstring;
                    btn.BackColor= Color.ForestGreen;
                    btn.FlatAppearance.MouseOverBackColor= Color.ForestGreen;
                } else
                {
                    //Now attempt to take piece
                    bool moveValid = Viable(coordstring, currentlyselected,false,"");
                    if (moveValid)
                    {
                        Move(i,j);
                        if (turn == "white") { turn = "black"; } else { turn = "white"; }
                    } else
                    {
                        //Move cannot be done, set the button to transparent and unselect it.
                        Button currentButton = (Button)Controls.Find(currentlyselected, true)[0];
                        currentButton.BackColor = Color.Transparent;
                        currentButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                        currentlyselected = "";
                    }
                }
            } else if (piece=="") //No piece at new coord
            {
                //Attempt to move to the blank spot in the new coord
                bool moveValid = Viable(coordstring, currentlyselected,false,"");
                if (moveValid)
                {
                    Move(i,j);
                    if (turn == "white") { turn = "black"; } else { turn = "white"; }
                } else
                {
                    //Move cannot be done, set the button to transparent and unselect it.
                    Button currentButton = (Button)Controls.Find(currentlyselected, true)[0];
                    currentButton.BackColor = Color.Transparent;
                    currentButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    currentlyselected = "";
                }
            }
            ClearUnselected();
        }
        private void SetImages()
        {
            for (int i=0;i<8;i++)
            {
                for (int j=0;j<8;j++)
                {
                    Button btn = (Button)Controls.Find(i.ToString() + j.ToString(), true)[0]; //Find button from controls object
                    SetButtonImage(btn, i.ToString() + j.ToString()); //Set image of button
                }
            }
        }
        private void Move(int i, int j)
        {
            //Set piece at coord to currently selected piece
            dictionaries.SetBoard(i, j, dictionaries.GetPieceWithCoordString(currentlyselected));
            //Get current coords
            int currentlyi = (int)char.GetNumericValue(currentlyselected[0]);
            int currentlyj = (int)char.GetNumericValue(currentlyselected[1]);
            currentlyselected = ""; //Set currentlyselected to nothing
            dictionaries.SetBoard(currentlyi, currentlyj, ""); //Set old pos to nothing as piece has moved
            SetImages();
            ClearUnselected(); //Clear image backgrounds
        }
        private void ClearUnselected()
        {
            for (int x=0;x<8;x++)
            {
                for (int y=0;y<8;y++)
                {
                    if (currentlyselected!=x.ToString() + y.ToString())
                    {
                        Button btn = (Button)Controls.Find(x.ToString() + y.ToString(), true)[0]; //Find button from controls object
                        btn.BackColor = Color.Transparent;
                        btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    }
                }
            }
        }
        private void SetButtonImage(Button button, string coordstring)
        {
            string curpiece = dictionaries.GetPieceWithCoordString(coordstring);
            switch (curpiece) //Switch statement to look at value of piece
            {
                case "BR":
                    button.Image = Resources.BR;
                    break;
                case "BH":
                    button.Image = Resources.BH;
                    break;
                case "BB":
                    button.Image = Resources.BB;
                    break;
                case "BQ":
                    button.Image = Resources.BQ;
                    break;
                case "BK":
                    button.Image = Resources.BK;
                    break;
                case "BP":
                    button.Image = Resources.BP;
                    break;
                case "WR":
                    button.Image = Resources.WR;
                    break;
                case "WH":
                    button.Image = Resources.WH;
                    break;
                case "WB":
                    button.Image = Resources.WB;
                    break;
                case "WQ":
                    button.Image = Resources.WQ;
                    break;
                case "WK":
                    button.Image = Resources.WK;
                    break;
                case "WP":
                    button.Image = Resources.WP;
                    break;
                case "WKIC":
                    button.Image = Resources.WKIC;
                    break;
                case "BKIC":
                    button.Image = Resources.BKIC;
                    break;
                case "":
                    button.Image = Resources.blankimage;
                    break;
                default:
                    // Set a default image or do nothing if the piece abbreviation is not recognized
                    break;
            }

        }
        private bool Viable(string coordstring, string curselected, bool overRide, string newabbrev)
        {
            bool viable = false; //Set to false initially
            string pieceabbrev = dictionaries.GetPieceWithCoordString(coordstring);
            string piececolour = dictionaries.GetPieceColour(pieceabbrev);
            string curabbrev = dictionaries.GetPieceWithCoordString(curselected);
            if (overRide) { curabbrev = newabbrev; }
            string curcolour = dictionaries.GetPieceColour(curabbrev);
            int i = (int)char.GetNumericValue(coordstring[0]); //Get horizontal of piece to try take
            int j = (int)char.GetNumericValue(coordstring[1]); //Get vertical of piece to try take
            int curi = (int)char.GetNumericValue(curselected[0]); //Get horizontal of current
            int curj = (int)char.GetNumericValue(curselected[1]); //Get vertical of current
            (int i, int j) attemptedmove = (i - curi, j - curj); //Make tuple of the attempted move
            if (curabbrev=="WH"||curabbrev=="BH")
            {
                int idiff = Math.Abs(i - curi);
                int jdiff = Math.Abs(j - curj);
                if (idiff == 2 && jdiff == 1 || idiff == 1 && jdiff == 2) { return true; }
                else { return false; }
            }
            else if (curabbrev=="WP"||curabbrev=="BP")
            {
                int idiff = i - curi; //Get horizontal difference
                int jdiff = j - curj; //Get vertical difference
                if (pieceabbrev=="") //Not attempting to take a piece
                {
                    if (idiff!=0) { return false; } //Can't move horizontally when not trying to take a piece.
                    if (curcolour!=colour)
                    {
                        if (jdiff==1) //Has to move down
                        {
                            return true;
                        }
                        if (jdiff==2 && curj==1) //Trying starting move
                        {
                            if (dictionaries.GetBoard(curi,curj+1)==""&&dictionaries.GetBoard(curi,curj+2)=="")
                            {
                                return true; //Move is valid as no pieces in the way.
                            }
                        }
                    } else
                    {
                        if (jdiff==-1) //Has to move up
                        {
                            return true;
                        }
                        if (jdiff == -2 && curj == 6) //Trying starting move
                        {
                            if (dictionaries.GetBoard(curi, curj-1) == "" && dictionaries.GetBoard(curi, curj-2) == "")
                            {
                                return true; //Move is valid as no pieces in the way.
                            }
                        }
                    }
                } 
                else
                {
                    //Attempting to take a piece
                    if (Math.Abs(idiff)!=1) {return false;} //Can't take at a diff other than 1
                    if (curcolour!=colour)//Not same colour as user
                    {
                        if (jdiff==1) //Moving down as opposing side
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (jdiff==-1) //Moving up as this is the user's side
                        {
                            return true;
                        }
                    }
                }
            }
            else if (curabbrev=="BR"||curabbrev=="WR")
            {
                List<(int, int)> possibleMoves = new List<(int, int)>(); //New list for possible moves
                //Each max will add 1, as the for loop in C# isn't inclusive and must iterate enough times.
                int upwardsMax = Math.Abs(0 - curj) + 1; //Get upwards max from bound
                int downwardsMax = Math.Abs(7 - curj) + 1; //Get downwards max from bound
                int leftMax = Math.Abs(0 - curi) + 1; //Get left max from bound
                int rightMax = Math.Abs(7 - curi) + 1; //Get right max from bound
                for (int x = 1; x < upwardsMax; x++)
                {
                    //Get piece colour as variable
                    string tempPieceColour = dictionaries.GetPieceColour(dictionaries.GetBoard(curi, curj - x));
                    if (tempPieceColour!=curcolour)//Either blank or enemy
                    {
                        possibleMoves.Add((0, -x)); //Add as possible move
                        if (tempPieceColour != "") { break; } //If Enemy, this is last possible move
                    } else
                    {
                        //Can't collide with own colour
                        break;
                    }
                }
                for (int x = 1; x<downwardsMax; x++)
                {
                    //Get piece colour as variable
                    string tempPieceColour = dictionaries.GetPieceColour(dictionaries.GetBoard(curi, curj + x));
                    if (tempPieceColour != curcolour)//Either blank or enemy
                    {
                        possibleMoves.Add((0, +x)); //Add as possible move
                        if (tempPieceColour != "") { break; } //If Enemy, this is last possible move
                    }
                    else
                    {
                        //Can't collide with own colour
                        break;
                    }
                }
                for (int x=1;x<leftMax; x++)
                {
                    //Get piece colour as variable
                    string tempPieceColour = dictionaries.GetPieceColour(dictionaries.GetBoard(curi-x,curj));
                    if (tempPieceColour!=curcolour)//Either blank or enemy
                    {
                        possibleMoves.Add((-x, 0)); //Add as possible move
                        if (tempPieceColour!="") { break; } //If enemy, this is last possible move
                    }
                    else
                    {
                        //Can't collide with own colour
                        break;
                    }
                }
                for (int x = 1; x < rightMax; x++)
                {
                    //Get piece colour as variable
                    string tempPieceColour = dictionaries.GetPieceColour(dictionaries.GetBoard(curi + x, curj));
                    if (tempPieceColour != curcolour)//Either blank or enemy
                    { 
                        possibleMoves.Add((x, 0)); //Add as possible move
                        if (tempPieceColour != "") { break; } //If enemy, this is last possible move
                    }
                    else
                    {
                        //Can't collide with own colour
                        break;
                    }
                }
                if (possibleMoves.Contains(attemptedmove))
                {
                    //Move is possible
                    return true;
                } 
                else 
                {
                    //Move is not possible
                    return false;
                }
            }
            else if (curabbrev=="BB"||curabbrev=="WB")
            {
                //Difference in both x and y axis must be equal
                if (Math.Abs(i-curi)!=Math.Abs(j-curj)) { return false; }
                List<(int, int)> possibleMoves = new List<(int, int)>(); //New list for possible moves
                //Each max will add 1, as the for loop in C# isn't inclusive and must iterate enough times.
                int upwardsMax = Math.Abs(0 - curj) + 1; //Get upwards max from bound
                int downwardsMax = Math.Abs(7 - curj) + 1; //Get downwards max from bound
                int leftMax = Math.Abs(0 - curi) + 1; //Get left max from bound
                int rightMax = Math.Abs(7 - curi) + 1; //Get right max from bound
                for (int x=1;x<leftMax; x++)
                {
                    //Left and down
                    if (x == downwardsMax) { break; } //Can't go outside of board
                    //Get piece colour as variable
                    string tempPieceColour = dictionaries.GetPieceColour(dictionaries.GetBoard(curi-x, curj+x));
                    if (tempPieceColour!=curcolour)
                    {
                        possibleMoves.Add((-x, x)); //Moves by same value in each direction but left so -x for horizontal
                        if (tempPieceColour!="") { break; }
                    } else
                    {
                        //Same colour, cannot collide with this
                        break;
                    }
                }
                for (int x = 1; x < leftMax; x++)
                {
                    //Left and up
                    if (x == upwardsMax) { break; } //Can't go outside of board
                    //Get piece colour as variable
                    string tempPieceColour = dictionaries.GetPieceColour(dictionaries.GetBoard(curi - x, curj - x));
                    if (tempPieceColour != curcolour)
                    {
                        possibleMoves.Add((-x, -x)); //Moves by same value in each direction but left so -x for horizontal
                        if (tempPieceColour != "") { break; }
                    }
                    else
                    {
                        //Same colour, cannot collide with this
                        break;
                    }
                }
                for (int x = 1; x < rightMax; x++)
                {
                    //Right and down
                    if (x == downwardsMax) { break; }//Can't go outside of board
                    //Get piece colour as variable
                    string tempPieceColour = dictionaries.GetPieceColour(dictionaries.GetBoard(curi+x, curj + x));
                    if (tempPieceColour != curcolour)
                    {
                        possibleMoves.Add((x, x)); //Moves by same value in each direction
                        if (tempPieceColour != "") { break; };
                    } 
                    else
                    {
                        //Same colour, cannot collide with this
                        break;
                    }
                }
                for (int x = 1; x < rightMax; x++)
                {
                    //Right and up
                    if (x == upwardsMax) { break; }//Can't go outside of board
                    //Get piece colour as variable
                    string tempPieceColour = dictionaries.GetPieceColour(dictionaries.GetBoard(curi + x, curj - x));
                    if (tempPieceColour != curcolour)
                    {
                        possibleMoves.Add((x, -x)); //Moves by same value in each direction
                        if (tempPieceColour != "") { break; };
                    }
                    else
                    {
                        //Same colour, cannot collide with this
                        break;
                    }
                }
                //If move possible, return true. If not, return false
                if (possibleMoves.Contains(attemptedmove)) { return true; } else { return false; }
            }
            else if (curabbrev=="WQ"||curabbrev=="BQ")
            {
                string tempColour = dictionaries.GetPieceColour(curabbrev);
                if (tempColour=="white")
                {
                    if (Viable(coordstring, curselected, true,"WB")||Viable(coordstring,curselected,true,"WR"))
                    {
                        //Move is valid for either white bishop or white rook
                        return true;
                    }
                } else
                {
                    if (Viable(coordstring,curselected,true,"BB")||Viable(coordstring,curselected,true,"BR"))
                    {
                        //Move is valid for either black bishop or black rook
                        return true;
                    }
                }
            }
            return viable;
        }
        private void Chess2_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Check if the user is closing the form
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Exit the application
                Application.Exit();
            }
        }
    }
}
