using System;
using BusinessEntities.Entities;
using BusinessServices.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessServices.Tests
{
    [TestClass]
    public class QuoteTests
    {
        [TestMethod]
        public void CreateGetDelete()
        {
            IQuoteServices qs = ServicesFactory.GetQuoteServices();
            IUserServices us = ServicesFactory.GetUserServices();

            var user = us.GetUserByEmail("test@test.hr");
            if (user == null)
            {
                us.Register("test@test.hr", "123456", "Test User");
                user = us.GetUserByEmail("test@test.hr");
            } 
            
            //Create quote
            var quote= new QuoteEntity() {Text = "Test",UserId = user.Id};
            var quoteId = qs.CreateQuote(quote);

            Assert.AreNotEqual(quoteId,0);

            //Get quote
            var newQuote = qs.GetQuoteById(quoteId);

            Assert.IsNotNull(newQuote);

            Assert.AreEqual(newQuote.Text,quote.Text);

            Assert.AreEqual(newQuote.UserId, quote.UserId);

            //Update quote
            newQuote.Text = "Test1";

            qs.UpdateQuote(newQuote.Id, newQuote);

            newQuote = qs.GetQuoteById(quoteId);

            Assert.IsNotNull(newQuote);

            Assert.AreEqual(newQuote.Text, "Test1");

            Assert.AreEqual(newQuote.UserId, quote.UserId);

            //Delete quote
            qs.DeleteQuote(newQuote.Id);

            newQuote = qs.GetQuoteById(quoteId);

            Assert.IsNull(newQuote);


        }
    }
}
