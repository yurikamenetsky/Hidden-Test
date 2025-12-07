using Data;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Views.UI
{
    public class HiddenItemUiTextView : HiddenItemUiView
    {
        [SerializeField] private TextMeshProUGUI nameText;
        
        public override void Init(ItemData item)
        {
            base.Init(item);
            nameText.text = item.name;
        }

        public override void Destroy(bool immediate = false)
        {
            if  (_isDestroying)
                return;
            
            base.Destroy(immediate);

            nameText.DOFade(0f, 1f).OnComplete(() => GameObject.Destroy(gameObject));
        }
    }
}
