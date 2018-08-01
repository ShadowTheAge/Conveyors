using System.Collections;

namespace Simulation
{
    public interface IWorldStart
    {
        void WorldStart();
    }

    public interface ITick
    {
        void Tick();
    }
    
    public class SimObject
    {
        public World world;

        public virtual void Init(World world)
        {
            this.world = world;
            world.Add(this);
        }

        public virtual void Update() {}
    }
}