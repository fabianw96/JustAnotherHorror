using Interfaces;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerInteraction
    {
        private const float RaycastDistance = 5f;
     
        public void Interact()
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Camera.main != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, RaycastDistance);

            if (!hit) return;
            GameObject hitObject = hitInfo.transform.gameObject;
            if (hitObject != null && hitObject.GetComponent<IInteractable>() != null)
            {
                hitObject.GetComponent<IInteractable>().Interaction();
            }
        }
    }
}
