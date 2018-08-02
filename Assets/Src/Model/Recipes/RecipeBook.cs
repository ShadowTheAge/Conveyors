using System;
using System.Collections.Generic;
using Simulation;

namespace Model
{
    public class RecipeBook
    {
        private readonly Dictionary<Item, int> allowedItems = new Dictionary<Item, int>();
        private readonly Dictionary<Item[], Recipe> recipes;
        private readonly IEqualityComparer<Item[]> equalityComparer;
        public readonly bool swappable;

        public RecipeBook(bool swappable)
        {
            this.swappable = swappable;
            equalityComparer = swappable ? SwappableRecipeComparer.Instance : FixedRecipeComparer.Instance;
            recipes = new Dictionary<Item[], Recipe>(equalityComparer);
        }
        
        private class FixedRecipeComparer : IEqualityComparer<Item[]>
        {
            public static readonly IEqualityComparer<Item[]> Instance = new FixedRecipeComparer();
            
            public bool Equals(Item[] recipe, Item[] check)
            {
                if (check.Length < recipe.Length)
                    return false;
                for (var i = 0; i < recipe.Length; i++)
                    if (recipe[i] != check[i])
                        return false;
                return true;
            }

            public int GetHashCode(Item[] recipe)
            {
                unchecked
                {
                    var hashCode = 0;
                    var mul = 397;
                    foreach (var item in recipe)
                    {
                        if (item != Item.None)
                            hashCode = hashCode + (int)item * mul;
                        mul *= 397;
                    }   
                    return hashCode;
                }
            }
        }

        private class SwappableRecipeComparer : IEqualityComparer<Item[]>
        {
            public static readonly IEqualityComparer<Item[]> Instance = new SwappableRecipeComparer();
            public bool Equals(Item[] recipe, Item[] check)
            {
                var prev = Item.None;
                var lastIndex = 0;
                foreach (var item in recipe)
                {
                    if (item != prev)
                        lastIndex = 0;
                    lastIndex = Array.IndexOf(check, prev, lastIndex);
                    if (lastIndex < 0)
                        return false;
                    prev = item;
                }

                return true;
            }

            public int GetHashCode(Item[] recipe)
            {
                unchecked
                {
                    var hashCode = 0;
                    foreach (var item in recipe)
                        if (item != Item.None)
                            hashCode = (hashCode + (int) item * (int) item * (int) item);
                    return hashCode;
                }
            }
        }

        public Recipe GetRecipe(Item[] recipe, Recipe previous)
        {
            if (previous == null || !equalityComparer.Equals(previous.ingredients, recipe))
                recipes.TryGetValue(recipe, out previous);
            return previous;
        }

        public bool GetRecipe(Item[] recipe, ref Recipe previous)
        {
            var newRecipe = GetRecipe(recipe, previous);
            if (newRecipe == null)
                return false;
            previous = newRecipe;
            return true;
        }

        public void AddRecipe(Recipe recipe)
        {
            for (var i = 0; i < recipe.ingredients.Length; i++)
            {
                int slotMap;
                if (swappable)
                    slotMap = -1;
                else
                {
                    allowedItems.TryGetValue(recipe.ingredients[i], out slotMap);
                    slotMap |= (1 << i);
                }

                allowedItems[recipe.ingredients[i]] = slotMap;
            }

            recipes[recipe.ingredients] = recipe;
        }
        
        public bool CanAcceptItem(Item item, int slot)
        {
            int slotMap;
            if (!allowedItems.TryGetValue(item, out slotMap))
                return false;
            return swappable || (slotMap & (1 << slot)) != 0;
        }
    }
}