using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities.Entities;
using BusinessServices;
using BusinessServices.Interfaces;
using RZNU.DZ1.ActionFilters;
using RZNU.DZ1.Filters;

namespace RZNU.DZ1.Controllers
{
    [AuthorizationRequired]
    public class QuoteController : ApiController
    {
        private readonly IQuoteServices _quoteServices;
        private readonly ITokenServices _tokenServices;

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize product service instance
        /// </summary>
        public QuoteController()
        {
            _quoteServices = ServicesFactory.GetQuoteServices();
            _tokenServices = ServicesFactory.GetTokenServices();
        }

        #endregion

        // GET api/quote
        [ApiAuthenticationFilter(true)]
        public HttpResponseMessage Get()
        {
            var re = Request;
            var headers = re.Headers;
            string token = null;

            if (headers.Contains("Token"))
            {
                token = headers.GetValues("Token").First();
            }

            if (token != null)
            {

                var userId = _tokenServices.GetUserId(token);
                if (userId != null)
                {
                    var quotes = _quoteServices.GetQuotesForUser(userId.Value);
                    if (quotes != null)
                    {
                        var quotestEntities = quotes as List<QuoteEntity> ?? quotes.ToList();
                        if (quotestEntities.Any())
                            return Request.CreateResponse(HttpStatusCode.OK, quotestEntities);
                    }
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Quotes not found");
        }

        // GET api/quote/5
        public HttpResponseMessage Get(int id)
        {
            var quoteEntity = _quoteServices.GetQuoteById(id);
            if (quoteEntity != null)
                return Request.CreateResponse(HttpStatusCode.OK, quoteEntity);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No quote found for this id");
        }

        // POST api/quote
        [ApiAuthenticationFilter(true)]
        public int Post([FromBody] QuoteEntity quoteEntity)
        {
            //Todo
            return _quoteServices.CreateQuote(quoteEntity);
        }

        // PUT api/quote/5
        [ApiAuthenticationFilter(true)]
        public bool Put(int id, [FromBody]QuoteEntity quoteEntity)
        {
            if (id > 0)
            {
                return _quoteServices.UpdateQuote(id, quoteEntity);
            }
            return false;
        }

        // DELETE api/quote/5
        [ApiAuthenticationFilter(true)]
        public bool Delete(int id)
        {
            if (id > 0)
                return _quoteServices.DeleteQuote(id);
            return false;
        }
    }
}