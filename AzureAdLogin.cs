using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
namespace TVMAzureAd

{

    public interface IAzureLogin
    {

        String Login(string ClientId, string Tenant, string Instance);
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class AzureLogin:IAzureLogin
    {
        string[] scopes = new string[] { "user.read" };

        public AzureLogin() {
 
        }


        [ComVisible(true)]
        public String Login(string ClientId, string Tenant, string Instance) {

            String result = "failed";

            try
            {
                var builder = PublicClientApplicationBuilder.Create(ClientId)
                   .WithAuthority($"{Instance}{Tenant}")
                   .WithDefaultRedirectUri();

                var app = builder.Build();


                var task = app.AcquireTokenInteractive(scopes)
                           .WithPrompt(Prompt.SelectAccount)
                           .ExecuteAsync();

                var taskResult = task.GetAwaiter().GetResult();

                result = taskResult.AccessToken;

            }
            catch (Exception ex) {
                result = "ex:" + ex.Message;
            }
            return result;

        }
    }
}
