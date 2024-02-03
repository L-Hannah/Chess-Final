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

namespace WindowsFormsApp1
{
    public partial class Chess2 : Form
    {
        string Username;
        string colour;
        Dictionaries dictionaries;
        public Chess2(string username)
        {
            Username = username;
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
            Text = $"Chess Project - Signed in as {Username}"; //Change title of window
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
                        BackColor= Color.Transparent //Set background to transparent
                    };
                    SetButtonImage(TempButton, i.ToString() + j.ToString());
                    Controls.Add(TempButton); //Add button to form controls
                    TempButton.BringToFront(); //Bring button forward
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
