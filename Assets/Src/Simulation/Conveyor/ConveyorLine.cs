using System;

namespace Simulation
{
    public class ConveyorLine : ItemProcessor
    {
        public ConveyorLine(ConveyorLineBuilder builder, params ConveyorLineSegment[] segments)
        {
            this.segments = segments;
            this.builder = builder;
            Init();
        }

        private void Init()
        {
            foreach (var segment in segments)
            {
                segment.SetConveyorLine(this, _length);
                _length += segment.length;
            }
            ringBuffer = new Item[length];
        }

        private readonly ConveyorLineBuilder builder;
        private ConveyorLineSegment[] segments;
        private ItemProcessor finalTarget;
        
        private Item[] ringBuffer;
        private int offset;
        private int _length;

        public Item head
        {
            get { return ringBuffer[offset]; }
            set { ringBuffer[offset] = value; }
        }
        public bool stuck => head != Item.None;

        public int length => _length;
        public Item this[int index] => ringBuffer[Index(index)];

        private int Index(int index) => (index + offset) % _length;

        private ConveyorLineSegment[] Slice(int offset, int length)
        {
            var copy = new ConveyorLineSegment[length];
            Array.Copy(segments, offset, copy, 0, length);
            return copy;
        }

        public bool AcceptItem(Item item, int position)
        {
            var index = Index(position);
            if (ringBuffer[index] != Item.None)
                return false;
            ringBuffer[index] = item;
            return true;
        }

        public Item Take(int position)
        {
            var index = Index(position);
            var item = ringBuffer[index];
            ringBuffer[index] = Item.None;
            return item;
        }

        private void Increment(int offset, int index, ConveyorLineSegment segment)
        {
            var prevSegments = segments;
            segments = new ConveyorLineSegment[prevSegments.Length + 1];
            Array.Copy(prevSegments, 0, segments, offset, prevSegments.Length);
            segments[index] = segment;
            Init();
        }

        public void Prepend(ConveyorLineSegment segment)
        {
            Increment(1, 0, segment);
        }

        public void Append(ConveyorLineSegment segment)
        {
            Increment(0, segments.Length, segment);
        }

        public ConveyorLine Merge(ConveyorLineSegment segment, ConveyorLineSegment post)
        {
            var addLine = post.line;
            var prevSegments = segments;
            segments = new ConveyorLineSegment[prevSegments.Length + 1 + addLine.segments.Length];
            Array.Copy(prevSegments, 0, segments, 0, prevSegments.Length);
            Array.Copy(addLine.segments, 0, segments, prevSegments.Length + 1, addLine.segments.Length);
            segments[prevSegments.Length] = segment;
            Init();
            return addLine;
        }

        public void RemoveSegment(ConveyorLineSegment segment)
        {
            var split = Array.IndexOf(segments, segment);
            if (split == -1)
                return;
            if (split == 0 && segments.Length == 1)
            {
                builder.ConveyorLineRemove(this);
                return;
            }

            if (split == 0)
                segments = Slice(1, segments.Length - 1);
            else if (split == segments.Length - 1)
                segments = Slice(0, segments.Length - 1);
            else
            {
                var part2 = Slice(split + 1, segments.Length - split - 1);
                segments = Slice(0, split);
                builder.ConveyorLineCreate(new ConveyorLine(builder, part2));
            }
            Init();
        }

        public void Move()
        {
            if (ringBuffer[offset] == Item.None)
                offset = (offset + 1) % _length;
        }
    }
}