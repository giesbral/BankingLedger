using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liphsoft.Crypto.Argon2; //https://github.com/alipha/csharp-argon2 Search on Liphsoft.Crypto.Argon2 on NuGet

namespace BankingLedger
{
    // User Class
    // stores information about a single user
    public class User
    {
        private string username, password, fname, mname, lname;

        private DateTime bdate;

        private double balance;

        // user's transaction history
        private List<string> history = new List<string>();

        public User()
        {
            username = null;
            password = null;
            balance = 0.0;
        }

        public User(string n, string pw, string first, string middle, string last, DateTime bday)
        {
            username = n;
            password = pw;
            fname = first;
            mname = middle;
            lname = last;
            bdate = bday;
            balance = 0.0;

            string entryDT = GetTimestamp(DateTime.Now);

            string iniHist = string.Format("{0} User Created\n", entryDT);
            history.Add(iniHist);
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Fname { get => fname; set => fname = value; }
        public string Mname { get => mname; set => mname = value; }
        public string Lname { get => lname; set => lname = value; }
        public DateTime Bdate { get => bdate; set => bdate = value; }
        public double Balance { get => balance; set => balance = value; }
        public List<string> History { get => history; set => history = value; }

        // Equals()
        // Equals method override for comparing two Users
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is User))
            {
                return false;
            }
            else if (this.username == ((User) obj).username && 
                this.history[0] == ((User)obj).history[0] &&
                this.fname == ((User)obj).fname &&
                this.mname == ((User)obj).mname &&
                this.lname == ((User)obj).lname &&
                this.bdate == ((User)obj).bdate)

            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddHistory(string entry)
        {
            history.Add(entry);
        }

        public List<string> GetHistory()
        {
            return history;
        }

        // GetTimestamp()
        // formats timestamp for user Creation
        private string GetTimestamp(DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss MM//dd//yyyy");
        }

        public override int GetHashCode()
        {
            var hashCode = 1058012924;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(username);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(password);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Username);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Password);
            return hashCode;
        }
    }
}
