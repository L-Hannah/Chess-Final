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

namespace WindowsFormsApp1
{
    public partial class Chess2 : Form
    {
        string Username;
        public Chess2(string username)
        {
            Username = username;
            InitializeComponent();
        }

        private void Chess2_Load(object sender, EventArgs e)
        {
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
