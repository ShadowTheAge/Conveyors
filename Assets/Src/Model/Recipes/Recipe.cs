using System;
using Simulation;

namespace Model
{
    public enum RecipeTag
    {
        Smelting,
        Crushing,
        Alloying,
        Assembling,
        Refining,
        Separating,
    }

    public abstract class RecipeResult
    {
        public struct RandomVariant
        {
            public Item item;
            public float probability;
        }

        public Item result;
        public RandomVariant[] variants;
        
        public Item GetItem(ProcessingMachine machine)
        {
            if (variants != null)
            {
                var value = machine.modelWorld.random.NextDouble();
                foreach (var variant in variants)
                {
                    value -= variant.probability;
                    if (value < 0)
                        return variant.item;
                }
            }

            return result;
        }
    }
    
    public class Recipe
    {
        public RecipeTag tag;
        public Item[] ingredients;
        public RecipeResult[] results;
        public int ticks;

        public void GenerateResult(ProcessingMachine machine)
        {
            var count = Math.Min(machine.outputs.Length, results.Length);
            for (var i = 0; i < count; i++)
            {
                var item = results[i].GetItem(machine);
                if (item != Item.None)
                    machine.outputs[i].buffer = item;
            }
        }
    }
}