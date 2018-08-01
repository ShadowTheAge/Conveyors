using System.Collections.Generic;

namespace Simulation
{
    public class World
    {        
        private readonly HashSet<SimObject> objects = new HashSet<SimObject>();
        private HashSet<SimObject> thisTickUpdateQueue = new HashSet<SimObject>();
        private HashSet<SimObject> nextTickUpdateQueue = new HashSet<SimObject>();

        public int tick { get; private set; }

        public void Tick()
        {
            ++tick;
            var tmp = thisTickUpdateQueue;
            thisTickUpdateQueue = nextTickUpdateQueue;
            nextTickUpdateQueue = tmp;
            
            foreach (var tickHandler in thisTickUpdateQueue)
                tickHandler.Update();
            thisTickUpdateQueue.Clear();
        }

        public void QueueUpdate(SimObject obj)
        {
            nextTickUpdateQueue.Add(obj);
        }

        public readonly ConveyorLineBuilder conveyors;

        public World()
        {
            conveyors = new ConveyorLineBuilder {world = this};
        }
        
        public void Add(SimObject simObject)
        {
            objects.Add(simObject);
        }

        public void Remove(SimObject simObject)
        {
            objects.Remove(simObject);
        }
    }
}