using System.Collections;

namespace Simulation
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
    
    public class Recipe
    {
        public RecipeTag tag;
        public Item source0;
        public Item source1;
        public Item result;
        public int ticks = 3;
    }
}