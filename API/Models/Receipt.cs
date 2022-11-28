using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class Receipt
    {
        public int? Id { get; set; }
        public int? PaymentId { get; set; }
        public string? UserId { get; set; }
        [JsonIgnore]
        public Payment? Payment { get; set; }
        public string? RefNo { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ORNo { get; set; }
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
        public DateTime? ReceiptDate { get; set; }
        
        public string? Name{ get; set; }
        public string? Status { get; set; } = "Pending";
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        

    }
}
