using System.ComponentModel.DataAnnotations.Schema;

namespace BethaniePieShop.Models
{
    [Table("Feedback")]
    public class Feedback
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool ContactMe { get; set; }
    }
}