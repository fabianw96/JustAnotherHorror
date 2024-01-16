using UnityEngine;

namespace Interfaces
{
    public interface IInteractable
    {
        void Interaction()
        {
            Debug.Log(this);
        }
    }
}