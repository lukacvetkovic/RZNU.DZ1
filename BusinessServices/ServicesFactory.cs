using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessServices.Implementation;
using BusinessServices.Interfaces;

namespace BusinessServices
{
    public class ServicesFactory
    {
        public static IQuoteServices GetQuoteServices()
        {
            return new QuoteServices();
        }
    }
}
