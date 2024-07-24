using System.ComponentModel.DataAnnotations;

namespace WebApiDotnetCoreSample.DataStoreModel
{
    public class Pizza
    {
        [Key]
        public int PizzaId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}

