using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookAppMVC.Models
{
    public class Category
    {
        [Key] // It will tell entity framework, this is primary key
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Display Order")] //Custom Field Name to Display
        [Range(1,100)] // Range validation
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
