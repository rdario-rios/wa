using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Models
{
    public class Contact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContactID { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(2)]
        [MaxLength(30)]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(2)]
        [MaxLength(30)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(2)]
        [MaxLength(30)] 
        public string Address { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(2)]
        [MaxLength(30)] 
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(2)]
        [MaxLength(30)]
        public string MobilPhoneNumber { get; set; }
        public ICollection<Skill> Skills { get; set; }
    }
}
