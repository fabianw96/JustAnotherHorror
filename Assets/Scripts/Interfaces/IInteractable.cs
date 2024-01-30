using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interfaces
{
    public interface IInteractable: IPointerEnterHandler
    {
        void Interaction()
        {
            Debug.Log(this);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Entered object");
        }
    }
}