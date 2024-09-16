using System.Linq.Expressions;

namespace USAApi.Infrastructure
{
    public interface ISearchExpressionProvider
    {
        IEnumerable<string> GetOperators();
        ConstantExpression GetValue(string input);

        Expression GetComparison(
            MemberExpression left,
            string op,
            ConstantExpression right);
    }
}
