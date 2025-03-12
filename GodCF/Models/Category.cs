using System.ComponentModel.DataAnnotations;

namespace GodCF.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Product> Products { get; set; }
    }
}