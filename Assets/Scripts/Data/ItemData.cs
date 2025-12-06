using System;
using UnityEngine;
using Views;

namespace Data
{
    //[CreateAssetMenu(fileName = "New Item", menuName = "Hidden Object/Item Data")]
    [Serializable]
    public class ItemData
    {
        public string name;
        public Sprite itemSprite;
        public Sprite uiIcon;
        public Vector3 position;
        public HiddenItemView prefab;
    }
}
