using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self_Host_Web_API_REST
{
    public class Employee_Model
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime DateOfBirth { get; set; }

        public enum Jobs
        {
            Administrator,
            Developer,
            Architect,
            Manager
        }

        public Jobs JobTitle { get; set; }
    }
}
