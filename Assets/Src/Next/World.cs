using System;
using System.Collections.Generic;

namespace Next
{
    public class World
    {
        public int tick;
        private readonly HashSet<WorldObject> worldObjects = new HashSet<WorldObject>();

        private List<WorldObject> itemsToUpdate = new List<WorldObject>();

        public Random random = new Random();

        private void Tick()
        {
            foreach (var worldObject in worldObjects)
            {
                if (!worldObject.locked)
                    itemsToUpdate.Add(worldObject);
            }

            foreach (var item in itemsToUpdate)
            {
                item.ProcessTick();
            }
            itemsToUpdate.Clear();
        }
    }
}