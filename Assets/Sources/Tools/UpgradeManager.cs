using UnityEngine;

namespace Eggland.Tools
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private SimplePair<ResourceType, int>[] pickaxeUpgrades;
        [SerializeField] private SimplePair<ResourceType, int>[] axeUpgrades;
        [SerializeField] private GameObject[] axes;
        [SerializeField] private GameObject[] pickaxes;
        [HideInInspector] public int AxeLevel { get; private set; }
        [HideInInspector] public int PickaxeLevel { get; private set; }

        private ResourceStorage storage;
        private Player player;

        private void Start()
        {
            storage = FindObjectOfType<ResourceStorage>();
            player = FindObjectOfType<Player>();
        }
        
        public GameObject GetAxePrefab(int index) => axes[index];
        public GameObject GetPickaxePrefab(int index) => pickaxes[index];
        public GameObject[] GetPrefabArray(ToolType type) => type == ToolType.AXE ? axes : pickaxes;

        public bool CanUpgrade(int index, ToolType type)
        {
            if (axeUpgrades.Length == index) return false;
            
            var upgrade = type == ToolType.AXE ? axeUpgrades[index] : pickaxeUpgrades[index];
            var storedAmount = storage.Get(upgrade.key);

            return storedAmount >= upgrade.value;
        }

        public void CountUpgrade(int index, ToolType type)
        {
            var upgrade = type == ToolType.AXE ? axeUpgrades[index] : pickaxeUpgrades[index];
            storage.Use(upgrade.key, upgrade.value);

            if (type == ToolType.AXE)
            {
                AxeLevel++;
            }
            else
            {
                PickaxeLevel++;
            }
        }
    }
}