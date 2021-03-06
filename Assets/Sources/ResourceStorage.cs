using System;
using System.IO;
using System.Linq;
using Eggland.Tools;
using UnityEngine;

namespace Eggland
{
    public class ResourceStorage : MonoBehaviour
    {
        public SerializedDictionary<ResourceType, int> state;

        private void Awake()
        {
            // Initialize
            state = new SerializedDictionary<ResourceType, int>
            {
                [ResourceType.WOOD] = 0,
                [ResourceType.COAL] = 0,
                [ResourceType.BRONZE] = 0,
                [ResourceType.IRON] = 0,
                [ResourceType.DIAMOND] = 0,
                [ResourceType.EMERALD] = 0,
                [ResourceType.BUSH_LEAF] = 0,
                [ResourceType.RUBY] = 0,
                [ResourceType.ROCK] = 0
            };
        }

        public int Get(ResourceType type) => state[type];

        public void Add(ResourceType type, int amount) => state[type] += amount;

        public void Use(ResourceType type, int amount) => state[type] -= amount;

        public void Synchronize()
        {
            foreach (var obj in FindObjectsOfType<ResourceUICount>())
            {
                obj.Sync(state[obj.type]);
            }
        }

        public static ResourceType GetToolResource(Tool tool)
        {
            var spriteName = tool.GetComponent<SpriteRenderer>().sprite.name;

            if (spriteName.Contains("bronze")) return ResourceType.BRONZE;
            if (spriteName.Contains("iron")) return ResourceType.IRON;
            if (spriteName.Contains("diamond")) return ResourceType.DIAMOND;
            if (spriteName.Contains("emerald")) return ResourceType.EMERALD;
            if (spriteName.Contains("ruby")) return ResourceType.RUBY;
            
            throw new ApplicationException();
        }

        public int CalculateGameScore()
        {
            return state.Sum(pair => ScoreForType(pair.Key) * pair.Value);
        }

        private static int ScoreForType(ResourceType type)
        {
            return type switch
            {
                ResourceType.WOOD => 1,
                ResourceType.COAL => 2,
                ResourceType.BRONZE => 3,
                ResourceType.IRON => 5,
                ResourceType.DIAMOND => 7,
                ResourceType.EMERALD => 9,
                ResourceType.RUBY => 11,
                ResourceType.BUSH_LEAF => 2,
                ResourceType.ROCK => 3,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public enum ResourceType
    {
        WOOD,
        COAL,
        BRONZE,
        IRON,
        DIAMOND,
        EMERALD,
        RUBY,
        BUSH_LEAF,
        ROCK
    }

    [Serializable]
    public struct Range
    {
        public int min;
        public int max;
    }

    [Serializable]
    public struct SimplePair<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }
}