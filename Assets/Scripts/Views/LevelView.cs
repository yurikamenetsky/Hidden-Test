using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Views
{
    public interface ILevelView
    {
        void Initialize(Action<ItemData> onItemClicked, Action<ItemData> onItemCollected);
        void Destroy();
        void UpdateItems(List<ItemData> items);
        void Collect(ItemData item);
    }
    
    public class LevelView : MonoBehaviour, ILevelView
    {
        private GameObject _background;
        private LevelData _levelData;
        private IObjectResolver _resolver;
        
        private readonly List<IHiddenItemView> _itemViews = new ();
        
        private Transform _transform;
        
        [Inject]
        public void Construct(IObjectResolver resolver, LevelData data)
        {
            _transform = transform;
            _levelData = data;
            _resolver = resolver;
        }

        public void Initialize(Action<ItemData> onItemClicked, Action<ItemData> onItemCollected)
        {
            Clear();

            _background = new GameObject("Background");
            _background.transform.SetParent(_transform);
            var spriteRenderer = _background.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = _levelData.backgroundSprite;

            foreach (var item in _levelData.items)
            {
                var itemGo = _resolver.Instantiate(item.prefab, _transform);
                itemGo.transform.position = new Vector3(item.position.x, item.position.y, -1);
                var view = itemGo.GetComponent<IHiddenItemView>();
                _itemViews.Add(view);
                view.Initialize(item);
                view.Clicked += onItemClicked;
                view.Collected += onItemCollected;
            }
        }

        public void Destroy()
        {
            Clear();
        }

        public void UpdateItems(List<ItemData> items)
        {
            foreach (var it in _itemViews)
            {
                it.Enable(items.Exists(x => it.IsName(x.name)));
            }
        }

        public void Collect(ItemData item)
        {
            foreach (var it in _itemViews)
            {
                if (it.IsName(item.name))
                {
                    it.Collect();
                    return;
                }
            }
        }

        private void Clear()
        {
            _itemViews.ForEach(x =>
            {
                Destroy(x.View);
            });
            _itemViews.Clear();
            Destroy(_background);
        }
    }
}
