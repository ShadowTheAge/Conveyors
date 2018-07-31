using System.Collections;

namespace Simulation
{
    public class RecipeMachine : Machine, ITick
    {
        private Recipe currentRecipe;
        private int recipeReadyTick;
        private RecipeBook book;

        private ConveyorLineSegment input;
        private ConveyorLineSegment output;
        
        public void Tick()
        {
            if (currentRecipe != null)
            {
                if (world.tick < recipeReadyTick)
                    return;
                if (output.stuck)
                    return;
                GenerateRecipeResults();
                currentRecipe = null;
            }

            if (currentRecipe == null)
            {
                var item = input.head;
                if (item != Item.None) {
                    currentRecipe = TryStartRecipe(item);
                    if (currentRecipe != null)
                    {
                        input.head = Item.None;
                        recipeReadyTick = world.tick + currentRecipe.ticks;
                    }
                }
            }
        }

        protected virtual void GenerateRecipeResults()
        {
            output.Push(currentRecipe.result);
        }

        protected virtual Recipe TryStartRecipe(Item item)
        {
            return book.GetRecipe(item);
        }

        public override void Init(World world)
        {
            input = new ConveyorLineSegment(machinePos, 1, 0, machinePos.back, Direction.Invalid);
            output = new ConveyorLineSegment(machinePos, 2, 0, Direction.Invalid, machinePos.direction);
            world.conveyors.AddSegment(input);
            world.conveyors.AddSegment(output);
            
            base.Init(world);
        }
    }
}