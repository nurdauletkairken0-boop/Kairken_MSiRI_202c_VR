using UnityEngine;

public class ForceDrop : MonoBehaviour
{
    void Start()
    {
        // Отключаем любые связи с родителями при появлении
        transform.SetParent(null);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Принудительно включаем физику
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.WakeUp(); // Будим физический движок Unity
        }
    }

    void Update()
    {
        // На всякий случай проверяем первые секунды, чтобы скрипты VR не перехватили объект
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && rb.isKinematic)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}