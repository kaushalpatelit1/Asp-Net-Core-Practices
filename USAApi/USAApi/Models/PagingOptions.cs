using System.ComponentModel.DataAnnotations;

namespace USAApi.Models
{
    public class PagingOptions
    {
        //Adding range validation
        [Range(1, 99999, ErrorMessage = "Offset must be greater than 0")]
        public int? OffSet { get; set; }
        [Range(1, 100, ErrorMessage = "Limit must be between 0 and 100")]
        public int? Limit { get; set; }

        public PagingOptions Replace(PagingOptions newPagingOptions)
        {
            return new PagingOptions
            {
                OffSet = newPagingOptions.OffSet ?? this.OffSet,
                Limit = newPagingOptions.Limit ?? this.Limit,
            };
        }
    }
}
