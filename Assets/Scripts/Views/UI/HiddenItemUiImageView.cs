using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI
{
    public class HiddenItemUiImageView : HiddenItemUiView
    {
        [SerializeField] private Image icon;
        
        public override void Init(ItemData item)
        {
            base.Init(item);
            icon.sprite = item.uiIcon;
        }

        public override void Destroy(bool immediate = false)
        {
            if  (_isDestroying)
                return;
            
            base.Destroy(immediate);

            icon.DOFade(0f, 1f).OnComplete(
                () => GameObject.Destroy(gameObject)
                );
        }
    }
}
