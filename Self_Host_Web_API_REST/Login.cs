using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self_Host_Web_API_REST
{
    class Login
    {
        private string User;
        private string Password;

        //Create the User and the password
        public Login()
        {
            User = "User";
            Password = "Password";
        }

        //Check if the login is correct; The user name can be lower or upper case
        internal bool Authenticate_User (string _User, string _Password)
        {
            bool Login_Result = false;

            Login_Result = (User.Equals(_User,StringComparison.OrdinalIgnoreCase) && Password == _Password) ? true : false;

            return Login_Result;
        }
    }
}
