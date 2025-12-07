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

        [field: SerializeField] public int MaxItemsCount { get; private set; } = 3;

        [Header("UI Settings")]
        [field: SerializeField] public bool UseImagesInsteadOfText { get; private set; } = true;
    
        [Header("Timer Settings")]
        [field: SerializeField] public bool TimerEnabled { get; private set; } = true;
        [field: SerializeField] public float TimerDuration { get; private set; } = 120f;

        public List<ItemData> GetActiveItems() => items.FindAll(x => x.isEnable);
    }
}
