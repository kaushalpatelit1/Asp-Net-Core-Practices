using System.Linq.Expressions;

namespace USAApi.Infrastructure
{
    public class DefaultSearchExpressionProvider : ISearchExpressionProvider
    {
        protected const string EqualsOperator = "eq";

        public virtual IEnumerable<string> GetOperators()
        {
            yield return EqualsOperator;
        }
        public virtual Expression GetComparison(
            MemberExpression left,
            string op,
            ConstantExpression right)
        {
            if(!op.Equals("eq", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"Invalid operator '{op}'.");

            return Expression.Equal(left, right);
        }

        public virtual ConstantExpression GetValue(string input)
            => Expression.Constant(input);
    }
}
