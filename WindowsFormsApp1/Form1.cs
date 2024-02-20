using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Chess2 ChessGUI;
        private static readonly HttpClient client = new HttpClient();
        private string ServerURL = "http://77.102.118.179:80/users";
        private string Authentication = "WzYsEiJrjHX.4L0fbv_PQmwthFqGnD,8U1o3p-9x2lBKc5aZuNyRgS6T7ICMeVdkAaOA";

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
                Name = "UsernameBox",
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
                Name = "SubmitLogin",
                Text = "Submit", //Set the text of the button
                Location = new Point(15,70) //Below the password label
            };
            SubmitButton.Click += SubmitButtonClick;
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
                Name = "UsernameBox",
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
                Name = "EmailBox",
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
                Name = "FirstNameBox",
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
                Name = "LastNameBox",
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
                Name = "SubmitSignup",
                Text = "Submit", //Set the text of the button
                Location = new Point(15, 160) //Below the password label
            };
            SubmitButton.Click += SubmitButtonClick; //Add event handler for when button is clicked
            Controls.Add(SubmitButton); //Add to controls
            Button ExitButton = new Button() //Button for exiting GUI
            {
                Text = "Exit", //Set text of button
                Location = new Point(113, 160) //Same Y as submit but 98 pixels away
            };
            ExitButton.Click += ExitGUI; //Add event handler for when button is clicked
            Controls.Add(ExitButton);
        }
        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private async void SubmitButtonClick(object sender, EventArgs e)
        {
            string Type = ""; //Set type to empty incase the code doesn't execute as intended.
            if (sender is Button clickedButton) //Cast to button
            {
                string ButtonName = clickedButton.Name; //Get name
                if (ButtonName == "SubmitSignup") //If sign up page
                {
                    Type = "Signup"; //Set type
                }
                else if (ButtonName == "SubmitLogin") //If login page
                {
                    Type = "Login"; //Set type
                } else
                {
                    //This should not occur but error handling for if it does.
                    MessageBox.Show("Somehow managed to get a value of " + ButtonName + " as the button name??");
                }
            }
            if (Type == "Signup") //If sender was the signup page button
            {
                string Email = ((TextBox)Controls["EmailBox"]).Text.Trim();//Find box using name and get text attribute
                string Username = ((TextBox)Controls["UsernameBox"]).Text.Trim(); //Find box using name and get text attribute
                string Password = ((TextBox)Controls["PasswordBox"]).Text.Trim(); //Find box using name and get text attribute
                string Firstname = ((TextBox)Controls["FirstNameBox"]).Text.Trim();//Find box using name and get text attribute
                string Lastname = ((TextBox)Controls["LastNameBox"]).Text.Trim();//Find box using name and get text attribute
                if (Email == "" || Username == "" || Password == "" || Firstname == "" | Lastname == "")
                {
                    MessageBox.Show("Boxes cannot be empty.");
                }
                else
                {
                    Validation ValidClass = new Validation(Email, Username, Password);
                    if (!ValidClass.Valid)
                    {
                        MessageBox.Show(ValidClass.Reason);
                    }
                    else
                    {
                        //MessageBox.Show("Information valid.");
                        //Test if user exists first or not
                        string resultEntity = GetUser(Username);
                        bool found=false;
                        try
                        {
                            BsonDocument bsonDocument = BsonSerializer.Deserialize<BsonDocument>(resultEntity);
                            found = true;
                        }
                        catch
                        {
                            //Don't need to show this as this is just for finding the user.
                            //If the user doesn't exist, it is created here and so we don't need this shown
                            //MessageBox.Show(resultEntity);
                        }
                        if (!found)
                        {
                            //If not found yet, see if email exists
                            found = FindEmail(Email);
                        }
                        if (!found)
                        {
                            //Attempt to create account here
                            string hashedPassword = ComputeSha256Hash(Password);
                            var values = new Dictionary<string, string>
                            {
                            { "authorization", Authentication},
                            { "firstname", Firstname},
                            { "lastname", Lastname},
                            { "username", Username},
                            { "email", Email},
                            { "password", hashedPassword}
                            };
                            string jsonContent = JsonConvert.SerializeObject(values);
                            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync(ServerURL, content);
                            var responseString = await response.Content.ReadAsStringAsync();
                            //MessageBox.Show(responseString);
                            ShowChessGUI(Username);
                        } else
                        {
                            MessageBox.Show("User with this username/email already exists.");
                        }
                    }
                }
            } else if (Type == "Login") //If sender was the login page button
            {
                //Login code
                string Username = ((TextBox)Controls["UsernameBox"]).Text.Trim(); //Find box using name and get text attribute
                string Password = ((TextBox)Controls["PasswordBox"]).Text.Trim(); //Find box using name and get text attribute
                if (Username == "" || Password == "") 
                {
                    MessageBox.Show("Boxes cannot be blank");
                    return;
                }//Do not allow null values
                string hashedPassword = ComputeSha256Hash(Password); //Hash password
                bool found = false;
                string resultEntity;
                BsonDocument bsonDocument;
                Dictionary<string, object> dict;
                resultEntity = GetUser(Username);
                try
                {
                    bsonDocument = BsonSerializer.Deserialize<BsonDocument>(resultEntity);
                    found = true;
                }
                catch
                {
                    try
                    {
                        var bsonArray = BsonSerializer.Deserialize<BsonDocument[]>(resultEntity);
                        //Shouldn't have an array here but if there is, do nothing
                        //MessageBox.Show("Bson array parsed");
                        return;
                    }
                    catch
                    {
                        MessageBox.Show(resultEntity);
                    }
                }
                if (found)
                {
                    bsonDocument = BsonSerializer.Deserialize<BsonDocument>(resultEntity);
                    dict = ToDictionary(bsonDocument);
                    if (dict["password"].ToString()==hashedPassword)
                    {
                        MessageBox.Show("Password correct.");
                        ShowChessGUI(Username);
                    } else
                    {
                        MessageBox.Show("Password incorrect.");
                    }
                }
            }
        }
        private string GetUser(string Username)
        {
            string resultEntity = "";
            var values = new Dictionary<string, string>
                        {
                            { "authorization", Authentication},
                        };
            string jsonContent = JsonConvert.SerializeObject(values);
            //Create URL to access specific user
            string tempURL = ServerURL + "/" + Username;
            var request = WebRequest.Create(tempURL);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers.Add("authorization", Authentication);
            var type = request.GetType();
            var currentMethod = type.GetProperty("CurrentMethod", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(request);

            var methodType = currentMethod.GetType();
            methodType.GetField("ContentBodyNotAllowed", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(currentMethod, false);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonContent);
            }
            try
            {
                var response = request.GetResponse();
                var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                    resultEntity = myStreamReader.ReadToEnd();
                }
                responseStream.Close();
                response.Close();
                return resultEntity;
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse httpResponse)
                {
                    if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        return "User not found (404)";
                    }
                    else
                    {
                        return $"Other HTTP error: {httpResponse.StatusCode}";
                    }
                }
                else
                {
                    return $"WebException: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
        private bool FindEmail(string Email)
        {
            //Set string array using empty get
            string resultEntity = GetUser("");
            BsonDocument[] bsonArray;
            try
            {
                //Will be an array if anything is returned as empty get request always returns BSON array
                bsonArray = BsonSerializer.Deserialize<BsonDocument[]>(resultEntity);
            }
            catch
            {
                //Anything other than the array will be an error
                MessageBox.Show(resultEntity);
                return false;
            }
            foreach (BsonDocument bsonDoc in bsonArray)
            {
                if (bsonDoc is BsonDocument)
                {
                    BsonValue value2;
                    if (bsonDoc.TryGetValue("email", out value2))
                    {
                        if (value2.ToString()==Email)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private Dictionary<string, object> ToDictionary(BsonDocument document)
        {
            var dict = new Dictionary<string, object>();

            foreach (var element in document)
            {
                dict[element.Name] = BsonTypeMapper.MapToDotNetValue(element.Value);
            }

            return dict;
        }
        private void ShowChessGUI (string username)
        {
            ChessGUI = new Chess2(username);
            ChessGUI.Show();
            Controls.Clear();
            Hide();
        }
    }
}
