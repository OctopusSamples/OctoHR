using System;
using System.Linq;

namespace OctopusSamples.OctoHR.PublicWebApp.Data
{
    public interface IUserAuthenticationService
    {
        ApplicationUser AuthenticateUser(string clientCode, string username, string password);
    }

    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IClientConfigDataAccess _clientConfigDataAccess;

        public UserAuthenticationService(IClientConfigDataAccess clientConfigDataAccess)
        {
            _clientConfigDataAccess = clientConfigDataAccess;
        }

        public ApplicationUser AuthenticateUser(string clientCode, string username, string password)
        {
            try
            {
                var user = _clientConfigDataAccess.CheckClientUser(clientCode, username, password);
                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}
