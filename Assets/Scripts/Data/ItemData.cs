using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class ItemData
    {
        public string name;
        public Sprite itemSprite;
        public Sprite uiIcon;
        public Vector3 position;
        public GameObject prefab;
        public bool isEnable = true;
    }
}
