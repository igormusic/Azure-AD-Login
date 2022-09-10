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

        bool Login(string ClientId, string Tenant, string Instance);

        string AccessToken { get; }

        string ErrorMessage { get; }

        string UserName { get; }
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class AzureLogin:IAzureLogin
    {
        string[] scopes = new string[] { "user.read" };
        private string _accessToken;
        private string _errorMessage;
        private string _userName;

        public AzureLogin() {
 
        }

        [ComVisible(true)]
        public string AccessToken => _accessToken;
        [ComVisible(true)]
        public string ErrorMessage => _errorMessage;
        [ComVisible(true)]
        public string UserName => _userName;

        [ComVisible(true)]
        bool IAzureLogin.Login(string ClientId, string Tenant, string Instance)
        {

            bool result;
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

                _accessToken = taskResult.AccessToken;

                if (taskResult.Account != null)
                {
                    _userName = taskResult.Account.Username;

                }

                result = true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                result = false;

            }

            return result;
        }
    }
}
