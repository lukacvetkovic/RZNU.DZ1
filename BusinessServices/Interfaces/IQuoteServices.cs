using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities.Entities;
using DataModel.Database;

namespace BusinessServices.Interfaces
{
    public interface IQuoteServices
    {
        QuoteEntity GetQuoteById(int quoteId);
        IEnumerable<QuoteEntity> GetQuotesForUser(int userId);
        IEnumerable<QuoteEntity> GetAllQuotes();
        int CreateQuote(QuoteEntity quoteEntity);
        bool UpdateQuote(int quoteId, QuoteEntity quoteEntity);
        bool DeleteQuote(int quoteId);
    }
}
