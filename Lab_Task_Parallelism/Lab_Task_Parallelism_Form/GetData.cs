using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Task_Parallelism_Form
{
    public static class GetData
    {
        public static List<Person> GetPerson()
        {
            var Persons = new List<Person>
            {
                new Person {Id="01", Name="AAA" },
                new Person {Id="02", Name="BBB" },
                new Person {Id="03", Name="CCC" }
            };
            return Persons;
        }
    }
}
