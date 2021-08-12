using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsAPI.Models
{
    public class SkillLevel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SkillLevelID { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
    }
}
