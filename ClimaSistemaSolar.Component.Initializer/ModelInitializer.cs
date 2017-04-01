using ClimaSistemaSolar.Component.DAL;
using ClimaSistemaSolar.Component.DAL.Context;
using ClimaSistemaSolar.Component.Model;
using SF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace ClimaSistemaSolar.Component.Initializer
{
    /// <summary>
    /// Entity Framework Database Initializer.
    /// </summary>
    public class ModelInitializer : System.Data.Entity.DropCreateDatabaseAlways<ModelContext> //DropCreateDatabaseAlways<ModelContext> //DropCreateDatabaseIfModelChanges<ModelContext>
    {
        //NOT BEING EXECUTED (BUG?)
        protected override void Seed(ModelContext context)
        {
            base.Seed(context);
           
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
       
        public override void InitializeDatabase(ModelContext context)
        {
            //base.InitializeDatabase(context);

            if (context.Database.Exists() && !context.Database.CompatibleWithModel(false))
                context.Database.Delete();
            
            if (!context.Database.Exists())
            {
                context.Database.Create();
                
                //Insert SQL Scripts:
                context.Database.ExecuteSqlCommand(@"
                USE ClimaSistemaSolar
                --[TipoClima]
                INSERT INTO [ClimaSistemaSolar].[dbo].[TipoClima]([Descripcion])VALUES('Sequía') 
                INSERT INTO [ClimaSistemaSolar].[dbo].[TipoClima]([Descripcion])VALUES('Lluvia') 
                INSERT INTO [ClimaSistemaSolar].[dbo].[TipoClima]([Descripcion])VALUES('Lluvia Pico Máximo') 
                INSERT INTO [ClimaSistemaSolar].[dbo].[TipoClima]([Descripcion])VALUES('Óptimo') 
                ");

            }

            //Database.SetInitializer(new ModelInitializer());
        }
    }//End class
}//End Namespace
