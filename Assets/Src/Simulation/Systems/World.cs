using System.Collections;
using System.Collections.Generic;

namespace Simulation
{
    public class World
    {
        private class EventHandlers<T> : HashSet<T> where T : class
        {
            public void AddHandler<TObj>(TObj obj) where TObj : SimObject
            {
                var casted = obj as T;
                if (casted != null)
                    Add(casted);
            }
            
            public void RemoveHandler<TObj>(TObj obj) where TObj : SimObject
            {
                var casted = obj as T;
                if (casted != null)
                    Remove(casted);
            }
        }
        
        private readonly HashSet<SimObject> objects = new HashSet<SimObject>();
        private readonly EventHandlers<IWorldStart> worldStart = new EventHandlers<IWorldStart>();
        private readonly EventHandlers<ITick> tickHandlers = new EventHandlers<ITick>();

        public int tick { get; private set; }

        public void Tick()
        {
            ++tick;
            foreach (var tickHandler in tickHandlers)
                tickHandler.Tick();
        }

        public readonly ConveyorLineBuilder conveyors;

        public World()
        {
            conveyors = new ConveyorLineBuilder {world = this};
        }
        
        public void Add(SimObject simObject)
        {
            objects.Add(simObject);
            worldStart.AddHandler(simObject);
            tickHandlers.AddHandler(simObject);
        }

        public void Remove(SimObject simObject)
        {
            if (objects.Remove(simObject))
            {
                worldStart.RemoveHandler(simObject);
                tickHandlers.RemoveHandler(simObject);
            }
        }
    }
}