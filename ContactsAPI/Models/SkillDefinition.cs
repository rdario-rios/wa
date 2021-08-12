using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsAPI.Models
{
    public class SkillDefinition
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SkillDefinitionID { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(5)]
        [MaxLength(50)] 
        public string Definition { get; set; }

    }
}
