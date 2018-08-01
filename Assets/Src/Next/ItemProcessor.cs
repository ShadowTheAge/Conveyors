using System.Collections;
using Simulation;

namespace Next
{
    public struct OutputTarget
    {
        public Item item;
        public ItemProcessor processor;
        public int slot;

        public bool TryPush()
        {
            if (item == Item.None)
                return true;
            if (processor.Insert(item, slot))
            {
                item = Item.None;
                return true;
            }

            return false;
        }

        public bool stuck => item != Item.None && (processor == null || !processor.CanInsert(item, slot));
    }
    
    public abstract class ItemProcessor : WorldObject
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