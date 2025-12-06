using UnityEngine;
using UnityEngine.EventSystems;

namespace Views
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class HiddenItemView : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            
        }
    }
}
