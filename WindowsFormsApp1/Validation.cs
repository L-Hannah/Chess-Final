using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    internal class Validation
    {
        private bool valid;//Internal variable for validity, will have a getter and setter method
        private string reason;//Internal variable for reason, if valid is false reason will be questioned
        public Validation(string email, string username, string password) 
        {
            Validate(email, username, password); //Instantly validate
        }
        private void Validate(string email, string username, string password)
        {
            string EmailValid = EmailValidation(email); //Check if email valid
            string PasswordValid = PasswordValidation(password);//Check if password valid
            if (EmailValid!="Email accepted") //If email not accepted, use the returned response as the reason
            {
                valid = false; //Set valid to false
                reason = EmailValid; //Set the reason as the returned response
            }
            else if (username.Length < 6 || username.Length > 32) //Check username length
            {
                valid =  false; //Set valid to false
                reason = "Username must be between 6 and 32 characters."; //Set pre coded reason
            } else if (username.Contains(" ")) //Check for a space
            {
                valid = false; //Set valid to false
                reason = "Username must not contain spaces."; //Set pre coded reason
            }
            else if (PasswordValid != "Password accepted") //If password is not accepted
            {
                valid = false; //Set valid to false
                reason = PasswordValid; //Set the reason as the returned response
            } else
            {
                valid = true; //If no flags determine the password to be false, it can be defined as true.
            }
            
        }
        private string PasswordValidation(string password)
        {
            bool capitalLetter = false; //Flags for each character
            bool lowercaseLetter = false;
            bool number = false;
            bool specialChar = false;
            for (int i = 0; i < password.Length; i++) //For loop to iterate through password
            {
                if ("1234567890".Contains(password[i])) { number = true; } //Checks for numbers
                if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(password[i] )) { capitalLetter = true; } //Checking for capital letters
                if ("abcdefghijklmnopqrstuvwxyz".Contains(password[i])) {  lowercaseLetter = true; }//Checking for lowercase letters
                if ("(!£$%^&*)".Contains(password[i])) { specialChar = true; } //Checking for special characters
            }
            if (string.IsNullOrEmpty(password)) //Check if password is null
            {
                return "Password cannot be empty";
            } else if (password.Length<6 || password.Length>32) //Check password length
            {
                return "Length of password incorrect. Should be 6-32 characters.";
            }
            else if (password.Contains(" ")) //Check for spaces
            {
                return "Password cannot contain spaces";
            } else if (!number) //Check number flag
            {
                return "Password must contain numbers";
            } else if (!specialChar)//Check special character flag
            {
                return "Password must contain special characters (!£$%^&*).";
            } else if (!lowercaseLetter)//Check lowercase flag
            {
                return "Password must contain a lowercase letter.";
            } else if (!capitalLetter)//Check capital letter flag
            {
                return "Password must contain a capital letter.";
            } else
            {
                return "Password accepted"; //All else succeeds, no issues. Return accepted
            }
        }
        private string EmailValidation(string email)
        {
            return "";
        }
        public bool Valid //Getter and setter methods
        {
            get { return valid; } //Return value of internal variable
            set { valid = value; } //Set to value passed in
        }
        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }
    }
}
