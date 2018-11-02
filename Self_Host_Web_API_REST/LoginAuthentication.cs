using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Self_Host_Web_API_REST
{
    class LoginAuthentication : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //Check if credentials are provided
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                //base 64 encoded token
                string authenticationtoken = actionContext.Request.Headers.Authorization.Parameter;

                //Split it (the decoded string is User:Password)
                string[] user_password;
                string _User = "";
                string _Password = "";

                //try to decode the token
                try
                {
                    string decodedauthenticationtoken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationtoken));
                    user_password = decodedauthenticationtoken.Split(':');
                    _User = user_password[0];
                    _Password = user_password[1];
                }
                catch (Exception)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
                
                //check if the provided authentication is correct
                Login Authentication_Checker = new Login();

                if (Authentication_Checker.Authenticate_User(_User,_Password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(_User), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

            }
        }
    }
}
