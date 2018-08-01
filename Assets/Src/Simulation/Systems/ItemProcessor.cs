using System.Collections;

namespace Simulation
{
    public abstract class ItemProcessor : SimObject
    {
        private ItemProcessor _inputLock;
        protected ConveyorLineSegment[] inputs;
        protected ConveyorLineSegment[] outputs;

        public void UpdateLockState()
        { 
            var newLock = _selfLocked ? this : ComputeInputLock();
            if (newLock == _inputLock)
                return;
            _inputLock = newLock;
            UpdateOutputLocks();
        }

        public ItemProcessor inputLock => _inputLock != null && !_inputLock._selfLocked ? _inputLock : null;

        protected ItemProcessor ComputeInputLock()
        {
            foreach (var output in outputs)
            {
                var outLock = output.outputLock;
                if (outLock != null)
                    return outLock;
            }

            return null;
        }

        protected void UpdateOutputLocks()
        {
            foreach (var input in inputs)
            {
                input.UpdateInputLock();
            }
        }

        private bool _selfLocked;

        public bool selfLocked
        {
            get { return _selfLocked; }
            set { _selfLocked = value; UpdateLockState(); }
        }
    }
}