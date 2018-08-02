using System.Diagnostics;
using System.Runtime.InteropServices;
using Simulation;

namespace Model
{
    public class ProcessingMachine : ModelObject
    {
        public ProcessingMachine(int inputCount, int outputCount, RecipeBook book)
        {
            this.recipeBook = book;
            
            inputs = new ProcessingMachineItemConnection[inputCount];
            for (var i = 0; i < inputCount; i++)
                inputs[i] = new ProcessingMachineItemConnection(this);
            
            outputs = new ProcessingMachineItemConnection[outputCount];
            for (var i = 0; i < outputCount; i++)
                outputs[i] = new ProcessingMachineItemConnection(this);
        }
        
        public class ProcessingMachineItemConnection : ItemConnection
        {
            private readonly ProcessingMachine machine;
            public Item buffer;

            public ProcessingMachineItemConnection(ProcessingMachine machine)
            {
                this.machine = machine;
            }

            public override bool Push(Item item)
            {
                if (buffer != Item.None)
                    return false;
                buffer = item;
                machine.inventoryChanged = true;
                return true;
            }

            public override Item Pull()
            {
                var ret = buffer;
                if (buffer != Item.None)
                {
                    machine.inventoryChanged = true;
                    buffer = Item.None;
                }
                return ret;
            }
        }

        private int readyTick;
        private Recipe recipe;
        private readonly RecipeBook recipeBook;
        private bool inventoryChanged = false;

        public readonly ProcessingMachineItemConnection[] inputs;
        public readonly ProcessingMachineItemConnection[] outputs;

        public override void Tick()
        {
            if (readyTick > modelWorld.tick || !inventoryChanged)
                return;

            inventoryChanged = false;
            if (readyTick > 0)
            {
                if (readyTick == modelWorld.tick)
                    recipe.GenerateResult(this);

                foreach (var output in outputs)
                {
                    if (output.buffer != Item.None)
                        return;
                }

                readyTick = 0;
            }
            
            if (recipeBook.GetRecipe(inventory, ref recipe))
            {
                readyTick = modelWorld.tick + recipe.ticks;
            }
        }
    }
}