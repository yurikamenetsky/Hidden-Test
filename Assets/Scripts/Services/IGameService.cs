using System;
using System.Collections.Generic;
using Data;

namespace Services
{
    public interface IGameService
    {
        void Initialize();
    }

    public interface IUIService : IGameService
    {
        event Action GameStarted;
        
        void UpdateItemListUI(List<ItemData> items);
        void RemoveItemFromUI(ItemData item);
        void UpdateTimer(string time);
        void ShowWinScreen();
        void ShowLoseScreen();
    }

    public interface IInputService : IGameService
    {
        void Enable();
        void Disable();
    }
}
