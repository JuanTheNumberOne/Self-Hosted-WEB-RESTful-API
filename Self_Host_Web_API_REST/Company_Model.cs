using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self_Host_Web_API_REST
{
    public class Company_Model
    {
        public string Name { get; set; }
        public int EstablishmentYear { get; set; }

        public List<Employee_Model> Employees { get; set; }

        public Company_Model()
        {
            Employees = new List<Employee_Model>();
        }
    }
}
