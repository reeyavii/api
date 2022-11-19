using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Receipt
    {
        public int? Id { get; set; }
        public int? PaymentId { get; set; }
        public string? UserId { get; set; }
  
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

        

    }
}
