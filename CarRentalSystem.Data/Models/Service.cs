using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalSystem.Data.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public DateTime ServiceDate { get; set; }
    }
}
