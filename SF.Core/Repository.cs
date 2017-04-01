using SF.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SF.Core
{
    /// <summary>
    /// Base Class for repositories.
    /// </summary>
    /// <typeparam name="TEntity">Type of Entity derived from EntityBase</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : EntityBase
    {
        /// <summary>
        /// EF Db Set of the entity
        /// </summary>
        protected DbSet<TEntity> entitySet;
        /// <summary>
        /// Contex where the DB set belongs to.
        /// </summary>
        protected DbContext context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public Repository(DbContext context)
        {
            this.context = context;
            this.entitySet = context.Set<TEntity>();
        }

        /// <summary>
        /// Creates an Entity in the entity set.
        /// </summary>
        /// <param name="entity">Entity with its data.</param>
        public virtual void Create(EntityBase oEntityBase)
        {
            entitySet.Add(oEntityBase as TEntity);
        }

        /// <summary>
        /// Uptades an Entity in the entity set.
        /// </summary>
        /// <param name="entity">Entity with its data.</param>
        public virtual void Update(EntityBase oEntityBase)
        {
            //2014-11-13 01:19:43 sculebras: Optimization
            var ent = entitySet.Local.FirstOrDefault(e => e.Id == oEntityBase.Id);

            if (ent != null && !ent.Equals(oEntityBase))
            {
                context.Entry(ent).CurrentValues.SetValues(oEntityBase);
            }
            else
            {
                context.Entry(oEntityBase).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Retrieves (Get) a Entity according to its unique Id.
        /// </summary>
        /// <param name="id">Unique Id of the entity.</param>
        /// <returns></returns>
        public virtual TEntity Retrieve(int id)
        {
            return entitySet.FirstOrDefault(e => e.Id == id);
        }
        
        /// <summary>
        /// Delete the Entity from entity set.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(EntityBase entity)
        {
            entitySet.Remove(entity as TEntity);
        }

        /// <summary>
        /// Retrieves a collection of all the Entities of the entered Type.
        /// (Query is made before executing it)
        /// </summary>
        /// <param name="includes">Names of the tables to Include in Query.</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> RetrieveQueryable(params string[] includes)
        {
            ////entitySet.Include("sdasdasdSA").AsQueryable().Where(k => EntityFunctions.);
            //return entitySet.AsQueryable();
            var es = entitySet.AsQueryable();
            foreach (var include in includes)
            {
                es = es.Include(include);
            }
            return es; //.ToList()
        }

        /// <summary>
        /// Retrieves a collection of all the Entities of the entered Type.
        /// (Query is made after retreiving entities.)
        /// </summary>
        /// <param name="includes">Names of the tables to Include in Query.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> RetrieveEnumerable(params string[] includes)
        {
            return this.RetrieveQueryable(includes).ToList(); //.ToList()
        }

        /// <summary>
        /// Validates Entity against the Entity data annotations.
        /// Validation messages are returned in listValidationResults parameter.
        /// </summary>
        /// <param name="oEntityBase">Entity to be validated.</param>
        /// <param name="listValidationResults">list of validation results where validation error will be added. If null it will create a new list.</param>
        /// <returns>
        /// true: Validation OK.
        /// false: Validation No OK.
        /// </returns>
        public virtual bool Validate(EntityBase oEntityBase,
                                     ref List<ValidationResult> listValidationResults)
        {
            // Use the ValidationContext to validate the Entyty model against the Entity data annotations
            ValidationContext oValidationContext = new ValidationContext(oEntityBase, serviceProvider: null, items: null);
            if (listValidationResults == null)
            {
                listValidationResults = new List<ValidationResult>();
            }
            return Validator.TryValidateObject(oEntityBase, oValidationContext, listValidationResults, true);
        }
    }//End class
}//End Namespace
