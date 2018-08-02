using System.Collections;
using Model;

namespace Simulation
{
    public class ConveyorLineSegment
    {
        public readonly Coord position;
        public readonly byte length;
        public readonly sbyte deltaH;
        public readonly Direction input;
        public readonly Direction output;

        public ConveyorLineSegment(Coord position, byte length, sbyte deltaH, Direction input, Direction output)
        {
            this.position = position;
            this.length = length;
            this.deltaH = deltaH;
            this.input = input;
            this.output = output;
        }

        public ConveyorLine line { get; private set; }
        public bool stuck => line.stuck;
        public Item head
        {
            get { return line.head; }
            set { line.head = value; }
        }

        protected int lineOffset;

        public bool Push(Item item, int offset = 0) => line.AcceptItem(item, offset + lineOffset);

        public void SetConveyorLine(ConveyorLine line, int lineOffset)
        {
            this.line = line;
            this.lineOffset = lineOffset;
        }

        public ItemProcessor outputLock => null;

        public void UpdateInputLock()
        {
            line.UpdateLockState();
        }
    }
}