using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Payment
    {
        public int? Id { get; set; }
        public string? userId { get; set; }
        //[ForeignKey("userId")]
        //public AppUser User { get; set; }
        public  string? AcquiredDate { get; set; }
        public string? PaymentStatus { get; set; } = "Pending";

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
        [InverseProperty("Payment")]
        public List<Receipt>? Receipts { get; set; }
        
        public string? GcashNumber { get; set; }
        public string? GcashName { get; set; }
        public string? DelinquentStatus { get; set; } = "No";
        





    }
}
