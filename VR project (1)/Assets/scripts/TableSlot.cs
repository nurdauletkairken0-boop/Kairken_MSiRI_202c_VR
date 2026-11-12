using UnityEngine;
using Valve.VR.InteractionSystem;

public class TableSlot : MonoBehaviour
{
    public Transform tableTransform; // Ссылка на стол или точку фиксации

    private void OnTriggerStay(Collider other)
    {
        // Проверяем, что это наша заготовка и она отпущена рукой (не удерживается)
        if (other.CompareTag("Workpiece"))
        {
            Interactable interactable = other.GetComponent<Interactable>();

            if (interactable != null && interactable.attachedToHand == null)
            {
                // Привязываем к столу и фиксируем физику
                other.transform.SetParent(tableTransform);
                
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; 
                }
            }
        }
    }
}