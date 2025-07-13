using System;
using System.Collections.Generic;

namespace WordJam
{
    /// <summary>
    /// WeightedRandom class provides methods to add items with associated weights and retrieve a random item based on the weights.
    /// This class is used to select items randomly with a probability proportional to their weights.
    /// Each item is represented by an index, and the weights determine the likelihood of selecting each item.
    /// </summary>
    public class WeightedRandom
    {
        private struct WeightedItem
        {
            public int index;
            public double accumulatedWeight;
        }

        private List<WeightedItem> items = new();
        private double totalWeight = 0f;
        private Random random = new();

        public void AddItem(int index, double weight)
        {
            totalWeight += weight;
            items.Add(new WeightedItem
            {
                index = index,
                accumulatedWeight = totalWeight
            });
        }

        public int GetRandomItem()
        {
            double r = random.NextDouble() * totalWeight;

            foreach (var item in items)
            {
                if (item.accumulatedWeight >= r)
                {
                    return item.index;
                }
            }

            return 1;
        }
    }
}
