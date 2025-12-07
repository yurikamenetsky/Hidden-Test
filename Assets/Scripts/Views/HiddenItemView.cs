using System;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Views
{
    public interface IHiddenItemView
    {
        event Action<ItemData> Clicked;
        event Action<ItemData> Collected;

        void Initialize(ItemData data);
        void Enable(bool enable);
        void Collect();
        bool IsName(string name);
        
        GameObject View { get; }
    }
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PolygonCollider2D))]
    public class HiddenItemView : MonoBehaviour, IPointerClickHandler, IHiddenItemView
    {
        public event Action<ItemData> Clicked;
        public event Action<ItemData> Collected;

        private Collider2D _collider;
        private SpriteRenderer _renderer;
        
        private ItemData _itemData;
        private bool _enabled;

        public GameObject View => gameObject;
        
        public void Initialize(ItemData data)
        {
            _itemData = data;
            _collider = GetComponentInChildren<Collider2D>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_enabled)
                return;
            
            Clicked?.Invoke(_itemData);
        }

        public bool IsName(string itemMame)
        {
            return _itemData?.name == itemMame;
        }
        
        public void Enable(bool enable)
        {
            _enabled = enable;
        }

        public void Collect()
        {
            _collider.enabled = false;
            _renderer.DOFade(0f, 1f).OnComplete(() => Collected?.Invoke(_itemData));
        }
    }
}
