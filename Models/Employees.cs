using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace Gistics.Models
{
    public class Employees
    {
        // Add validation: TitleID must be greater than 0 (select a real title)
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Title.")]
        public int TitleID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name must be at most 50 characters.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name must be at most 50 characters.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "BirthDay is required.")]
        public required DateOnly BirthDate { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public required DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        [Required(ErrorMessage = "BadgeNumber is required.")]
        [StringLength(6, ErrorMessage = "BadgeNumber must be 6 characters.")]
        public required string BadgeNumber { get; set; }

        [ForeignKey(nameof(TitleID))]
        public required EmployeeTitles Title { get; set; }

        public Employees()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            BadgeNumber = string.Empty;
            Title = new EmployeeTitles { Name = string.Empty };
            BirthDate = DateOnly.FromDateTime(DateTime.MinValue);
            StartDate = DateOnly.FromDateTime(DateTime.MinValue);
        }
    }
}