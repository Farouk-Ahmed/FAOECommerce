namespace CleanArchitecture.Services.DTOs.Categories
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
    }
    public class CategoryUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}