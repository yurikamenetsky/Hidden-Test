using System;
using UnityEngine;
using Views;

namespace Data
{
    [Serializable]
    public class ItemData
    {
        public string name;
        public Sprite itemSprite;
        public Sprite uiIcon;
        public Vector3 position;
        public HiddenItemView prefab;
        public bool isEnable = true;
    }
}
