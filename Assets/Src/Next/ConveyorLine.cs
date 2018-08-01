using System.Collections;
using Simulation;

namespace Next
{
    public class ConveyorLine : ItemProcessor
    {
        private Item[] buffer;
        private int offset;
        private OutputTarget output;

        private int BufferIndex(int slot)
        {
            return (offset + slot) % buffer.Length;
        }
        
        public override bool Insert(Item item, int slot)
        {
            return InsertAt(item, ref buffer[BufferIndex(slot)]);
        }

        public override Item ItemAt(int slot)
        {
            return buffer[BufferIndex(slot)];
        }

        public override int slotCount => buffer.Length;

        public override bool locked
        {
            get { return output.stuck; }
        }

        public override void ProcessTick()
        {
            if (output.TryPush())
            {
                offset--;
                if (offset < 0)
                    offset += buffer.Length;
                output.item = buffer[offset];
                buffer[offset] = Item.None;
            }
        }
    }
}