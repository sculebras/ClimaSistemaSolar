using ClimaSistemaSolar.Component.Model;
using SF.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaSistemaSolar.Component.Repositories
{
    public class TipoClimaRepository : Repository<TipoClima>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public TipoClimaRepository(DbContext context)
            : base(context)
        { }
    }
}
