using System.ComponentModel.DataAnnotations;
using USAApi.Infrastructure;

namespace USAApi.Models
{
    public class SortOptions<T, TEntity> : IValidatableObject
    {
        public string[] OrderBy { get; set; }
        //ASP.NET Core will call this validate incoming parameters.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var processor = new SortOptionsProcessor<T, TEntity>(OrderBy);
            var validTerms = processor.GetValidTerms().Select(x => x.Name);
            var invalidTerms = processor.GetAllTerms().Select(x => x.Name).Except(validTerms, StringComparer.OrdinalIgnoreCase);
            foreach(var inTerm in invalidTerms)
            {
                yield return new ValidationResult($"Invalid sort term {inTerm}.", new[]  { nameof(OrderBy) });
            }
        }
        //The service code will call this to apply these sort options to database query
        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            var processor = new SortOptionsProcessor<T, TEntity>(OrderBy);
            return processor.Apply(query);
        }
    }
}
