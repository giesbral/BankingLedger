using BankingLedger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Globalization;

namespace BankingLedger.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void MenuSelectTest1()
        {
            int expected = 1;

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", expected, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                var loginMenu = new[] { "1. Login", "2. Create New User" };

                int selected = testProgram.MenuSelect(loginMenu);

                Assert.AreEqual<int>(expected, selected);
            }
        }

        [TestMethod()]
        public void MenuSelectTestNeg()
        {
            double input = -1.284;
            var loginMenu = new[] { "1. Login", "2. Create New User" };
            string expected = "Invalid Selection";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", input, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                try
                {
                    testProgram.MenuSelect(loginMenu);
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expected, e.Message);
                }    
            }
        }

        [TestMethod()]
        public void MenuSelectTestLen()
        {
            var loginMenu = new[] { "1. Login", "2. Create New User" };

            int input = loginMenu.Length + 1;
            string expected = "Invalid Selection";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", input, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                try
                {
                    testProgram.MenuSelect(loginMenu);
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expected, e.Message);
                }
            }
        }

        [TestMethod()]
        public void GetInputAsIntTest1()
        {
            int expected = 1;

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", expected, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                int output = testProgram.GetInputAsInt();

                Assert.AreEqual<int>(expected, output);
            }
        }

        [TestMethod()]
        public void GetInputAsIntTestStr()
        {
            int expected = -1;
            string input = "ABC";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", input, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                int output = testProgram.GetInputAsInt();

                Assert.AreEqual<int>(expected, output);
            }
        }

        [TestMethod()]
        public void GetInputAsIntTestDbl()
        {
            int expected = -1;
            double input = 1.1;

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", input, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                int output = testProgram.GetInputAsInt();

                Assert.AreEqual<int>(expected, output);
            }
        }

        [TestMethod()]
        public void CreateUserTest()
        {
            string userName = "testing";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            Program testProgram = new Program();
            User expectedUser = new User(userName, userPass, fName, mName, lName, bDay);
            User outputUser = new User();

            using (StringReader sReader = new StringReader(string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}{6}", userName, userPass, fName, mName, lName, bDay, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                outputUser = testProgram.CU.CreateUser();

                Assert.IsTrue(expectedUser.Equals(outputUser) && expectedUser.Equals(testProgram.CU.UserList[0]));
            }
        }

        // Check if an exception is thrown when attempting to add a user with a username that already exists 
        [TestMethod()]
        public void GetUsernameTestDup()
        {
            string userName = "testing";
            string userPass = "password";
            string fName = "Tester";
            string mName = "Tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string expectedAssertMsg = "Username Already Exists";

            User duplicateUser = new User(userName, userPass, fName, mName, lName, bDay);

            Program testProgram = new Program();

            testProgram.CU.UserList.Add(duplicateUser);

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", userName, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.CU.GetUsername();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }

            }
        }

        [TestMethod()]
        public void GetUsernameTestShort()
        {
            string userName = "sh";

            string expectedAssertMsg = "Username \"sh\" is too short; Username must be at least 6 characters long";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", userName, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                try
                {
                    testProgram.CU.GetUsername();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void GetPasswordTestEmpty()
        {
            string password = "";

            string expectedAssertMsg = "Password field cannot be blank";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", password, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                try
                {
                    testProgram.CU.GetPassword();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void GetPasswordTestShort()
        {
            string password = "sh";

            string expectedAssertMsg = "Password is too short; Passwords must be at least 6 characters long";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", password, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                try
                {
                    testProgram.CU.GetPassword();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void GetFirstNameEmpty()
        {
            string fName = "";

            string expectedAssertMsg = "First Name field cannot be blank";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", fName, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                try
                {
                    testProgram.CU.GetFirstName();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void GetFirstNameTestFNameHasSpaces()
        {
            string fName = "Doctor Name";

            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string expectedAssertMsg = "First Name field cannot contain spaces";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", fName, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                try
                {
                    testProgram.CU.GetFirstName();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void GetFirstNameTestHasNumbers()
        {
            string fName = "123abc";

            string expectedAssertMsg = "First Name field cannot contain numbers or special characters";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", fName, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                try
                {
                    testProgram.CU.GetFirstName();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void GetMiddleNameTestHasNumbers()
        {
            string mName = "123abc";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string expectedAssertMsg = "Middle Name field cannot contain numbers or special characters";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", mName, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                Program testProgram = new Program();

                try
                {
                    testProgram.CU.GetMiddleName();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void GetLastNameTestHasSpacesAndHyphens()
        {
            string userName = "tester";
            string userPass = "password";
            string fName = "Tester";
            string mName = "Tee";
            string lName = "McTesterone-Jasper Wassername";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            Program testProgram = new Program();
            User expectedUser = new User(userName, userPass, fName, mName, lName, bDay); 
            User outputUser = new User();

            using (StringReader sReader = new StringReader(string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}{6}", userName, userPass, fName, mName, lName, bDay, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                outputUser = testProgram.CU.CreateUser();

                Assert.IsTrue(expectedUser.Equals(outputUser));
            }
        }

        [TestMethod()]
        public void GetLastNameTestEmpty()
        {
            string lName = "";

            string expectedAssertMsg = "Last Name field cannot be blank";

            Program testProgram = new Program();

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", lName, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.CU.GetLastName();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void GetBirthDateTestNoDate()
        {
            string bDay = "";

            string expectedAssertMsg = "Birth Date field cannot be blank";

            Program testProgram = new Program();

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", bDay, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.CU.GetBirthDate();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void GetBirthDateTestBadDateFormat()
        {
            string bDay = "31/05/2000";

            string expectedAssertMsg = "Invalid Birth Date format; Correct format: mm/dd/yyyy";

            Program testProgram = new Program();

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", bDay, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.CU.GetBirthDate();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void CreateUserTestGoodDateFormat()
        {
            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            User expectedUser = new User(userName, userPass, fName, mName, lName, bDay);

            Program testProgram = new Program();

            using (StringReader sReader = new StringReader(string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}{6}", userName, userPass, fName, mName, lName, bDay, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                User outputUser = testProgram.CU.CreateUser();

                Assert.IsTrue(expectedUser.Equals(outputUser));
            }
        }

        //[TestMethod()]
        //public void LoginUserTestValid()
        //{
        //    Program testProgram = new Program();

        //    string userName = "Tester";
        //    string userPass = "password";
        //    string fName = "tester";
        //    string mName = "tee";
        //    string lName = "McTesterone";
        //    DateTime.TryParse("12/15/2010", out DateTime bDay);

        //    string hashPass = testProgram.CU.HashPassword(userPass);

        //    User expectedUser = new User(userName, hashPass, fName, mName, lName, bDay); 

        //    testProgram.CU.UserList.Add(expectedUser);

        //    using (StringReader sReader = new StringReader(string.Format("{0}{2}{1}{2}", userName, userPass, Environment.NewLine)))
        //    {
        //        Console.SetIn(sReader);

        //        testProgram.LoginUser();

        //        Assert.IsTrue(expectedUser.Equals(testProgram.LoggedInUser));
        //    }
        //}

        [TestMethod()]
        public void AuthenticateUserTestValid()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);
            User expectedUser = new User(userName, userPass, fName, mName, lName, bDay);

            testProgram.CU.UserList.Add(testUser);

            testProgram.AuthenticateUser(userName, userPass);

            Assert.IsTrue(expectedUser.Equals(testProgram.LoggedInUser));
        }

        [TestMethod()]
        public void AuthenticateUserTestInvalid()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string badPass = "plusburd";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            testProgram.CU.UserList.Add(testUser);

            Assert.IsFalse(testProgram.AuthenticateUser(userName, badPass));
        }

        [TestMethod()]
        public void DepositTest()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            double startingAmount = 1000.15;

            testUser.Balance = startingAmount;

            testProgram.LoggedInUser = testUser;

            double depositAmount = 500.35;

            double expectedAmount = startingAmount + depositAmount;

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", depositAmount, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                testProgram.Deposit();

                Assert.IsTrue(testProgram.LoggedInUser.Balance == expectedAmount);
            }
        }

        [TestMethod()]
        public void DepositTestLettersAndNumbers()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            double startingAmount = 1000.15;

            testUser.Balance = startingAmount;

            testProgram.LoggedInUser = testUser;

            string depositAmount = "5AB.35";

            string expectedAssertMsg = "Deposit amount must be a decimal number of dollars and cents";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", depositAmount, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.Deposit();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void DepositTestBlankAmount()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            double startingAmount = 1000.15;

            testUser.Balance = startingAmount;

            testProgram.LoggedInUser = testUser;

            string depositAmount = "";

            string expectedAssertMsg = "Deposit amount cannot be empty";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", depositAmount, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.Deposit();
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void DepositTestTooMuchCents()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            double startingAmount = 1000.15;

            testUser.Balance = startingAmount;

            testProgram.LoggedInUser = testUser;

            string depositAmount = "100.805";

            string expectedAssertMsg = "Deposit amount can only be specified to two decimal places";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", depositAmount, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.Deposit();
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void DepositTestLessThanOrEqualToZero()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            double startingAmount = 1000.15;

            testUser.Balance = startingAmount;

            testProgram.LoggedInUser = testUser;

            string depositAmount = "0.00";

            string expectedAssertMsg = "Deposit amount must be greater than zero";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", depositAmount, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.Deposit();
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void WithdrawalTest()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            double startingAmount = 1000.15;

            testUser.Balance = startingAmount;

            testProgram.LoggedInUser = testUser;

            double withdrawalAmount = 500.35;

            double expectedAmount = startingAmount - withdrawalAmount;

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", withdrawalAmount, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                testProgram.Withdrawal();

                Assert.IsTrue(testProgram.LoggedInUser.Balance == expectedAmount);
            }
        }

        [TestMethod()]
        public void WithdrawalTestLettersAndNumbers()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            double startingAmount = 1000.15;

            testUser.Balance = startingAmount;

            testProgram.LoggedInUser = testUser;

            string withdrawalAmount = "5AB.35";

            string expectedAssertMsg = "Withdrawal amount must be a decimal number of dollars and cents";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", withdrawalAmount, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.Withdrawal();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void WithdrawalTestBlankAmount()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            double startingAmount = 1000.15;

            testUser.Balance = startingAmount;

            testProgram.LoggedInUser = testUser;

            string withdrawalAmount = "";

            string expectedAssertMsg = "Withdrawal amount cannot be empty";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", withdrawalAmount, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.Withdrawal();
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void WithdrawalTestTooMuchCents()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            double startingAmount = 1000.15;

            testUser.Balance = startingAmount;

            testProgram.LoggedInUser = testUser;

            string withdrawalAmount = "100.805";

            string expectedAssertMsg = "Withdrawal amount can only be specified to two decimal places";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", withdrawalAmount, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.Withdrawal();
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void WithdrawalTestLessThanOrEqualToZero()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            double startingAmount = 1000.15;

            testUser.Balance = startingAmount;

            testProgram.LoggedInUser = testUser;

            string withdrawalAmount = "0.00";

            string expectedAssertMsg = "Withdrawal amount must be greater than zero";

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", withdrawalAmount, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.Withdrawal();
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void WithdrawalTestInsufficientFunds()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            double startingAmount = 1000.15;

            testUser.Balance = startingAmount;

            testProgram.LoggedInUser = testUser;

            double withdrawalAmount = 2000;

            string expectedAssertMsg = string.Format("Insufficient Funds to withdraw {0:C2}; Current Balance: {1:C2}", withdrawalAmount, startingAmount);

            using (StringReader sReader = new StringReader(string.Format("{0}{1}", withdrawalAmount, Environment.NewLine)))
            {
                Console.SetIn(sReader);

                try
                {
                    testProgram.Withdrawal();
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    Assert.AreEqual<string>(expectedAssertMsg, e.Message);
                }
            }
        }

        [TestMethod()]
        public void CheckBalanceTest()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            double expectedAmount = 1000.15;

            testUser.Balance = expectedAmount;

            testProgram.LoggedInUser = testUser;

            double outputAmount = testProgram.CheckBalance();

            Assert.IsTrue(expectedAmount == outputAmount);
        }

        [TestMethod()]
        public void GetTransactionHistoryTest()
        {
            Program testProgram = new Program();

            string userName = "Tester";
            string userPass = "password";
            string fName = "tester";
            string mName = "tee";
            string lName = "McTesterone";
            DateTime.TryParse("12/15/2010", out DateTime bDay);

            string hashPass = testProgram.CU.HashPassword(userPass);

            User testUser = new User(userName, hashPass, fName, mName, lName, bDay);

            string expectedHistory = testUser.History[0];

            testProgram.LoggedInUser = testUser;

            string outputHistory = testProgram.GetTransactionHistory();

            Assert.AreEqual(expectedHistory, outputHistory);
        }
    }
}