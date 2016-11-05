using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using BusinessEntities.Entities;
using BusinessServices.Interfaces;
using DataModel.Database;
using DataModel.UnitOfWork;

namespace BusinessServices.Implementation
{
    public class QuoteServices : IQuoteServices
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public QuoteServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public QuoteEntity GetQuoteById(int quoteId)
        {
            var quote = _unitOfWork.QuoteRepository.GetByID(quoteId);
            if (quote != null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<Quote, QuoteEntity>());

                QuoteEntity dto = Mapper.Map<QuoteEntity>(quote);

                return dto;
            }
            return null;
        }

        public IEnumerable<QuoteEntity> GetQuotesForUser(int userId)
        {
            var quotes = _unitOfWork.QuoteRepository.GetMany(p=>p.UserId == userId).ToList();
            if (quotes.Any())
            {
                Mapper.Initialize(cfg => cfg.CreateMap<Quote, QuoteEntity>());

                List<QuoteEntity> dtoList = Mapper.Map<List<QuoteEntity>>(quotes);

                return dtoList;
            }
            return null;
        }

        public IEnumerable<QuoteEntity> GetAllQuotes()
        {
            var quotes = _unitOfWork.QuoteRepository.GetAll().ToList();
            if (quotes.Any())
            {
                Mapper.Initialize(cfg => cfg.CreateMap<Quote, QuoteEntity>());

                List<QuoteEntity> dtoList = Mapper.Map<List<QuoteEntity>>(quotes);

                return dtoList;
            }
            return null;
        }

        public int CreateQuote(QuoteEntity quoteEntity)
        {
            using (var scope = new TransactionScope())
            {
                var quote = new Quote()
                {
                    UserId = quoteEntity.UserId,
                    Text = quoteEntity.Text
                };
                _unitOfWork.QuoteRepository.Insert(quote);
                _unitOfWork.Save();
                scope.Complete();
                return quote.Id;
            }
        }

        public bool UpdateQuote(int quoteId, QuoteEntity quoteEntity)
        {
            var success = false;
            if (quoteEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var quote = _unitOfWork.QuoteRepository.GetByID(quoteId);
                    if (quote != null)
                    {
                        quote.Text = quoteEntity.Text;
                        _unitOfWork.QuoteRepository.Update(quote);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public bool DeleteQuote(int quoteId)
        {
            var success = false;
            if (quoteId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var quote = _unitOfWork.QuoteRepository.GetByID(quoteId);
                    if (quote != null)
                    {

                        _unitOfWork.QuoteRepository.Delete(quote);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
    }
}
