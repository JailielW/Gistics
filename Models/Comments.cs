using System.ComponentModel.DataAnnotations.Schema;

namespace Gistics.Models
{
    [Table("COMMENTS")]
    public class Comments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }

        public required string Comment { get; set; }

        public Comments()
        {
            Comment = string.Empty;
        }
    }
}
