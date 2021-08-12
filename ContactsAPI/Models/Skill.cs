using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Models
{
    public class Skill
    {

        public int SkillID { get; set; }

        [Required(ErrorMessage = "Required")]
        public int ContactID { get; set; }

        [Required(ErrorMessage = "Required")]
        public int SkillDefinitionID { get; set; }

        [Required(ErrorMessage = "Required")]
        public int SkillLevelID { get; set; }

        public Contact Contact { get; set; }
        public SkillDefinition SkillDefinition { get; set; }
        public SkillLevel SkillLevel { get; set; }
    }
}
