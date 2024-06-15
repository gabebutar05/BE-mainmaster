using API_Dinamis.Data;
using System.Linq.Expressions;

namespace API_Dinamis.Utilities
{
    public class RepositoryUtils
    {
        private readonly DataContext _context;

        public RepositoryUtils(DataContext context)
        {
            _context = context;
        }
        public bool SaveChanges()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public int GetIdPropertyValue(object obj)
        {
            var idProperty = obj.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                return (int)idProperty.GetValue(obj);
            }
            return 0; // or throw an exception, depending on your error handling strategy
        }

        public static Expression<Func<T, object>> GetOrderByExpression<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            return lambda;
        }
    }
}
