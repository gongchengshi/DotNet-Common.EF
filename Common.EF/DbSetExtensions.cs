using System;
using System.Data.Entity;
using System.Linq;
using Gongchengshi.Collections.Generic;

namespace Gongchengshi.EF
{
    public static class DbSetExtensions
    {
       static public T FirstOrAdd<T>(this DbSet<T> @this, Func<T, bool> predicate) where T : class, new()
       {
          return @this.FirstOr(predicate,
                               () =>
                                  {
                                     var x = new T();
                                     @this.Add(x);
                                     return x;
                                  });
       }

       static public T FirstOrAdd<T>(this DbSet<T> @this, Func<T, bool> predicate, Func<T> initializer) where T : class
       {
          return @this.FirstOr(predicate,
                               () =>
                                  {
                                     var x = initializer();
                                     @this.Add(x);
                                     return x;
                                  });
       }

       static public T FirstOrAdd<T>(this DbSet<T> @this, Func<T, bool> predicate, T item) where T : class
       {
          return @this.FirstOr(predicate,
                               () =>
                                  {
                                     @this.Add(item);
                                     return item;
                                  });
       }

       static public void IfNotContainsAdd<T>(this DbSet<T> @this, Func<T, bool> predicate, Func<T> initializer) where T : class
       {
          if (!@this.Any(predicate))
          {
             @this.Add(initializer());
          }
       }
    }
}
