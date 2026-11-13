using UnityEngine;

public class ForceGravity : MonoBehaviour
{
    void Start()
    {
        // Отвязываем от возможных родителей
        transform.SetParent(null);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.WakeUp(); // «Будим» физику
        }
    }
}