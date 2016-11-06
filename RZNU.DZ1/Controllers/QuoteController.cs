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
using System.Threading;

namespace RZNU.DZ1.Controllers
{
    [ApiAuthenticationFilter(true)]
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
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            if (basicAuthenticationIdentity != null)
            {
                var userId = basicAuthenticationIdentity.Id;
                var quotes = _quoteServices.GetQuotesForUser(userId);
                if (quotes != null)
                {
                    var quotestEntities = quotes as List<QuoteEntity> ?? quotes.ToList();
                    if (quotestEntities.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, quotestEntities);
                    }
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Quotes not found");
        }


        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            var quoteEntity = _quoteServices.GetAllQuotes();
            if (quoteEntity != null)
                return Request.CreateResponse(HttpStatusCode.OK, quoteEntity);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No quote found");
        }
        // GET api/quote/5
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var quoteEntity = _quoteServices.GetQuoteById(id);
            if (quoteEntity != null)
                return Request.CreateResponse(HttpStatusCode.OK, quoteEntity);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No quote found for this id");
        }

        // POST api/quote
        [HttpPost]
        public HttpResponseMessage Create([FromBody] QuoteEntity quoteEntity)
        {
            var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            if (basicAuthenticationIdentity != null)
            {
                var userId = basicAuthenticationIdentity.Id;


                quoteEntity.UserId = userId;

                var id = _quoteServices.CreateQuote(quoteEntity);
                return Request.CreateResponse(HttpStatusCode.OK, id);
            }


            return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Not athorized");
        }

        // PUT api/quote/5
        [HttpPut]
        public HttpResponseMessage Update(int id, [FromBody] QuoteEntity quoteEntity)
        {
            var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            if (basicAuthenticationIdentity != null)
            {
                var userId = basicAuthenticationIdentity.Id;
                if (id > 0)
                {
                    if (_quoteServices.GetQuoteById(id).UserId == userId)
                    {
                        var succ = _quoteServices.UpdateQuote(id, quoteEntity);
                        return Request.CreateResponse(HttpStatusCode.OK, succ);
                    }
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Not athorized");
        }

        // DELETE api/quote/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            if (basicAuthenticationIdentity != null)
            {
                var userId = basicAuthenticationIdentity.Id;
                if (id > 0)
                {
                    if (_quoteServices.GetQuoteById(id).UserId == userId)
                    {
                        var succ = _quoteServices.DeleteQuote(id);
                        return Request.CreateResponse(HttpStatusCode.OK, succ);
                    }
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Not athorized");
        }
    }
}