using SF.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Core
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private ModelContextBase context;

        protected ModelContextBase Context
        {
            get { return this.context; }
            set { this.context = value; }
        }

        //private IDataMockRepository dataMockRepository;
        
        public UnitOfWork()
        {
            this.context = new ModelContextBase();
        }

        public UnitOfWork(string connectionStringName)
        {
            this.context = new ModelContextBase(connectionStringName);
        }

        public IRepository<TEntity> RepositoryFor<TEntity>()
            where TEntity : EntityBase
        {
            return new Repository<TEntity>(context);
        }

        /// <summary>
        /// Commit changes in Data base.
        /// </summary>
        public void Commit()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

       
        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            this.Context.Dispose();
        }


    }//End class
}//End Namespace