﻿using System.Collections.Generic;
using Simulation;

namespace Model
{
    // represents a single item buffer between two machines/conveyors/pipes/etc

    public abstract class ItemProcessor : ModelObject
    {
        public abstract bool Insert(Item item, int slot);
        public virtual bool CanInsert(Item item, int slot) => ItemAt(slot) != Item.None;
        public abstract Item ItemAt(int slot);
        public abstract int slotCount { get; }
        protected bool inventoryChanged;

        protected bool InsertAt(Item item, ref Item target)
        {
            if (target == Item.None)
            {
                target = item;
                inventoryChanged = true;
                return true;
            }

            return false;
        }
    }
}