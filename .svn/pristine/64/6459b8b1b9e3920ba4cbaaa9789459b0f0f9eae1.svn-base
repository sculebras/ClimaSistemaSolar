using SF.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SF.Core
{
    //2014-11-13 03:00:58 sculebras: 
    /// <summary>
    /// Extensions for Interface IRepositiry.
    /// </summary>
    public static class RepositoryExtensions
    {
       /// <summary>
       /// Retrieves a collection of all the Entities of the entered Type.
       /// (Query is made after retreiving entities.)
       /// </summary>
       /// <typeparam name="TEntity">Type of Entity</typeparam>
       /// <param name="oIRepository">Repository</param>
       /// <param name="includes">Includes Lambda expressions list. To Include JOINS in SQL Query.
       ///  <example>
       ///      <code>x => x.Invoice.Item, x => x.Invoice.Totals</code> //Must be Navegation properties.
        ///      <code>x => x.Invoice.Item.ItemComplexProperty</code>
       ///  </example>
       /// </param>
       /// <returns></returns>
        public static IEnumerable<TEntity> RetrieveEnumerable<TEntity>(this IRepository<TEntity> oIRepository,
                                                                       params Expression<Func<TEntity, object>>[] includes)
             where TEntity : EntityBase
        {
            var includeNames = new List<string>();

            foreach (var include in includes)
            {
                var exp = (include.Body as MemberExpression);

                var str = GetPath(exp); //or exp.GetPath();

                //while (exp != null)
                //{
                //    if (!string.IsNullOrEmpty(str))
                //    {
                //        str = "." + str;
                //    }
                //    str = exp.Member.Name + str;
                //    exp = exp.Expression as MemberExpression;
                //}

                includeNames.Add(str);
            }

            return oIRepository.RetrieveEnumerable(includeNames.ToArray());
        }

        
       /// <summary>
       /// Converts a lambda expression into string.
       /// </summary>
       /// <param name="expression">Lambda Expresion</param>
       /// <returns></returns>
        public static string GetPath(this MemberExpression expression)
        {
            var exp = expression.Expression as MemberExpression;

            if (exp != null)
            {
                return string.Join(".", new string[] { exp.GetPath(), expression.Member.Name });
            }

            return expression.Member.Name;
        }

    }//End class
}//End Namespace				
				
