using UnityEngine;

public class WorkpieceClamper : MonoBehaviour
{
    private bool isClamped = false;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в зону попала заготовка с Rigidbody и она еще не привязана
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && !isClamped)
        {
            isClamped = true;

            // 1. Отключаем физику, чтобы заготовка не сопротивлялась
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // 2. Отключаем коллайдер, чтобы не было физических конфликтов со столом
            Collider col = other.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }

            // 3. ПРИВЯЗКА К СТОЛУ:
            // Второй аргумент (true) заставляет Unity сохранить исходный мировой масштаб заготовки, 
            // поэтому её размер гарантированно не изменится!
            other.transform.SetParent(transform, true);

            // 4. Аккуратно ставим ровно в центр зоны фиксации и сбрасываем вращение
            other.transform.localPosition = Vector3.zero;
            other.transform.localRotation = Quaternion.identity;

            Debug.Log("Заготовка успешно привязана к столу!");
        }
    }
}