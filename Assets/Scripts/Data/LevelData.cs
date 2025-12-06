using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Hidden Object/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Base Settings")]
        public string levelId;
        public Sprite backgroundSprite;

        [Header("Items Settings")]
        public List<ItemData> items;
        public List<bool> itemsEnabled;
    
        [Header("UI Settings")]
        public bool useImagesInsteadOfText = false;
    
        [Header("Timer Settings")]
        public bool timerEnabled = true;
        public float timerDuration = 120f;
    
        [Header("Items Order")]
        public List<int> itemOrder;
    
        public List<ItemData> GetActiveItemsInOrder()
        {
            List<ItemData> result = new List<ItemData>();
        
            foreach (int index in itemOrder)
            {
                if (index < items.Count && index < itemsEnabled.Count && itemsEnabled[index])
                {
                    result.Add(items[index]);
                }
            }
        
            return result;
        }
    }
}
