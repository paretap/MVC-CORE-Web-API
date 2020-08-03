using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.DirectoryServices.Protocols;
using System.Net;

namespace WmDevWebAPI.Helpers
{
    public class ADHelper
    {
        const string LDAP_PATH = "EX://ldap.example.com:5555";
        const string LDAP_DOMAIN = "ldap.example.com:5555";
        const string SERVICE_ACCT_USER = "Service_Account_User";
        const string SERVICE_ACCT_PSWD = "Service_Account_password";

        public static AuthenticateResult DoAuthentication(string username, string password)
        {
            using (var context = new PrincipalContext(ContextType.Domain, LDAP_DOMAIN, SERVICE_ACCT_USER, SERVICE_ACCT_PSWD))
            {
                if (context.ValidateCredentials(username, password))
                {
                    using var directoryEntry = new DirectoryEntry(LDAP_PATH);
                    using var directorySearcher = new DirectorySearcher(directoryEntry);
                    // another logic to verify the user has correct permission(s)

                    // To do user is authenticated and authorized
                    var identities = new List<ClaimsIdentity> { new ClaimsIdentity("custom auth type") };
                    var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), Options.DefaultName);
                    return AuthenticateResult.Success(ticket);
                    //  return Task.FromResult(AuthenticateResult.Success(ticket));
                }
            }

            // User is not authenticated.
            return AuthenticateResult.Fail("Invalid auth key.");
            //return Task.FromResult(AuthenticateResult.Fail("Invalid auth key."));

        }


        // Need to check on Linux OS
        public static bool IsAuthenticated(string username, string password)
        {
            bool authenticated = false;
            
            using (LdapConnection connection = new LdapConnection(LDAP_PATH))
            {
                try
                {
                    username += LDAP_DOMAIN;
                    connection.AuthType = AuthType.Basic;
                    connection.SessionOptions.ProtocolVersion = 3;
                    var credential = new NetworkCredential(username, password);
                    connection.Bind(credential);
                    authenticated = true;
                    return authenticated;
                }
                catch (LdapException)
                {
                    return authenticated;
                }
                finally
                {
                    connection.Dispose();
                }
            }
        }
    }
}
