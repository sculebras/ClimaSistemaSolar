using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ClimaSistemaSolar.Component.Model;
using SF.Core;

namespace ClimaSistemaSolar.Component.DAL.Context
{
    public class ModelContext : ModelContextBase
    {
        public ModelContext(): base() 
        {
        }
               
        public DbSet<TipoClima> TipoClimas { get; set; }
        public DbSet<Clima> Climas { get; set; }

    }//End class

}//End Namespace				
				
