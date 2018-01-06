using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace HETSAPI
{
    /// <summary>
    /// Db Context Transaction Wrapper
    /// </summary>
    public class DbContextTransactionWrapper : IDbContextTransaction, IDisposable
    {
        private readonly IDbContextTransaction _transaction;

        internal DbContextTransactionWrapper(IDbContextTransaction transaction, bool existingTransaction)
        {
            _transaction = transaction;
            ExistingTransaction = existingTransaction;
        }

        internal bool ExistingTransaction { get; set; }

        /// <summary>
        /// Database TRansaction Id
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Commits all changes made to the database in the current transaction.
        /// </summary>
        public void Commit()
        {
            // Don't commit someone else's transaction
            if (!ExistingTransaction)
            {
                _transaction.Commit();
            }
        }

        /// <summary>
        /// Discards all changes made to the database in the current transaction.
        /// </summary>
        public void Rollback()
        {
            // Don't rollback someone else's transaction
            if (!ExistingTransaction)
            {
                _transaction.Rollback();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting 
        /// unmanaged resources.
        /// </summary>        
        private bool _disposed;

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing && !ExistingTransaction) _transaction.Dispose();

            _disposed = true;
        }

        ~DbContextTransactionWrapper()
        {
            Dispose(false);
        }
    }
}
