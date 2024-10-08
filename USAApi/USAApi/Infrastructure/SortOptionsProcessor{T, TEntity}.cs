﻿using LandonApi.Infrastructure;
using System.Reflection;

namespace USAApi.Infrastructure
{
    public class SortOptionsProcessor<T, TEntity>
    {
        private readonly string[] _orderBy;
        public SortOptionsProcessor(string[] orderBy)
        {
            _orderBy = orderBy;
        }
        public IEnumerable<SortTerm> GetAllTerms()
        {
            if(_orderBy == null) yield break;
            foreach(var term in _orderBy)
            {
                if(string.IsNullOrEmpty(term)) continue;
                var tokens = term.Split(' ');

                if(tokens.Length == 0)
                {
                    yield return new SortTerm { Name = term };
                    continue;
                }
                var descending = tokens.Length > 1 && tokens[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

                yield return new SortTerm
                {
                    Name = tokens[0],
                    IsDescending = descending
                };
            }
        }
        public IEnumerable<SortTerm> GetValidTerms()
        {
            var queryTerms = GetAllTerms().ToArray();
            if(!queryTerms.Any()) yield break;
            var declaredTerms = GetTermFromModel();
            foreach(var term in queryTerms)
            {
                var declaredTerm = declaredTerms.SingleOrDefault(x => x.Name.Equals(term.Name, StringComparison.OrdinalIgnoreCase));
                if(declaredTerm == null) continue;

                yield return new SortTerm
                {
                    Name = declaredTerm.Name,
                    EntityName = declaredTerm.EntityName,
                    IsDescending = declaredTerm.IsDescending,
                    DefaultSort = declaredTerm.DefaultSort
                };
            }
        }
        private static IEnumerable<SortTerm> GetTermFromModel()
        {
            return typeof(T).GetTypeInfo() //using the reflection here
                    .DeclaredProperties
                    .Where(p => p.GetCustomAttributes<SortableAttribute>().Any())
                    .Select(p => new SortTerm
                    {
                        Name = p.Name,
                        DefaultSort = p.GetCustomAttribute<SortableAttribute>().DefaultSort
                    });
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            var terms = GetValidTerms().ToArray();

            if(!terms.Any())
            {
                terms = GetTermFromModel().Where(t => t.DefaultSort).ToArray();
            }

            var modifiedQuery = query;
            var useThenBy = false;
            foreach(var term in terms)
            {
                var propertyInfo = ExpressionHelper.GetPropertyInfo<TEntity>(term.EntityName ?? term.Name);
                var obj = ExpressionHelper.Parameter<TEntity>();

                //Build the LINQ Expression Backwards:
                // query = query.OrderBy(x => x.Property);

                // x=> x.Property
                var key = ExpressionHelper.GetPropertyExpression(obj, propertyInfo);
                var keySelector = ExpressionHelper.GetLambda(typeof(TEntity), propertyInfo.PropertyType, obj, key);

                // query.OrderBy/ThenBy[Descending](x => x.Property)
                modifiedQuery = ExpressionHelper.CallOrderByOrThenBy(modifiedQuery, useThenBy, term.IsDescending, propertyInfo.PropertyType, keySelector);
                useThenBy = true;
            }
            return modifiedQuery;
        }
    }
}
