

namespace CleanArchitecture.DataAccess.Models
{
    public class Category: ModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
