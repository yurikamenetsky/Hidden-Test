using Data;
using DG.Tweening;
using UnityEngine;

namespace Views.UI
{
    public interface IHiddenItemUiView
    {
        void Init(ItemData item);
        void Destroy(bool immediate = false);
        string ItemName { get; }
    }
    
    public abstract class HiddenItemUiView : MonoBehaviour, IHiddenItemUiView
    {
        protected bool _isDestroying = false;

        private ItemData _item;

        public string ItemName => _item.name;
        
        public virtual void Init(ItemData item)
        {
            gameObject.SetActive(true);
            _item = item;
        }

        public virtual void Destroy(bool immediate = false)
        {
            if (immediate && !_isDestroying)
            {
                DOTween.Kill(gameObject);
                Destroy(gameObject);
            }
            _isDestroying = true;
        }
    }
}
