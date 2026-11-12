using UnityEngine;
using Valve.VR.InteractionSystem;

public class TableSlot : MonoBehaviour
{
    public Transform tableTransform; // Ссылка на стол

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Workpiece") || other.name.Contains("Workpiece"))
        {
            Interactable interactable = other.GetComponent<Interactable>();

            // Проверяем, что компонент есть и объект НЕ привязан к руке (attachedToHand == null)
            if (interactable != null && interactable.attachedToHand == null)
            {
                other.transform.SetParent(tableTransform);
                
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // Фиксируем заготовку на столе
                }
            }
        }
    }
}