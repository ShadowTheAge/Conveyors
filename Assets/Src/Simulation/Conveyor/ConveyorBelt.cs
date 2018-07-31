using System;
using System.Collections;

namespace Simulation
{
    public class ConveyorBelt : Machine
    {   
        private ConveyorLineSegment segment;
        private ConveyorBeltAttachment attachment;
        private Direction bend;
        private sbyte dh;

        public override void Init(World world)
        {
            segment = new ConveyorLineSegment(machinePos.pos, 3, dh, machinePos.direction.Relative(bend), machinePos.direction);
            attachment?.Init(world);
            base.Init(world);
        }
    }
}