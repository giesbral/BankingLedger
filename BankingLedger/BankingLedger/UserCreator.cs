using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Liphsoft.Crypto.Argon2; //https://github.com/alipha/csharp-argon2 Search on Liphsoft.Crypto.Argon2 on NuGet

namespace BankingLedger
{
    // Class for creating and storing new users
    public class UserCreator
    {
        // stores users and their information as a User object
        private List<User> userList;
        public List<User> UserList { get => userList; set => userList = value; }

        public UserCreator()
        {
            userList = new List<User>();
        }

        // CreateUser()
        // drives new user creation process and prompts
        public User CreateUser()
        {
            Console.Clear();
            Console.WriteLine("Creating a New User");
            Console.WriteLine("Please enter information for new user (* Required Fields):");

            string username = null;
            string passHash = null;
            string fName = null;
            string mName = null;
            string lName = null;
            DateTime bDate = DateTime.Now;

            bool userDone, passDone, fNameDone, mNameDone, lNameDone, bDateDone;
            userDone = passDone = fNameDone = mNameDone = lNameDone = bDateDone = false;

            // loop through user creation process until user successfully creates a new user
            do
            {
                if (!userDone)
                {
                   doUser:

                    // prompt user for username
                    try
                    {
                        username = GetUsername();
                        userDone = true;
                    }
                    // if username is invalid, redo username creation
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                        Console.Clear();
                        goto doUser;
                    }
                }

                if (!passDone)
                {
                    doPass:

                    // prompt user for password
                    try
                    {
                        passHash = GetPassword();
                        passDone = true;
                    }
                    // if password is invalid, redo password creation
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                        Console.Clear();
                        goto doPass;
                    }
                }

                if (!fNameDone)
                {
                    doFName:

                    // prompt user for first name
                    try
                    {
                        fName = GetFirstName();
                        fNameDone = true;
                    }
                    // if first name is invalid, redo first name creation
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                        Console.Clear();
                        goto doFName;
                    }
                }

                if (!mNameDone)
                {
                    doMName:

                    // prompt user for middle name (*not required)
                    try
                    {
                        mName = GetMiddleName();
                        mNameDone = true;
                    }
                    // if middle name is invalid, redo middle name creation
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                        Console.Clear();
                        goto doMName;
                    }
                }

                if (!lNameDone)
                {
                    doLName:

                    // prompt user for last name
                    try
                    {
                        lName = GetLastName();
                        lNameDone = true;
                    }
                    // if last name is invalid, redo last name creation
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                        Console.Clear();
                        goto doLName;
                    }
                }

                if (!bDateDone)
                {
                    doBDate:

                    // prompt user for birthdate
                    try
                    {
                        bDate = GetBirthDate();
                        bDateDone = true;
                    }
                    // if birthdate is invalid, redo birthdate creation
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                        Console.Clear();
                        goto doBDate;
                    }
                }

            } while (!(userDone && passDone && fNameDone && mNameDone && lNameDone && bDateDone));
            
            // create new user with credentials provided by user
            User newUser = new User(username, passHash, fName, mName, lName, bDate);

            // add new user to the list of users
            UserList.Add(newUser);

            // display new user creation confirmation
            Console.Clear();
            Console.WriteLine("\nA new user has been created with the following information:\nUsername: {0}\nName: {1} {2} {3}\nBirthday: {4}",
                newUser.Username,
                newUser.Fname,
                newUser.Mname,
                newUser.Lname,
                newUser.Bdate);

            return newUser;
        }

        // GetUsername()
        // attempts to get valid username from user
        public string GetUsername()
        {
            Console.WriteLine("\n* New Username (Usernames must be at least 6 characters long): ");
            string username = Console.ReadLine();

            // check if username is blank
            if (username == "" || username == null || username == " ")
            {
                throw new ArgumentException("Password field cannot be blank");
            }
            // check if username is too short
            else if (username.Length < 6)
            {
                throw new ArgumentException(string.Format("Username \"{0}\" is too short; Username must be at least 6 characters long", username));
            }
            // check if username already exists
            else if (UserList.Exists(x => x.Username == username))
            {
                throw new ArgumentException("Username Already Exists");
            }
            // check if username contains spaces
            else if (username.Contains(" "))
            {
                throw new ArgumentException("Username field cannot contain spaces");
            }

            return username;
        }

        // GetPassword()
        // attempts to get valid password from user
        public string GetPassword()
        {
            Console.WriteLine("\n* New User Password (Passwords must be at least 6 characters long): ");
            string pw = Console.ReadLine();
            string passHash;

            // check if password is blank
            if (pw == "" || pw == null || pw == " ")
            {
                throw new ArgumentException("Password field cannot be blank");
            }
            // check if password is too short
            if (pw.Length < 6)
            {
                throw new ArgumentException("Password is too short; Passwords must be at least 6 characters long");
            }
            // if password is valid, hash password with one-way hash algorithm
            else
            {
                passHash = HashPassword(pw);
            }

            return passHash;
        }

        // HashPassword()
        // hash password using one-way password hashing algorithm
        public string HashPassword(string password)
        {
            var hasher = new PasswordHasher();
            string passHash = hasher.Hash(password);

            return passHash;
        }

        // GetFirstName()
        // attempts to get valid first name from user 
        public string GetFirstName()
        {
            Console.WriteLine("\n* First Name: ");
            string fName = Console.ReadLine();

            // check if first name is blank
            if (fName == "" || fName == null)
            {
                throw new ArgumentException("First Name field cannot be blank");
            }
            // check if first name contains spaces
            else if (fName.Contains(" "))
            {
                throw new ArgumentException("First Name field cannot contain spaces");
            }
            // check if first name contains characters other than letters
            else if (Regex.IsMatch(fName, @"[^a-zA-Z]+"))
            {
                throw new ArgumentException("First Name field cannot contain numbers or special characters");
            }

            return fName;
        }

        // GetMiddleName()
        // attempts to get valid middle name from user
        public string GetMiddleName()
        {
            Console.WriteLine("\n  Middle Name: ");
            string mName = Console.ReadLine();

            // if middle name is not blank
            if (mName != null && mName != "" && mName != " ")
            {
                // check if middle name contains characters other than letters and spaces
                if (Regex.IsMatch(mName, @"[^a-zA-Z ]+"))
                {
                    throw new ArgumentException("Middle Name field cannot contain numbers or special characters");
                }
            }

            return mName;
        }

        // GetLastName()
        // attempts to get valid last name from user
        public string GetLastName()
        {
            Console.WriteLine("\n* Last Name: ");
            string lName = Console.ReadLine();

            // check if last name is empty
            if (lName == "" || lName == null)
            {
                throw new ArgumentException("Last Name field cannot be blank");
            }
            // check if last name contains characters other than letters, spaces, and hyphens
            else if (Regex.IsMatch(lName, @"[^a-zA-Z -]+"))
            {
                throw new ArgumentException("Last Name field can only contain letters, spaces, and hyphens (-)");
            }

            return lName;
        }

        // GetBirthDate()
        // attempts to get valid birthdate from user
        public DateTime GetBirthDate()
        {
            Console.WriteLine("\n* Birth Date (mm/dd/yyyy): ");
            string birthday = Console.ReadLine();
            DateTime bDate;

            // check if birthdate is empty
            if (birthday == "" || birthday == null)
            {
                throw new ArgumentException("Birth Date field cannot be blank");
            }
            // check if birthdate is in the proper format
            else if (DateTime.TryParse(birthday, out bDate))
            {
                String.Format("{0:MM/dd/yyyy}", bDate);
            }
            else
            {
                throw new ArgumentException("Invalid Birth Date format; Correct format: mm/dd/yyyy");
            }

            return bDate;
        }
    }
}
