using System.Collections.Generic;

namespace EdClasses.ClassDefinitions
{
    public class EdSystem
    {
        public EdSystem()
        {
            Stations = new List<IEdStation>();
        }
        public int Id { get; set; } //Predefined, also defined in log
        public string Name { get; set; }
        public List<IEdStation> Stations { get; set; }
    }
}
