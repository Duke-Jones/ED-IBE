using System;
using EdClasses.ClassDefinitions;

namespace RegulatedNoise
{
    class EdLogLine
    {
        public DateTime Date { get; set; }

        public bool isSystem { get; set; }

        private string line { get; set; }
        public EdLogLine(string logline)
        {
            if (!logline.StartsWith("{")){return;} //If it doesnt starts with a { its a metadata line, not a logline with timestamp
            Date = DateTime.Parse(logline.Substring(1, 8)); //The DATE itself it not written just Time. Don't care to do magic to figure out the date before we need it!

            line = logline.Substring(11);
            if (line.StartsWith("System:"))
            {
                isSystem = true;
            }
        }

        public EdSystem parseSystem()
        {
            var system = new EdSystem();

            var startOfSystemName = line.IndexOf("(", StringComparison.Ordinal);
            var endOfSystemName = line.IndexOf(")", startOfSystemName, StringComparison.Ordinal);
            system.Id = int.Parse(line.Substring(7, startOfSystemName - 7));
            system.Name = line.Substring(startOfSystemName+1, endOfSystemName - startOfSystemName - 1);
            //system.Stations Should be filled from database

            return system;
        }
    }
}
