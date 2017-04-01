using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Core.Interfaces
{
    /// <summary>
    /// Interface for Unit Of Work Pattern.
    /// </summary>
    public interface IUnitOfWork
    {
        void Commit();

        IRepository<TEntity> RepositoryFor<TEntity>()
            where TEntity : EntityBase;

        //IDataMockRepository DataMockRepository { get; }
    }//End interface
}//End Namespace
