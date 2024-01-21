using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Home";
            Button loginButton = new Button()
            {
                Size = new Size(150, 60),
                Location = new Point(50, 30),
                Image = Properties.Resources.login,
                FlatStyle = FlatStyle.Flat,
            };
            loginButton.FlatAppearance.BorderSize = 0;
            Controls.Add(loginButton);
            loginButton.Click += LoginPage;
            Button signupButton = new Button()
            {
                Size = new Size(150,60),
                Location = new Point(250, 30),
                Image = Properties.Resources.signup4,
                FlatStyle = FlatStyle.Flat
            };
            signupButton.FlatAppearance.BorderSize = 0;
            Controls.Add(signupButton);
            signupButton.Click += SignUpPage;
        }
        private void LoginPage(object sender, EventArgs e)
        {
            Text = "Log in";
            Controls.Clear(); //Clear entire window
            Size = new Size(270, 150); //Resize for login widgets
            Label usernameLabel = new Label() //Make a label for the username
            {
                Text = "Username: ",
                Location = new Point(5, 10),
                AutoSize=true, //Size will change for the "Username: " text
                Font = new Font("Arial",12,FontStyle.Bold)
            };
            TextBox username = new TextBox() //Text box for user input
            {
                Location = new Point(103, 10), //On same line as label but a bit later
            };
            Controls.Add(usernameLabel); //Add both to controls
            Controls.Add(username);
            Label passwordLabel = new Label()
            {
                Text = "Password: ",
                Location = new Point(5, 40),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold) //Same font as username
            };
            TextBox password = new TextBox()
            {
                Name = "PasswordBox", //Needs a name so it can be found by the button function
                UseSystemPasswordChar = true, //Set this as true initially
                Location = new Point(103,40) //On same line as label but later on
            };
            Controls.Add(passwordLabel); //Add both to controls
            Controls.Add(password);
            Button RevealPassword = new Button() //Button for revealing password
            {
                Name = "RevealButton", //name so I can find it in the button function
                Image = Properties.Resources.eye2, //Use eye2 image
                Size = new Size(26, 24), //Change size to same as image
                Location = new Point(216, 38), //Add on same line as password text box
                Tag = "eye2"
            };
            Controls.Add(RevealPassword);
            RevealPassword.Click += PasswordReveal; //Attach the PasswordReveal function
            Button SubmitButton = new Button() //Button for submitting login information
            {
                Text = "Submit", //Set the text of the button
                Location = new Point(15,70) //Below the password label
            };
            Controls.Add(SubmitButton); //Add to controls
            Button ExitButton = new Button() //Button for exiting GUI
            {
                Text = "Exit", //Set text of button
                Location = new Point(113, 70) //Same Y as submit but 98 pixels away
            };
            ExitButton.Click += ExitGUI;
            Controls.Add(ExitButton);
        }
        private void ExitGUI(object sender, EventArgs e)
        {
            Close();
        }
        private void PasswordReveal(object sender, EventArgs e)
        {
            TextBox PasswordBox = (TextBox)Controls["PasswordBox"]; //Find box using the name
            PasswordBox.UseSystemPasswordChar = !PasswordBox.UseSystemPasswordChar; //Toggle the password character
            Button RevealButton = (Button)sender; //Turn sender object into Button so that we have an Image definition to work with 
            if (RevealButton.Tag.ToString() == "eye2") //Simple if statement to change the case
            {
                RevealButton.Image= Properties.Resources.eye1; //Change image
                RevealButton.Tag = "eye1"; //Change tag to represent image change
            } 
            else if (RevealButton.Tag.ToString()=="eye1")
            { 
                RevealButton.Image = Properties.Resources.eye2; //Change image
                RevealButton.Tag = "eye2"; //Change tag to represent image
            }
        }
        private void SignUpPage(object sender, EventArgs e)
        {
            Text = "Sign up";
            Controls.Clear();
            Size = new Size(270, 250); //Resize for signup widgets
            Label usernameLabel = new Label() //Make a label for the username
            {
                Text = "Username: ",
                Location = new Point(5, 10),
                AutoSize = true, //Size will change for the "Username: " text
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            TextBox username = new TextBox() //Text box for user input
            {
                Location = new Point(105, 10), //On same line as label but a bit later
            };
            Label emailLabel = new Label()
            {
                Text = "Email: ",
                Location = new Point(5, 40),
                AutoSize = true,
                Font = new Font("Arial",12,FontStyle.Bold)
            };
            TextBox email = new TextBox()
            {
                Location = new Point(105,40)
            };
            Label firstNameLabel = new Label()
            {
                Text = "First name: ",
                Location = new Point(5, 70),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            TextBox firstName = new TextBox()
            {
                Location = new Point(105, 70)
            };
            Label lastNameLabel = new Label()
            {
                Text = "Last name: ",
                Location = new Point(5,100),
                AutoSize=true,
                Font = new Font("Arial",12,FontStyle.Bold)
            };
            TextBox lastName = new TextBox()
            {
                Location = new Point(105, 100)
            };
            Label passwordLabel = new Label()
            {
                Text = "Password: ",
                Location = new Point(5, 130),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            TextBox password = new TextBox()
            {
                Name = "PasswordBox",
                UseSystemPasswordChar = true, //Set this as true initially
                Location = new Point(105,130)
            };
            Controls.Add(usernameLabel); //Add everything to controls
            Controls.Add(username);
            Controls.Add(emailLabel);
            Controls.Add(email);
            Controls.Add(firstNameLabel);
            Controls.Add(firstName);
            Controls.Add(lastNameLabel);
            Controls.Add(lastName);
            Controls.Add(passwordLabel);
            Controls.Add(password);
            Button RevealPassword = new Button() //Button for revealing password
            {
                Name = "RevealButton", //Name so I can find it in the button function
                Image = Properties.Resources.eye2, //Use eye2 image
                Size = new Size(26, 24), //Change size to same as image
                Location = new Point(216, 128), //Add on same line as password text box
                Tag = "eye2"
            };
            Controls.Add(RevealPassword);
            RevealPassword.Click += PasswordReveal; //Attach the PasswordReveal function
            Button SubmitButton = new Button() //Button for submitting login information
            {
                Text = "Submit", //Set the text of the button
                Location = new Point(15, 160) //Below the password label
            };
            Controls.Add(SubmitButton); //Add to controls
            Button ExitButton = new Button() //Button for exiting GUI
            {
                Text = "Exit", //Set text of button
                Location = new Point(113, 160) //Same Y as submit but 98 pixels away
            };
            ExitButton.Click += ExitGUI;
            Controls.Add(ExitButton);
        }
    }
}
