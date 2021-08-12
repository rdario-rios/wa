using ContactsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            var skillLevels = new SkillLevel[]
            {
            new SkillLevel{SkillLevelID=101,Name="Beginner",Level=1},
            new SkillLevel{SkillLevelID=102,Name="Average",Level=2},
            new SkillLevel{SkillLevelID=103,Name="Skilled",Level=3},
            new SkillLevel{SkillLevelID=104,Name="Specialist",Level=4},
            new SkillLevel{SkillLevelID=105,Name="Expert",Level=5}
            };
            foreach (SkillLevel s in skillLevels)
            {
                context.SkillLevels.Add(s);
            }
            context.SaveChanges();

            var skillDefinition = new SkillDefinition[]
            {
            new SkillDefinition{SkillDefinitionID=101,Definition="Productivity software"},
            new SkillDefinition{SkillDefinitionID=102,Definition="Operating systems"},
            new SkillDefinition{SkillDefinitionID=103,Definition="Presentation software"},
            new SkillDefinition{SkillDefinitionID=104,Definition="Digital marketing"},
            new SkillDefinition{SkillDefinitionID=105,Definition="Computer programming"},
            new SkillDefinition{SkillDefinitionID=106,Definition="Graphic design"},
            new SkillDefinition{SkillDefinitionID=107,Definition="Communication tools"},
            new SkillDefinition{SkillDefinitionID=108,Definition="Database management"}
            };
            foreach (SkillDefinition s in skillDefinition)
            {
                context.SkillDefinitions.Add(s);
            }
            context.SaveChanges();

            var contacts = new Contact[]
            {
            new Contact{ContactID=1051,Firstname="Steve",Lastname="Jobs",Address="",Email="steve.jobs@contactsapi.com",MobilPhoneNumber=""},
            new Contact{ContactID=1052,Firstname="Bill",Lastname="Gates",Address="",Email="bill.gates@contactsapi.com",MobilPhoneNumber=""},
            new Contact{ContactID=1053,Firstname="Jeff",Lastname="Bezos",Address="",Email="jeff.bezos@contactsapi.com",MobilPhoneNumber=""}
            };
            foreach (Contact c in contacts)
            {
                context.Contacts.Add(c);
            }
            context.SaveChanges();

            var skills = new Skill[]
            {
            new Skill{ContactID=1051,SkillDefinitionID=103,SkillLevelID=105},
            new Skill{ContactID=1051,SkillDefinitionID=104,SkillLevelID=104},
            new Skill{ContactID=1052,SkillDefinitionID=101,SkillLevelID=104},
            new Skill{ContactID=1052,SkillDefinitionID=102,SkillLevelID=104},
            };
            foreach (Skill s in skills)
            {
                context.Skills.Add(s);
            }
            context.SaveChanges();
        }
    }
}
