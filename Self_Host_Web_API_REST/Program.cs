using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

//Self Host references
using Microsoft.Owin.Hosting;


namespace Self_Host_Web_API_REST
{
    class Program
    {
        //Main program
        static void Main(string[] args)
        {
            //Variables
            string sLocalHostPort = "12345"; //Holds the port number to which the Web Api should listen
            string sHostRoute = "http://localhost:"; //Holds the whole route
            ConsoleKeyInfo cSessionCharacter; //Session key
            bool bPortIsValid = false; //Checks if the provided port number is valid (contains only characters from 0 to 9)
            bool bSessionBit = true; //Session bit, if false close the session
            

            //Start the API and listen to the chosen port
            while (bSessionBit)
            {
                //Specify the port number
                Console.WriteLine("Please specify the port number... ");
                Console.Write("Port: ");
                sLocalHostPort = Console.ReadLine();

                bPortIsValid = CheckPortNumber(sLocalHostPort);

                //Check if the provided port number is correct
                while (bPortIsValid == false)
                {
                    Console.WriteLine("Provided port number: " + sLocalHostPort + " , is wrong.");
                    Console.WriteLine("It is not a number, is greater than 65534 or less than 1");
                    Console.WriteLine("Please provide a correct port number (Must be a natural number > 0)");
                    Console.WriteLine("Examples: 12345 ; 589 ; 65534 ; 21");
                    Console.Write("Port: ");

                    sLocalHostPort = Console.ReadLine();
                    bPortIsValid = CheckPortNumber(sLocalHostPort);
                }

                //Update the route
                sHostRoute = sHostRoute + sLocalHostPort;

                //Some ports are denied
                try
                {

                    using (WebApp.Start<ServerStart>(sHostRoute)) 
                    {
                        Console.WriteLine();
                        Console.WriteLine("Web Server is Online... ");
                        Console.WriteLine("Route: " + sHostRoute);
                        Console.WriteLine("Press 'n' to close the server and choose a new port to listen.");
                        Console.WriteLine("Press any other key to close the server.");

                        cSessionCharacter = Console.ReadKey();

                        if (cSessionCharacter.KeyChar == 'n' || cSessionCharacter.KeyChar == 'N')
                        {
                            bSessionBit = true;
                            Console.WriteLine();
                            //Reset route config
                            sHostRoute = "http://localhost:";
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Session terminated. Closing... ");
                            Thread.Sleep(2000);
                            bSessionBit = false;
                        }
                    }
                }
                catch (System.Reflection.TargetInvocationException e)
                {
                    HttpListenerException k = (HttpListenerException)e.InnerException;
                    Console.WriteLine("Error Code: " + k.NativeErrorCode);
                    Console.WriteLine("Error Message: " + k.Message);
                    Console.WriteLine("Provided port number: " + sLocalHostPort + " , is not avaible.");
                    sHostRoute = "http://localhost:";
                    Console.WriteLine();
                }
                catch (SystemException e)
                {  
                    Console.WriteLine("Unexpected error has ocurred");
                    sHostRoute = "http://localhost:";
                    Console.WriteLine();
                }
            }
        }

        //Methods
        static private bool CheckPortNumber (string sPort)
        {
            //Local variables
            bool bIsNumber = false;     //Check if it is a number
            bool bResult = false;
            string _sPort = sPort;

            bIsNumber = Regex.IsMatch(_sPort, "^[0-9]{1,5}$"); //Only port numbers up to 5 digits

            //Check if it is a number and if so if it is between 1 and  65535 (port scope)
            bResult = 
                bIsNumber == true ? 
                (Int32.Parse(_sPort) > 0 && Int32.Parse(_sPort) < 65535) ? true : false
                : false;

            //Return value
            return bResult;
        }
    }
}
