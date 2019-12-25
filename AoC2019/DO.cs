using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2019
{
    class UniverseObject
    {
        public string ID { get; }

        public UniverseObject(string ID)
        {
            this.ID = ID;
        }

        private UniverseObject orbitParent;
        public UniverseObject OrbitParent
        {
            get => orbitParent;
            set { orbitParent = value;  value.orbitChildren.Add(this); }
        }

        private readonly List<UniverseObject> orbitChildren= new List<UniverseObject>();
        public IEnumerable<UniverseObject> OrbitChildren => orbitChildren;
    }
}
