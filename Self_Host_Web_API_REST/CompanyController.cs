using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Self_Host_Web_API_REST
{
    public class CompanyController : ApiController
    {
        
        [LoginAuthentication]
        [HttpPost]
        // POST/company/create
        public HttpResponseMessage Create([FromBody]Company NewCompany)
        {
            long ID_Returned = 0;

            //Variables to check if allowable Job titles have been provided
            Employee_Filter Jobs_Filter = new Employee_Filter();
            List<string> Jobs_Provided = new List<string>();
            string Forbidden_jobs = "";

            //Check if the obtained info is not null (in this case int cannot be null (default value is 0))
            if ((NewCompany!=null && NewCompany.Name != "" && NewCompany.EstablishmentYear != 0) && !(NewCompany.Employees.Any(m => m.DateOfBirth == default(DateTime))))
            {
                Forbidden_jobs = Jobs_Filter.CheckJobTitles(NewCompany.Employees);
                //If no forbidden jobs found proceed
                if (Forbidden_jobs == "")
                {

                    try
                    {
                        using (Web_Api_Company_EmployeesEntities db = new Web_Api_Company_EmployeesEntities())
                        {
                    
                            //Save the new entry in the DB
                            db.Companies.Add(NewCompany);
                            db.SaveChanges();
   
                            //Get the ID of the last entry (the ID is an identity with auto incrementation)
                            var ID = db.Companies.Max(b => b.ID);
                            ID_Returned = ID;

                        }
                    }
       
                    catch (Exception e)
                    {
                        Console.WriteLine("Error while trying to write to database, check if the SQL server is online: " , e.Message);
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error when trying to post the content");
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, ID_Returned);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Some of the jobs provided are not allowed, (company not created): " + Forbidden_jobs);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data input is null, or some data is incorrect");
                
            }

        }

        [HttpPost]
        //POST/company/search
        public HttpResponseMessage Search([FromBody]Employee_Filter Search_Filter)
        {
            bool NoDate = false;
            bool NoData = false;

            if (Search_Filter !=null)
            {

                //If date to is < than date from return error
                if (Search_Filter.EmployeeDateOfBirthTo < Search_Filter.EmployeeDateOfBirthFrom)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Date to is less than Date from, Gregorian calendars do not work that way");
                }

                //If no dates provided set corresponding values
                if (Search_Filter.EmployeeDateOfBirthTo == default(DateTime) && Search_Filter.EmployeeDateOfBirthFrom == default(DateTime))
                {
                    Search_Filter.EmployeeDateOfBirthTo = DateTime.MaxValue;
                    Search_Filter.EmployeeDateOfBirthFrom = DateTime.MinValue;
                    NoDate = true;
                }

                //If no date to provided set it at maximum
                if (Search_Filter.EmployeeDateOfBirthTo == default(DateTime))
                {
                    Search_Filter.EmployeeDateOfBirthTo = DateTime.MaxValue;
                }

                if (NoDate & Search_Filter.Keyword == "" & Search_Filter.EmployeeJobTitles.Count == 0)
                {
                    NoData = true;
                }

                List<Company> Companies_Match = new List<Company>();
                List<Company_Model> Results = new List<Company_Model>();

                //Temp models
                Company_Model  Temp_Company_Model  = new Company_Model();
                Employee_Model Temp_Employee_Model = new Employee_Model();
                ICollection<Employee> Employees_In_Comp_Match = new List<Employee>();

                if (NoData == false)
                {

                    using (Web_Api_Company_EmployeesEntities db = new Web_Api_Company_EmployeesEntities())
                    {
               
                        //Get all the companies that match the keyword
                
                        Companies_Match = db.Companies
                                         .Where(a => Search_Filter.Keyword != ""? (a.Name == Search_Filter.Keyword || a.Employees.Any(b => b.FirstName == Search_Filter.Keyword || b.LastName == Search_Filter.Keyword)): (a.ID != 0))  //Keyword
                                         .Where(f => NoDate == false ? (f.Employees.Any( g => g.DateOfBirth >= Search_Filter.EmployeeDateOfBirthFrom && g.DateOfBirth <= Search_Filter.EmployeeDateOfBirthTo)): (f.Employees.Count >= 0))                              //Birth dates
                                         .Where(h => Search_Filter.EmployeeJobTitles.Count > 0 ? (h.Employees.Any(i => Search_Filter.EmployeeJobTitles.Contains(i.JobTitle))) : (h.Employees.Count >= 0))                                                                                                                                       //Job titles
                                         .ToList();

                        //Copy retrieved data from db entity to model

                        //First select company
                        foreach (var item in Companies_Match)
                        {
                            Temp_Company_Model = new Company_Model();
                            Temp_Company_Model.Name = item.Name;
                            Temp_Company_Model.EstablishmentYear = item.EstablishmentYear;

                            //then filter the employee list according to the searching criteria
                            Employees_In_Comp_Match = item.Employees
                                                     .Where(j => (Search_Filter.Keyword != item.Name) ? (Search_Filter.Keyword != "") ? (j.FirstName == Search_Filter.Keyword || j.LastName == Search_Filter.Keyword) : (j.ID != 0) : (j.ID != 0 ))
                                                     .Where(k => NoDate == false ? ((k.DateOfBirth >= Search_Filter.EmployeeDateOfBirthFrom && k.DateOfBirth <= Search_Filter.EmployeeDateOfBirthTo)) : (k.ID != 0))
                                                     .Where(l => Search_Filter.EmployeeJobTitles.Count > 0 ? ((Search_Filter.EmployeeJobTitles.Any(r => r == l.JobTitle))) : (l.ID != 0))
                                                     .ToList(); 

                            foreach (var item_2 in Employees_In_Comp_Match)
                            {
                                Temp_Employee_Model = new Employee_Model();
                                Temp_Employee_Model.FirstName = item_2.FirstName;
                                Temp_Employee_Model.LastName = item_2.LastName;
                                Temp_Employee_Model.DateOfBirth = item_2.DateOfBirth;
                                Temp_Employee_Model.JobTitle = (Employee_Model.Jobs)Enum.Parse(typeof(Employee_Model.Jobs), item_2.JobTitle);

                                //ad to list of employess in company_model. 
 
                                Temp_Company_Model.Employees.Add(Temp_Employee_Model);
  
                            }

                            //If no list is returned and the keyword doesn't match the company's name do not return it
                            if (Temp_Company_Model.Employees.Count < 1 && Temp_Company_Model.Name != Search_Filter.Keyword)
                            {
                                Thread.Sleep(10);
                            }
                            else
                            {
                                Results.Add(Temp_Company_Model);
                            }
                            
                        }

                    }

                    //wrap the list in a json object
                    var returnObject = new
                    {
                        Results = Results
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, returnObject, MediaTypeHeaderValue.Parse("application/json"));
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data input is null");
                }  
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data input is null");
            }
        }
        [LoginAuthentication]
        [HttpPut]
        // PUT api/company/update/{id}
        public HttpResponseMessage Update(int id,[FromBody] Company NewCompany)
        {
            if (NewCompany != null)
            {
                if (!((NewCompany.Name == "" || NewCompany.EstablishmentYear == 0) || (NewCompany.Employees.Any(m => m.DateOfBirth == default(DateTime)))))
                {
                    //Variables to check if allowable Job titles have been provided
                    Employee_Filter Jobs_Filter = new Employee_Filter();
                    string Forbidden_jobs = "";

                    Forbidden_jobs = Jobs_Filter.CheckJobTitles(NewCompany.Employees);
                    //If no forbidden jobs found proceed
                    if (Forbidden_jobs == "")
                    { 
                        Company UpdateComp = new Company();
                        using (Web_Api_Company_EmployeesEntities db = new Web_Api_Company_EmployeesEntities())
                        {

                            //Retrieve the desired company to update
                            UpdateComp = (Company)db.Companies.SingleOrDefault(z => z.ID == id);
                            if (UpdateComp != null)
                            {

                                using (Web_Api_Company_EmployeesEntities db_employees = new Web_Api_Company_EmployeesEntities())
                                { 

                                    UpdateComp.Name = NewCompany.Name;
                                    UpdateComp.EstablishmentYear = NewCompany.EstablishmentYear;

                                    //Delete all previous employees
                                    foreach (Employee item in db_employees.Employees)
                                    {
                                        if (item.CompanyToken == id)
                                        {
                                            db_employees.Employees.Remove(item);
                                        }
                                    }
                                    db_employees.SaveChanges();

                                    //Get updated staff
                                    UpdateComp.Employees = NewCompany.Employees;
                                    db.SaveChanges();
                                }

                                return Request.CreateResponse(HttpStatusCode.NoContent);
                            }
                            else
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The company with id: " + id + "doesn't exist any longer");
                            }
                        }
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Some of the jobs provided are not allowed, (company not updated): " + Forbidden_jobs);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Company name and/or establishment year is null or data in employees is incorrect ");
                }
 
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data input is null");
            }
        }
        [LoginAuthentication]
        [HttpDelete]
        // DELETE api/company/delete/{id}
        public HttpResponseMessage Delete(int id)
        {
            using (Web_Api_Company_EmployeesEntities db = new Web_Api_Company_EmployeesEntities())
            {
                Company DeleteModel = new Company() { ID = id };

                try
                {
                    db.Companies.Attach(DeleteModel);
                    db.SaveChanges();
                    db.Companies.Remove(DeleteModel);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "id doesn't correspond to any record in the database, therefore cannot be deleted");
                }   
            }

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}