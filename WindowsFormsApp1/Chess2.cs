using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;
using WindowsFormsApp1y;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WindowsFormsApp1
{
    public partial class Chess2 : Form
    {
        string username;
        string colour;
        string currentlyselected=""; //Holds the currently selected piece.
        string turn = "white"; //Holds current turn, always starts with white
        bool WKIC = false; //White king in check, starts as false
        bool BKIC = false; //Black king in check, starts as false
        int fullmoves = 0;
        int halfmoves = 0;
        int movecounter = 0;
        Dictionaries dictionaries;
        HypotheticalChess hypotheticalChess;
        //These two will be for whichever colour is at the top
        bool zeroZeroRookMoved = false;
        bool sevenZeroRookMoved = false;
        //These two will be for whichever colour is at the bottom
        bool zeroSevenRookMoved = false;
        bool sevenSevenRookMoved = false;
        //These two will be for the kings
        bool whiteKingMoved = false;
        bool blackKingMoved = false;
        //For castling logic again
        bool castling = false;
        string rookOldCoords;
        string rookNewCoords;
        //Create a list of objects that hold the data for en passant
        List<EnPassantData> EnPassantList = new List<EnPassantData>();
        bool EnPassanting = false;
        (int, int) EnPassantRemove;
        bool GameOver = false;
        string Winner = "";
        bool Promoting = false;
        public Chess2(string uname)
        {
            username = uname;
            InitializeComponent();
        }

        private void Chess2_Load(object sender, EventArgs e)
        {
            //Make new menu for game options
            Menu = new MainMenu();
            MenuItem item = new MenuItem("Game options");
            Menu.MenuItems.Add(item);
            //Add options to this menu
            item.MenuItems.Add("Resign", new EventHandler(MenuEvent));
            item.MenuItems.Add("Offer draw", new EventHandler(MenuEvent));
            //Call playgame function
            PlayGame("white");
        }
        private void MenuEvent(object sender, EventArgs e)
        {
            //Don't allow actions when game over
            if (GameOver || Promoting) return;
            string tempOption = "";
            if (sender is MenuItem menuItem)
            {
                tempOption = menuItem.Text;
            }
            DialogResult dialogResult;
            switch (tempOption)
            {
                case "Resign":
                    dialogResult = MessageBox.Show("Would you like to resign?","Resign",MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //Do code to resign
                        GameOver = true;
                        if (turn=="white") { Winner = "black"; } else { Winner = "white"; }
                        GameDone();
                    }
                    else
                    {
                        //Do nothing
                        break;
                    }
                    break;
                case "Offer draw":
                    dialogResult = MessageBox.Show($"{turn} has offered a draw. Do you accept?","Draw offer",MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //Do code to cause a draw
                        GameOver = true;
                        Winner = "";
                        GameDone();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //Do nothing I guess
                        break;
                    }
                    break;
                default:
                    break;
            }
        }
        private void GameDone()
        {
            DialogResult dialogResult;
            string message;
            if (Winner=="") { message = "Game drawn. Play new?"; } 
            else
            {
                message = $"{Winner} has won the game, play again?";
            }
            dialogResult = MessageBox.Show(message, "Game over",MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                PlayGame("white");
            }
        }
        private void PlayGame(string colour)
        {
            ClearVariables();
            Controls.Clear();
            dictionaries = new Dictionaries(); //Initialize new dictionary
            this.colour = colour;
            hypotheticalChess = new HypotheticalChess(this.colour, BKIC, WKIC);
            Text = $"Chess Project - Signed in as {username}"; //Change title of window
            Size = new Size(816, 858); //Set size
            MaximumSize = new Size(816, 858); //Don't allow resizing
            MinimumSize = new Size(816, 858); //Don't allow resizing
            BackgroundImage = Resources.board; //Set background image to board
            InitializeDictionary();
            CreateButtons(); //Create the buttons
        }
        private void PromoteGUI()
        {
            Form settingsForm = new Form();
            settingsForm.Show();
        }
        private void ClearVariables()
        {
            //Set all variables to their default functions from their initiation
            currentlyselected = "";
            turn = "white";
            WKIC = false;
            BKIC = false;
            fullmoves = 0;
            halfmoves = 0;
            movecounter = 0;
            zeroZeroRookMoved = false;
            sevenZeroRookMoved = false;
            zeroSevenRookMoved = false;
            sevenSevenRookMoved = false;
            whiteKingMoved = false;
            blackKingMoved = false;
            castling = false;
            rookOldCoords="";
            rookNewCoords="";
            EnPassantList = new List<EnPassantData>();
            EnPassanting = false;
            EnPassantRemove = (-1,-1);
            GameOver = false;
            Winner = "";
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
            //Don't allow button press if game over
            if (GameOver || Promoting) { return; }
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
                        bool determineValid = hypotheticalChess.DetermineValid(coordstring, currentlyselected, dictionaries.Board);
                        if (!determineValid)
                        {
                            //Move cannot be done, set the button to transparent and unselect it.
                            Button currentButton = (Button)Controls.Find(currentlyselected, true)[0];
                            currentButton.BackColor = Color.Transparent;
                            currentButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                            currentlyselected = "";
                        } else
                        {
                            Move(i, j);
                            if (turn == "white") { turn = "black"; } else { turn = "white"; }
                            StalemateCheck();
                        }
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
                    bool determineValid = hypotheticalChess.DetermineValid(coordstring, currentlyselected, dictionaries.Board);
                    if (!determineValid)
                    {
                        //Move cannot be done, set the button to transparent and unselect it.
                        Button currentButton = (Button)Controls.Find(currentlyselected, true)[0];
                        currentButton.BackColor = Color.Transparent;
                        currentButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                        currentlyselected = "";
                    }
                    else
                    {
                        //Move does not put user into check, it can be done
                        Move(i, j);
                        if (turn == "white") { turn = "black"; } else { turn = "white"; }
                        StalemateCheck();
                    }
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
        private async void StalemateCheck()
        {
            if (movecounter<10) return;
            return; //Temporary measure
            string FEN = GetFEN();
            string dbResponse = await ChessDB.Test(FEN);
            MessageBox.Show(dbResponse);
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
            string pieceAtCoord = dictionaries.GetBoard(i, j);
            string currentlySelectedPiece = dictionaries.GetPieceWithCoordString(currentlyselected);
            //Set piece at coord to currently selected piece
            dictionaries.SetBoard(i, j, currentlySelectedPiece);
            //Get current coords
            int currentlyi = (int)char.GetNumericValue(currentlyselected[0]);
            int currentlyj = (int)char.GetNumericValue(currentlyselected[1]);
            currentlyselected = ""; //Set currentlyselected to nothing
            dictionaries.SetBoard(currentlyi, currentlyj, ""); //Set old pos to nothing as piece has moved
            if (castling)
            {
                //Castling, need rook value and coordinates.
                string currentRook = dictionaries.GetPieceWithCoordString(rookOldCoords);
                int oldRookI = (int)char.GetNumericValue(rookOldCoords[0]);
                int oldRookJ = (int)char.GetNumericValue(rookOldCoords[1]);
                int newRookI = (int)char.GetNumericValue(rookNewCoords[0]);
                int newRookJ = (int)char.GetNumericValue(rookNewCoords[1]);
                dictionaries.SetBoard(oldRookI, oldRookJ, "");
                dictionaries.SetBoard(newRookI, newRookJ, currentRook);
                castling = false;
            }
            if (EnPassanting)
            {
                //En passanting, set the coords to empty
                dictionaries.SetBoard(EnPassantRemove.Item1, EnPassantRemove.Item2, "");
                EnPassanting = false;
            }
            Check(); //See if either king is in check
            SetImages();
            ClearUnselected(); //Clear image backgrounds
            if (pieceAtCoord=="") { halfmoves++; } else { halfmoves = 0; }
            if (currentlySelectedPiece=="WK") { whiteKingMoved= true; }
            else if (currentlySelectedPiece=="BK") { blackKingMoved= true; }
            else if (currentlySelectedPiece == "WR" || currentlySelectedPiece == "BR")
            {
                if (currentlyi == 0 && currentlyj == 7) { zeroSevenRookMoved = true; }
                else if (currentlyi == 7 && currentlyj == 7) { sevenSevenRookMoved = true; }
                else if (currentlyi == 0 && currentlyj == 0) { zeroZeroRookMoved = true; }
                else if (currentlyi == 7 && currentlyj == 0) {  sevenZeroRookMoved = true; }
            }
            else if (currentlySelectedPiece == "WP" || currentlySelectedPiece=="BP")
            {
                halfmoves = 0;
            } 
            //Iterate through the en passant list and add one to the movecount
            foreach (var data in EnPassantList)
            {
                data.MoveCount++;
            }
            //Check each element's move count and remove if it has been there for >1 move
            for (int x=EnPassantList.Count-1; x>=0;x--)
            {
                if (EnPassantList[x].MoveCount>1)
                {
                    EnPassantList.RemoveAt(x);
                }
            }
            if (turn=="black")
            {
                fullmoves++;
            }
            movecounter++;
        }
        private string GetEnPassantFEN()
        {
            if (EnPassantList.Count==0)
            {
                return "-";
            } 
            else
            {
                string normalCoords = GetNormalCoords(EnPassantList[0].Empty.Item1, EnPassantList[0].Empty.Item2);
                return normalCoords;
            }
        }
        private string GetNormalCoords(int i,int j)
        {
            List<string> rows = new List<string>
                {
                    "a","b","c","d","e","f","g","h"
                };
            List<string> cols;
            if (colour=="white")
            {
                cols = new List<string>
                {
                    "8","7","6","5","4","3","2","1"
                };
            } 
            else
            {
                cols = new List<string>
                {
                    "1","2","3","4","5","6","7","8"
                };
            }
            return rows[i] + cols[j];
        }
        static string GetFENPiece(string piece)
        {
            switch (piece)
            {
                case "WK":
                    return "K";
                case "BK":
                    return "k";
                case "WH":
                    return "N";
                case "BH":
                    return "n";
                case "WP":
                    return "P";
                case "BP":
                    return "p";
                case "WB":
                    return "B";
                case "BB":
                    return "b";
                case "WR":
                    return "R";
                case "BR":
                    return "r";
                case "WQ":
                    return "Q";
                case "BQ":
                    return "q";
                default:
                    return ""; // Empty square
            }
        }
        private bool CheckForCastle(string kingColour, string direction)
        {
            //Takes in king colour and checks if that king can castle
            if (kingColour=="white")
            {
                //Instantly return false if king in check
                if (WKIC) { return false; }
                if (colour=="white")
                {
                    string kingCoord = dictionaries.FindIndex("WK");
                    int kingI = (int)char.GetNumericValue(kingCoord[0]);
                    int kingJ = (int)char.GetNumericValue(kingCoord[1]);
                    //If white king moved, returns false no matter what
                    if (whiteKingMoved) { return false; }
                    //Left direction needs rook at (0,7) to have not moved.
                    if (direction=="left"&&zeroSevenRookMoved) { return false; }
                    //Right direction needs rook at (7,7) to have not moved.
                    if (direction == "right" && sevenSevenRookMoved) { return false; }
                    if (direction=="left")
                    {
                        int emptyCounter = 0;
                        for (int i = 1; i< 4;i++)
                        {
                            //If not empty, break out of loop. If empty, add to counter.
                            if (dictionaries.GetBoard(kingI-i,kingJ)!="") { break; } else {  emptyCounter++; }
                        }
                        if (emptyCounter<3) {  return false; }
                        rookOldCoords = "07";
                        rookNewCoords = "37";
                        return hypotheticalChess.DetermineValid("37", "47", dictionaries.Board) && hypotheticalChess.DetermineValid("27", "47", dictionaries.Board);
                    }
                    if (direction=="right")
                    {
                        int emptyCounter = 0;
                        for (int i=1; i< 3;i++)
                        {
                            if (dictionaries.GetBoard(kingI+1,kingJ)!="") { break; } else { emptyCounter++; }
                        }
                        if (emptyCounter<2) { return false; }
                        rookOldCoords = "77";
                        rookNewCoords = "57";
                        return hypotheticalChess.DetermineValid("57", "47", dictionaries.Board) || !hypotheticalChess.DetermineValid("67", "47", dictionaries.Board);
                    }
                }
                if (colour=="black")
                {
                    string kingCoord = dictionaries.FindIndex("WK");
                    int kingI = (int)char.GetNumericValue(kingCoord[0]);
                    int kingJ = (int)char.GetNumericValue(kingCoord[1]);
                    //If white king moved, returns false no matter what
                    if (whiteKingMoved) { return false; }
                    //Left direction needs rook at (7,0) to have not moved.
                    if (direction == "left" && sevenZeroRookMoved) { return false; }
                    //Right direction needs rook at (0,0) to have not moved.
                    if (direction == "right" && zeroZeroRookMoved) { return false; }
                    if (direction == "left")
                    {
                        int emptyCounter = 0;
                        for (int i = 1; i < 4; i++)
                        {
                            //If not empty, break out of loop. If empty, add to counter.
                            if (dictionaries.GetBoard(kingI + i, kingJ) != "") { break; } else { emptyCounter++; }
                        }
                        if (emptyCounter < 3) { return false; }
                        rookOldCoords = "70";
                        rookNewCoords = "40";
                        return hypotheticalChess.DetermineValid("50", "40", dictionaries.Board) && hypotheticalChess.DetermineValid("60", "40", dictionaries.Board);
                    }
                    if (direction == "right")
                    {
                        int emptyCounter = 0;
                        for (int i = 1; i < 3; i++)
                        {
                            if (dictionaries.GetBoard(kingI - 1, kingJ) != "") { break; } else { emptyCounter++; }
                        }
                        if (emptyCounter < 2) { return false; }
                        rookOldCoords = "00";
                        rookNewCoords = "20";
                        return hypotheticalChess.DetermineValid("30", "40", dictionaries.Board) || !hypotheticalChess.DetermineValid("20", "40", dictionaries.Board);
                    }
                }
            }
            else if (kingColour == "black")
            {
                //Instantly return false if king in check
                if (BKIC) { return false; }
                if (colour == "black")
                {
                    string kingCoord = dictionaries.FindIndex("BK");
                    int kingI = (int)char.GetNumericValue(kingCoord[0]);
                    int kingJ = (int)char.GetNumericValue(kingCoord[1]);
                    //If black king moved, returns false no matter what
                    if (blackKingMoved) { return false; }
                    //Left direction needs rook at (0,7) to have not moved.
                    if (direction == "left" && zeroSevenRookMoved) { return false; }
                    //Right direction needs rook at (7,7) to have not moved.
                    if (direction == "right" && sevenSevenRookMoved) { return false; }
                    if (direction == "left")
                    {
                        int emptyCounter = 0;
                        for (int i = 1; i < 3; i++)
                        {
                            //If not empty, break out of loop. If empty, add to counter.
                            if (dictionaries.GetBoard(kingI - i, kingJ) != "") { break; } else { emptyCounter++; }
                        }
                        if (emptyCounter < 2) { return false; }
                        rookOldCoords = "07";
                        rookNewCoords = "27";
                        return hypotheticalChess.DetermineValid("27", "37", dictionaries.Board) && hypotheticalChess.DetermineValid("17", "37", dictionaries.Board);
                    }
                    if (direction == "right")
                    {
                        int emptyCounter = 0;
                        for (int i = 1; i < 4; i++)
                        {
                            if (dictionaries.GetBoard(kingI + 1, kingJ) != "") { break; } else { emptyCounter++; }
                        }
                        if (emptyCounter < 3) { return false; }
                        rookOldCoords = "77";
                        rookNewCoords = "47";
                        return hypotheticalChess.DetermineValid("57", "47", dictionaries.Board) || !hypotheticalChess.DetermineValid("67", "47", dictionaries.Board);
                    }
                }
                if (colour == "white")
                {
                    string kingCoord = dictionaries.FindIndex("BK");
                    int kingI = (int)char.GetNumericValue(kingCoord[0]);
                    int kingJ = (int)char.GetNumericValue(kingCoord[1]);
                    //If white king moved, returns false no matter what
                    if (blackKingMoved) { return false; }
                    //Left direction needs rook at (7,0) to have not moved.
                    if (direction == "left" && sevenZeroRookMoved) { return false; }
                    //Right direction needs rook at (0,0) to have not moved.
                    if (direction == "right" && zeroZeroRookMoved) { return false; }
                    if (direction == "left")
                    {
                        int emptyCounter = 0;
                        for (int i = 1; i < 3; i++)
                        {
                            //If not empty, break out of loop. If empty, add to counter.
                            if (dictionaries.GetBoard(kingI + i, kingJ) != "") { break; } else { emptyCounter++; }
                        }
                        if (emptyCounter < 2) { return false; }
                        rookOldCoords = "70";
                        rookNewCoords = "50";
                        return hypotheticalChess.DetermineValid("50", "40", dictionaries.Board) && hypotheticalChess.DetermineValid("60", "40", dictionaries.Board);
                    }
                    if (direction == "right")
                    {
                        int emptyCounter = 0;
                        for (int i = 1; i < 4; i++)
                        {
                            if (dictionaries.GetBoard(kingI - 1, kingJ) != "") { break; } else { emptyCounter++; }
                        }
                        if (emptyCounter < 3) { return false; }
                        rookOldCoords = "00";
                        rookNewCoords = "30";
                        return hypotheticalChess.DetermineValid("30", "40", dictionaries.Board) || !hypotheticalChess.DetermineValid("20", "40", dictionaries.Board);
                    }
                }
            }
            return false;
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
        private string GetCastlingAvailability()
        {
            string castlingAvailability = "";
            if (colour == "white")
            {
                if (!whiteKingMoved)
                {
                    if (!sevenSevenRookMoved) { castlingAvailability += "K"; }
                    if (!zeroSevenRookMoved) { castlingAvailability += "Q"; }
                }
                if (!blackKingMoved)
                {
                    if (!sevenZeroRookMoved) { castlingAvailability += "k"; }
                    if (!zeroZeroRookMoved) { castlingAvailability += "q"; }
                }
            }
            else if (colour == "black")
            {
                if (!whiteKingMoved)
                {
                    if (!zeroZeroRookMoved) { castlingAvailability += "K"; }
                    if (!sevenZeroRookMoved) { castlingAvailability += "Q"; }
                }
                if (!blackKingMoved)
                {
                    if (!zeroSevenRookMoved) { castlingAvailability += "k"; }
                    if (sevenSevenRookMoved) { castlingAvailability += "q"; }
                }
            }
            if (castlingAvailability.Length==0)
            {
                return "-";
            }
            return castlingAvailability;
        }
        private string GetFEN()
        {
            string fen = "";
            int emptySquareCount = 0;
            for (int col = 0; col < 8; col++)
            {
                for (int row = 0; row < 8; row++)
                {
                    var position = (row,col);
                    string piece = dictionaries.Board[position];
                    if (piece=="")
                    {
                        emptySquareCount++;
                    } 
                    else
                    {
                        if (emptySquareCount>0)
                        {
                            fen += emptySquareCount.ToString();
                            emptySquareCount = 0;
                        }
                        fen += GetFENPiece(piece);
                    }
                }
                if (emptySquareCount>0)
                {
                    fen += emptySquareCount.ToString();
                    emptySquareCount = 0;
                }
                if (col<7)
                {
                    fen += "/";
                }
            }
            fen += " ";
            fen += turn.Substring(0,1) + " ";
            fen += GetCastlingAvailability() + " ";
            fen += GetEnPassantFEN() + " ";
            fen += halfmoves.ToString() + " ";
            fen += fullmoves.ToString() + " ";
            return fen;
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
            string pieceabbrev = dictionaries.GetPieceWithCoordString(coordstring);
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
                    if (Math.Abs(idiff)==1&&Math.Abs(jdiff)==1)
                    {
                        //Only time this will be valid is for en passant
                        foreach (var data in EnPassantList)
                        {
                            //Must be different colour and attempted move should be the empty value
                            if (data.Colour!=curcolour && data.Empty==(i,j))
                            {
                                //Set the boolean to true and the remove to the attribute
                                EnPassanting = true;
                                EnPassantRemove = data.Take;
                                return true;
                            }
                        }
                    }
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
                                EnPassantList.Add(new EnPassantData((curi, curj + 1), (curi, curj + 2),curcolour));
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
                                EnPassantList.Add(new EnPassantData((curi,curj-1), (curi,curj-2),curcolour));
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
            else if (curabbrev=="WKIC"||curabbrev=="BKIC"||curabbrev=="WK"||curabbrev=="BK")
            {
                //Can only move by a maximum of 1 in each direction, abs must be either 0 or 1
                if (Math.Abs(i-curi)<=1 && Math.Abs(j-curj)<=1)
                {
                    return true;
                } else
                {
                    //King attempting to castle possibly
                    if ((curabbrev=="WK"||curabbrev=="BK")&&Math.Abs(i-curi)==2&&j-curj==0)
                    {
                        string direction;
                        if (curcolour == colour)
                        {
                            if (i - curi == -2) { direction = "left"; }
                            else { direction = "right"; }
                        }
                        else
                        {
                            if (i - curi == -2) { direction = "right"; }
                            else { direction = "left"; }
                        }
                        bool castle = CheckForCastle(curcolour, direction);
                        if (castle) { castling = true; }
                        /*if (!castle)
                        {
                            MessageBox.Show($"Could not castle. curcolour: [{curcolour}], direction: [{direction}]");
                        }*/
                        return castle;
                    }
                    return false;
                }
            }
            return false;
        }
        private void Check()
        {
            //Get king coords
            string WK_coord = dictionaries.FindIndex("WK");
            string WKIC_coord = dictionaries.FindIndex("WKIC");
            string BK_coord = dictionaries.FindIndex("BK");
            string BKIC_coord = dictionaries.FindIndex("BKIC");
            if (WK_coord==""&&WKIC_coord=="")
            {
                //No white king for some reason
                return;
            }
            if (BK_coord==""&&BKIC_coord=="")
            {
                //No black king for some reason
                return;
            }
            int whitekingcounter = 0;
            int blackkingcounter = 0;
            for (int i = 0;i<8;i++)
            {
                for (int j = 0;j<8;j++)
                {
                    string currentCoord = i.ToString() + j.ToString();
                    if (WK_coord!="") 
                    {
                        //If white king exists, not in check
                        if (Viable(WK_coord, currentCoord, false, "")&&dictionaries.GetPieceColour(dictionaries.GetPieceWithCoordString(currentCoord))!="white")
                        {
                            //Piece can take king and isn't of the same colour
                            WKIC = true;
                            whitekingcounter++;
                        }
                    } else if (WKIC_coord!="")
                    {
                        //If white king in check exists
                        if (Viable(WK_coord, currentCoord, false, "") && dictionaries.GetPieceColour(dictionaries.GetPieceWithCoordString(currentCoord)) != "white")
                        {
                            //Piece can take king and isn't of the same colour
                            WKIC = true;
                            whitekingcounter++;
                        }
                    }
                    if (BK_coord!="")
                    {
                        //If black king exists but not in check
                        if (Viable(BK_coord,currentCoord, false,"")&&dictionaries.GetPieceColour(dictionaries.GetPieceWithCoordString(currentCoord))!="black")
                        {
                            //Piece can take king and isnt of same colour
                            BKIC = true;
                            blackkingcounter++;
                        }
                    } else if (BKIC_coord!="")
                    {
                        //If black king exists but in check
                        if (Viable(BKIC_coord,currentCoord,false,"")&&dictionaries.GetPieceColour(dictionaries.GetPieceWithCoordString(currentCoord))!="black")
                        {
                            //Piece can take king and isnt of same colour
                            BKIC = true;
                            blackkingcounter++;
                        }
                    }
                }
            }
            if (whitekingcounter == 0)
            {
                //White king not in check, set variable to false
                WKIC = false;
                if (WKIC_coord!="") 
                {
                    //Need to get coordinates before using set board
                    int i = (int)char.GetNumericValue(WKIC_coord[0]);
                    int j = (int)char.GetNumericValue(WKIC_coord[1]);
                    //Set back to "WK" value as no longer in check
                    dictionaries.SetBoard(i, j, "WK");
                }
            }
            if (blackkingcounter == 0)
            {
                BKIC = false;
                if (BKIC_coord!="") 
                {
                    //Need to get the coordinates before using set board
                    int i = (int)char.GetNumericValue(BKIC_coord[0]);
                    int j = (int)char.GetNumericValue(BKIC_coord[1]);
                    //Set back to "BK" value as no longer in check
                    dictionaries.SetBoard(i, j, "BK");
                }
            }
            if (WKIC)
            {
                if (WK_coord!="")
                {
                    //Need to get the coordinates before using set board
                    int i = (int)char.GetNumericValue(WK_coord[0]);
                    int j = (int)char.GetNumericValue(WK_coord[1]);
                    //Set to "WKIC" value as now in check
                    dictionaries.SetBoard(i, j, "WKIC");
                }
            }
            if (BKIC)
            {
                if (BK_coord!="")
                {
                    //Need to get the coordinates before using set board
                    int i = (int)char.GetNumericValue(BK_coord[0]);
                    int j = (int)char.GetNumericValue(BK_coord[1]);
                    //Set to "BKIC" value as now in check
                    dictionaries.SetBoard(i, j, "BKIC");
                }
            }
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
