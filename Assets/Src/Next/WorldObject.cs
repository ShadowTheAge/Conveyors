using System.Collections;
using Simulation;

namespace Next
{
    public abstract class WorldObject
    {
        public World world { get; private set; }
        
        public abstract bool locked { get; }
        public abstract void ProcessTick();
        
        public virtual void Spawn(World world)
        {
            this.world = world;
        }
    }
}