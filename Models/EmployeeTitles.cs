using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gistics.Models
{
    [Table("Employee_Titles")]
    public class EmployeeTitles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("EmpTitleID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [Column("Title")]
        [StringLength(50, ErrorMessage = "Title must be at most 50 characters.")]
        public required string Name { get; set; }
    }
}
