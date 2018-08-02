using System.Collections.Generic;
using Simulation;

namespace Model
{
    public class ConveyorLine : ModelObject
    {
        public class MidConveyorConnection : ItemConnection
        {
            public ConveyorLine line;
            public int slot;

            public override Item Peek()
            {
                return line.ItemAt(slot);
            }

            public override bool Push(Item item)
            {
                return line.Insert(item, slot);
            }

            public override Item Pull()
            {
                return line.Extract(slot);
            }
        }
        
        private Item[] buffer;
        private int offset;

        private ItemConnection input = ItemConnection.Void;
        private ItemConnection output = ItemConnection.Void;
        private readonly List<MidConveyorConnection> connections = new List<MidConveyorConnection>();

        private int BufferIndex(int slot)
        {
            return (offset + slot) % buffer.Length;
        }
        
        public bool Insert(Item item, int slot)
        {
            var idx = BufferIndex(slot);
            if (buffer[idx] != Item.None)
                return false;
            buffer[idx] = item;
            return true;
        }

        public Item ItemAt(int slot)
        {
            return buffer[BufferIndex(slot)];
        }

        public Item Extract(int slot)
        {
            var idx = BufferIndex(slot);
            var item = buffer[idx];
            buffer[idx] = Item.None;
            return item;
        }

        public override bool PreTick()
        {
            var itemAtEnd = buffer[offset];
            if (itemAtEnd != Item.None)
            {
                if (!output.Push(itemAtEnd))
                    return false;
            }

            buffer[offset] = input.Pull();
            return true;
        }

        public override void Tick()
        {
            offset++;
            if (offset == buffer.Length)
                offset = 0;
        }
    }
}