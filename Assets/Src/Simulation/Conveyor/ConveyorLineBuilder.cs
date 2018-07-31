using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulation
{
    public class ConveyorLineBuilder : SimObject, IWorldStart, ITick
    {
        private readonly Dictionary<MachinePos, ConveyorLineSegment> inputs = new Dictionary<MachinePos, ConveyorLineSegment>(MachinePos.Comparer);
        private readonly Dictionary<MachinePos, ConveyorLineSegment> outputs = new Dictionary<MachinePos, ConveyorLineSegment>(MachinePos.Comparer);
        private readonly HashSet<ConveyorLine> lines = new HashSet<ConveyorLine>();

        public void AddSegment(ConveyorLineSegment segment)
        {
            ConveyorLineSegment pre = null, post = null;
            if (segment.input != Direction.Invalid)
            {
                var input = new MachinePos(segment.position, segment.input);
                input.pos.y += segment.deltaH;
                inputs[input] = segment;
                outputs.TryGetValue(input.GetConnected(), out pre);
            }

            if (segment.output != Direction.Invalid)
            {
                var output = new MachinePos(segment.position, segment.output);
                outputs[output] = segment;
                inputs.TryGetValue(output.GetConnected(), out post);
            }

            if (pre == null)
            {
                if (post == null)
                    ConveyorLineCreate(new ConveyorLine(this, segment));
                else
                    post.line.Prepend(segment);
            }
            else
            {
                if (post == null || post.line == pre.line)
                    pre.line.Append(segment);
                else
                    ConveyorLineRemove(pre.line.Merge(segment, post));
            }
        }

        public void RemoveSegment(ConveyorLineSegment segment)
        {
            segment.line.RemoveSegment(segment);
        }

        public void ConveyorLineCreate(ConveyorLine line)
        {
            lines.Add(line);
        }

        public void ConveyorLineRemove(ConveyorLine line)
        {
            lines.Remove(line);
        }

        public void WorldStart()
        {
            
        }

        public void Tick() {}
    }
}