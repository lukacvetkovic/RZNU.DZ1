using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using BusinessServices;
using BusinessServices.Interfaces;
using RZNU.DZ1.Filters;

namespace RZNU.DZ1.Controllers
{

    public class AuthenticateController : ApiController
    {
        public class UserLogin
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        public class UserRegister
        {
            public string email { get; set; }
            public string password { get; set; }
            public string name { get; set; }
        }

        #region Private variable.

        private readonly ITokenServices _tokenServices;
        private readonly IUserServices _userServices;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize product service instance
        /// </summary>
        public AuthenticateController()
        {
            _tokenServices = ServicesFactory.GetTokenServices();
            _userServices = ServicesFactory.GetUserServices();
        }

        #endregion

        /// <summary>
        /// Authenticates user and returns token with expiry.
        /// </summary>
        /// <returns></returns>
        [ApiAuthenticationFilter]
        public HttpResponseMessage Authenticate()
        {
            if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    var userId = basicAuthenticationIdentity.Id;
                    return GetAuthToken(userId);
                }
            }
            return null;
        }

        /// <summary>
        /// Returns auth token for the validated user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private HttpResponseMessage GetAuthToken(int userId)
        {
            var token = _tokenServices.GenerateToken(userId);
            var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
            response.Headers.Add("Token", token.AuthToken);
            response.Headers.Add("TokenExpiry", Convert.ToString(86400));
            response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
            return response;
        }

        public HttpResponseMessage Register(UserRegister userRegister)
        {
            var succ = _userServices.Register(userRegister.email, userRegister.password, userRegister.name);
            return Request.CreateResponse(HttpStatusCode.OK, succ);
        }

    }
}