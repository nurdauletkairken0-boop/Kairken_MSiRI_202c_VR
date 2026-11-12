using UnityEngine;

public class WorkpieceClamper : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в зону попал объект с Rigidbody (наша заготовка)
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && other.transform.parent != transform)
        {
            // 1. Отключаем физику и гравитацию
            rb.isKinematic = true;
            rb.useGravity = false;

            // 2. Делаем дочерним объектом стола
            other.transform.SetParent(transform);

            // 3. Выравниваем позицию аккуратно на столе (опционально)
            // other.transform.localPosition = new Vector3(0, 0.1f, 0); 
            // other.transform.localRotation = Quaternion.identity;

            Debug.Log("Заготовка успешно зафиксирована на столе: " + other.name);
        }
    }
}