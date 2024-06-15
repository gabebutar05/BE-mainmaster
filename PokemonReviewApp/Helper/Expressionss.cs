using API_Dinamis.Models;
using System.Linq.Expressions;

namespace API_Dinamis.Helper
{
    public class Expressionss
    {
        public static Expression<Func<Branch, object>> GetOrderByExpression(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(Branch), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<Branch, object>>(Expression.Convert(property, typeof(object)), parameter);

            return lambda;
        }
    }
}
