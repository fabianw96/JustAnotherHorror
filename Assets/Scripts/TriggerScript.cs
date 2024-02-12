using ScriptableObjects.Events;
using UnityEngine;


public class TriggerScript : MonoBehaviour
{
    [SerializeField] private ScriptableEvent triggerEvent;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            triggerEvent.RaiseEvent();
            this.gameObject.SetActive(false);
        }
    }
}