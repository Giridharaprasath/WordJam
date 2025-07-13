using System;
using System.Collections.Generic;

namespace WordJam
{
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
