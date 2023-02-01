using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
namespace TVMAzureAd

{

    public interface IAzureLogin
    {

        bool Login(string ClientId, string Tenant, string Instance);

        string AccessToken { get; }

        string ErrorMessage { get; }

        string UserName { get; }

        string[] Groups { get; }
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class AzureLogin:IAzureLogin
    {
        readonly string[] _scopes = new string[] { "user.read" };
        string[] _groups;
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

        public string[] Groups => _groups;

        [ComVisible(true)]
        public bool Login(string ClientId, string Tenant, string Instance)
        {

            bool result;
            try
            {
                var builder = PublicClientApplicationBuilder.Create(ClientId)
                   .WithAuthority($"{Instance}{Tenant}")
                   .WithDefaultRedirectUri();

                var app = builder.Build();


                var loginTask = app.AcquireTokenInteractive(_scopes)
                           .WithPrompt(Microsoft.Identity.Client.Prompt.SelectAccount)
                           .ExecuteAsync();

                var loginResult = loginTask.GetAwaiter().GetResult();

                _accessToken = loginResult.AccessToken;

                if (loginResult.Account != null)
                {
                    _userName = loginResult.Account.Username;

                }

                GetGroups();

                //_groups = new string[] { "group-A", "group-B" } ;
                    
                result = true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.ToString();
                result = false;

            }

            return result;
        }

        private void GetGroups()
        {
            var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
            {
                requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                return Task.CompletedTask;
            }));

            var groupTask = graphServiceClient.Me.MemberOf.Request().GetAsync();

            var groupResult = groupTask.GetAwaiter().GetResult();

            var groupNames = new List<string>();

            foreach (Group group in groupResult)
            {
                groupNames.Add(group.DisplayName);
            }

            _groups = groupNames.ToArray();
        }
    }
}
