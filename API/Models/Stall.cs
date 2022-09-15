using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Stall
    {
        public int Id { get; set; }
        public int StallNumber { get; set; }
        public string Dimension { get; set; }

        [Column(TypeName = "money")]
        public decimal MonthlyPayment { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string? StallType { get; set; }
       
      

    }
}
