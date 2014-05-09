using Chapter01.Threads;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chapter01
{
    public class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var programs = assembly.GetTypes()
                .Where(t => typeof(ProgramBase)
                    .IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface).ToList();

           
            int selected;
            bool isValid;
            do
            {
                Console.WriteLine("There are following tests:");

                for (int i = 0; i < programs.Count(); i++)
                {
                    var program = programs[i];
                    var attribute = program.GetCustomAttribute<DisplayNameAttribute>();
                    if (attribute == null)
                    {
                        throw new ArgumentException("Class {0} does not have DisplayName attribute.", program.FullName);
                    }

                    Console.WriteLine(i.ToString() + ' ' + attribute.DisplayName);
                }

                Console.WriteLine("Select one of them: ");

                var selectedProgram = Console.ReadKey();
                Console.WriteLine();
                isValid = int.TryParse(selectedProgram.KeyChar.ToString(), out selected);

                var instance = Activator.CreateInstance(programs[selected]);
                ((ProgramBase)instance).Run();
                Console.WriteLine();

            } while (isValid && selected >= 0 && selected < programs.Count());

            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
        }
    }
}
