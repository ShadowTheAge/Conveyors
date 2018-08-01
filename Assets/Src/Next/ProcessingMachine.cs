using System.Diagnostics;
using Simulation;

namespace Next
{
    public abstract class ProcessingMachine : ItemProcessor
    {
        protected ProcessingMachine(int inventoryCount, int outputCount)
        {
            inventory = new Item[inventoryCount];
            outputs = new OutputTarget[outputCount];
        }

        public readonly Item[] inventory;
        public readonly OutputTarget[] outputs;
        private int readyTick;
        private Recipe recipe;
        private RecipeBook recipeBook;
        
        public override bool Insert(Item item, int slot)
        {
            return InsertAt(item, ref inventory[slot]);
        }

        public override Item ItemAt(int slot)
        {
            return inventory[slot];
        }

        public override bool CanInsert(Item item, int slot)
        {
            return readyTick == 0 && CanAcceptItem(item, slot);
        }

        protected bool CanAcceptItem(Item item, int slot) => true;

        public override int slotCount => inventory.Length;

        public override bool locked => readyTick != 0;

        public override void ProcessTick()
        {
            if (readyTick == 0)
            {
                if (!inventoryChanged)
                    return;
                inventoryChanged = false;
                if (recipeBook.GetRecipe(inventory, ref recipe))
                {
                    readyTick = world.tick + recipe.ticks;
                }
            }
            else if (readyTick <= world.tick)
            {
                if (readyTick == world.tick)
                    recipe.GenerateResult(this);
                var empty = true;
                foreach (var output in outputs)
                {
                    if (!output.TryPush())
                        empty = false;
                }

                if (empty)
                    readyTick = 0;
            }
        }
    }
}