using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelContextBase : DbContext
    {
        public ModelContextBase()
            : this(strNameOrConnectionString: "ModelContext") //<-- Connection String Identifier (Default)
        {
        }

        public ModelContextBase(string strNameOrConnectionString)
            : base(nameOrConnectionString: strNameOrConnectionString) //<-- Connection String Identifier
        {
            //2014-11-13 04:27:22 sculebras: 
            //Disable the use of proxy and Lazy loading
            this.Configuration.ProxyCreationEnabled = false;
        }
        //public DbSet<Connection> Connections { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        
    }//End class

}//End Namespace
