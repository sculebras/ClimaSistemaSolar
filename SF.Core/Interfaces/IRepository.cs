﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Core.Interfaces
{
    //2014-08-29 12:47:35 sculebras: 
    /// <summary>
    /// Intereface for Domain Repositories.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<out TEntity>
        where TEntity : EntityBase
    {
        void Create(EntityBase entity);

        void Update(EntityBase entity);

        TEntity Retrieve(int id);

        void Delete(EntityBase entity);

       
        /// <summary>
        /// Retrieves a collection of all the Entities of the entered Type.
        /// (Query is made before executing it)
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> RetrieveQueryable(params string[] includes);
        

        /// <summary>
        /// Retrieves a collection of all the Entities of the entered Type.
        /// (Query is made after retreiving entities.)
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> RetrieveEnumerable(params string[] includes);
    }//End interface
}//End Namespace				
