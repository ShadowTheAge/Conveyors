using System;
using System.Collections.Generic;

namespace Model
{
    public class ModelWorld
    {
        public int tick;
        private readonly HashSet<ModelObject> worldObjects = new HashSet<ModelObject>();

        private readonly List<ModelObject> itemsToUpdate = new List<ModelObject>();
        public readonly Random random = new Random();

        private void Tick()
        {
            ++tick;
            foreach (var worldObject in worldObjects)
            {
                if (worldObject.PreTick())
                    itemsToUpdate.Add(worldObject);
            }

            foreach (var item in itemsToUpdate)
                item.Tick();
        }
    }
}