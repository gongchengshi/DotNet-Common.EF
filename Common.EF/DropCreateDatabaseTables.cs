using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Transactions;

namespace Gongchengshi.EF
{
   public class DropCreateDatabaseTables<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
   {
      public void InitializeDatabase(TContext context)
      {
         using (new TransactionScope(TransactionScopeOption.Suppress))
         {
            if (!context.Database.Exists())
            {
               throw new ApplicationException("No database instance");
            }
         }

         // remove all tables
         context.Database.ExecuteSqlCommand(
            "EXEC sp_MSforeachtable @command1 = \"DROP TABLE ?\"");

         // create all tables
         var dbCreationScript = ((IObjectContextAdapter) context).ObjectContext.CreateDatabaseScript();
         context.Database.ExecuteSqlCommand(dbCreationScript);

         Seed(context);
         context.SaveChanges();
      }

      protected virtual void Seed(TContext context)
      {}
   }
}
