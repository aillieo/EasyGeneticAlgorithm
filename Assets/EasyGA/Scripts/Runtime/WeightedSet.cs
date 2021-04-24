using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AillieoUtils
{
    public class WeightedSet<T>
    {
        private Random rand = new Random();

        private class Entry
        {
            public float weight;
            public T value;

            public Entry(T value, float weight = 1)
            {
                this.weight = weight;
                this.value = value;
            }
        }

        private List<Entry> managedItems = new List<Entry>();

        public void Add(IEnumerable<KeyValuePair<T, float>> values)
        {
            foreach (var pair in values)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public void Add(T value, float weight = 1f)
        {
            if (weight <= 0)
            {
                throw new Exception("invalid");
            }

            managedItems.Add(new Entry(value, weight));
        }

        public void Clear()
        {
            managedItems.Clear();
        }

        public int Remove(T value)
        {
            return managedItems.RemoveAll(e => EqualityComparer<T>.Default.Equals(e.value, value));
        }

        private IEnumerable<T> InternalRandomTake(int count)
        {
            //if (count == 1)
            //{
            //    yield return RandomTake();
            //    yield break;
            //}

            // deepcopy
            List<Entry> deepcopy = new List<Entry>();
            deepcopy.AddRange(managedItems);

            for (var i = 0; i < count; i++)
            {
                float weightSum = deepcopy.Sum(item => item.weight);
                float ran = (float)(rand.NextDouble() * weightSum);

                float sum = 0f;
                for (var j = 0; j < deepcopy.Count; j++)
                {
                    sum += deepcopy[j].weight;

                    //UnityEngine.Debug.LogError($"{sum} : {ran}");

                    if (sum > ran)
                    {
                        yield return deepcopy[j].value;

                        // 移到最后一个
                        var last = deepcopy[deepcopy.Count - 1];
                        deepcopy[j] = last;
                        deepcopy.RemoveAt(deepcopy.Count - 1);

                        break;
                    }
                }
            }

            yield break;
        }

        public IEnumerable<T> RandomTake(int count)
        {
            if (count > managedItems.Count)
            {
                throw new Exception("not enough items");
            }

            if (count == managedItems.Count)
            {
                return managedItems.Select(e => e.value);
            }

            return InternalRandomTake(count);
        }

        public T RandomTake()
        {
            if (managedItems.Count == 0)
            {
                throw new Exception("not enough items");
            }

            float weightSum = managedItems.Sum(item => item.weight);
            float ran = (float)(rand.NextDouble() * weightSum);
            float sum = 0f;
            for (var j = 0; j < managedItems.Count; j++)
            {
                sum += managedItems[j].weight;
                if (sum > ran)
                {
                    return managedItems[j].value;
                }
            }

            return default;
        }
    }

}
