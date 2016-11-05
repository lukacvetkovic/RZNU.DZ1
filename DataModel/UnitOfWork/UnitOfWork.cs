using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Database;
using DataModel.GenericRepository;

namespace DataModel.UnitOfWork
{
    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        #region Private member variables...

        private RZNUDb _context = null;
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<Token> _tokenRepository;
        private IGenericRepository<Quote> _quoteRepository;
        #endregion

        public UnitOfWork()
        {
            _context = new RZNUDb();
        }

        #region Public Repository Creation properties...

        public IGenericRepository<User> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                    this._userRepository = new GenericRepositorySQL<User>(_context);
                return _userRepository;
            }
        }

        public IGenericRepository<Token> TokenRepository
        {
            get
            {
                if (this._tokenRepository == null)
                    this._tokenRepository = new GenericRepositorySQL<Token>(_context);
                return _tokenRepository;
            }
        }


        public IGenericRepository<Quote> QuoteRepository
        {
            get
            {
                if (this._quoteRepository == null)
                    this._quoteRepository = new GenericRepositorySQL<Quote>(_context);
                return _quoteRepository;
            }
        }
        #endregion

        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
