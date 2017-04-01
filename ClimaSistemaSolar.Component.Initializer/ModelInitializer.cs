﻿using ClimaSistemaSolar.Component.DAL;
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
    public class ModelInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ModelContext> //DropCreateDatabaseAlways<ModelContext> //DropCreateDatabaseIfModelChanges<ModelContext>
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
            //Adds Unique constraint to Configuration Table
            if (!context.Database.Exists())
            {
                context.Database.Create();
                //context.Database.ExecuteSqlCommand("alter table Configuration add constraint IX_UniqueName unique (Name)");
                //context.Database.ExecuteSqlCommand("alter table [Connection] add constraint IX_UniqueIdentifier unique ([Identifier])");
               
//                //Insert SQL Scripts:
//                context.Database.ExecuteSqlCommand(@"
//                USE MultiTenancyManager
//                --[Configuration]
//                INSERT INTO [MultiTenancyManager].[dbo].[Configuration]([Name],[Value],[Description])VALUES('TokenDecryptingKey','Citroen9','Key Used to decrypt URL tokens. (Alphanumeric).') 
//                INSERT INTO [MultiTenancyManager].[dbo].[Configuration]([Name],[Value],[Description])VALUES('UseTokenExpiration','1','Defines if Token Expiration is used or not. -0: Not Uses Token Expiration. -1: Uses Token Expiration.') 
//                INSERT INTO [MultiTenancyManager].[dbo].[Configuration]([Name],[Value],[Description])VALUES('TokenExpiringMinutes','30','Expiration time in minutes to admit token if UseTokenExpiration is active. (Integer).') 
//                --[Connection]
//                INSERT INTO [MultiTenancyManager].[dbo].[Connection]([Description],[Identifier],[Server],[DataBase],[User],[Password],[IntegratedSecurity]) VALUES('Local 1','009660H','localhost','SidAuto_Testing','sa','Sofrecom14',0)
//                INSERT INTO [MultiTenancyManager].[dbo].[Connection]([Description],[Identifier],[Server],[DataBase],[User],[Password],[IntegratedSecurity]) VALUES('Local 2','123456A','localhost','SidAuto_Testing2','sa','Sofrecom14',0)
//                ");

            }

            //Database.SetInitializer(new ModelInitializer());
        }
    }//End class
}//End Namespace
