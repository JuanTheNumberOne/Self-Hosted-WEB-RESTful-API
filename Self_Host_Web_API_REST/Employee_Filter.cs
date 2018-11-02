using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;

namespace Self_Host_Web_API_REST
{
    public class Employee_Filter
    {
        public string Keyword;
        public DateTime EmployeeDateOfBirthFrom;
        public DateTime EmployeeDateOfBirthTo;

        //Inherit the enum type from the employee_model
        public List<string> EmployeeJobTitles;

        private List<string> AllowableJobTitles;

        //Class constructor
        public Employee_Filter()
        {
            AllowableJobTitles = new List<string>();

            AllowableJobTitles.Add("Administrator");
            AllowableJobTitles.Add("Developer");
            AllowableJobTitles.Add("Architect");
            AllowableJobTitles.Add("Manager");
        }

        //Returns a string containing eventual wrong job titles
        public string CheckJobTitles(ICollection<Employee> Titles_provided)
        {
            List<string> Jobs_Provided = new List<string>();

            string Forbidden_Catched = "";

            //Get all the jobs from the user list
            foreach (Employee item in Titles_provided)
            {
                Jobs_Provided.Add(item.JobTitle);
            }

            if (Jobs_Provided.Count > 0)
            {
                foreach (var item in Jobs_Provided)
                {
                    if (AllowableJobTitles.Contains(item))
                    {
                        Thread.Sleep(10);
                    }
                    else
                    {
                        Forbidden_Catched += item + ';';
                    }
                }
            }

            return Forbidden_Catched;
        }

    }
}
