using UnityEngine;

public class WorkpieceClamper : MonoBehaviour
{
    private Transform clampedObject = null;
    private Rigidbody clampedRb = null;
    private Collider clampedCol = null;

    [Header("Точка на столе, куда ложится деталь")]
    public Transform attachPoint; 

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        
        // Проверяем, что в зону попала заготовка (и она не является уже зафиксированной)
        if (rb != null && other.transform != clampedObject)
        {
            // ЕСЛИ НА СТОЛЕ УЖЕ ЕСТЬ ДЕТАЛЬ — УДАЛЯЕМ ЕЁ
            if (clampedObject != null)
            {
                Debug.Log("Убираем старую заготовку: " + clampedObject.name);
                Destroy(clampedObject.gameObject);
            }

            // Запоминаем новую заготовку
            clampedObject = other.transform;
            clampedRb = rb;
            clampedCol = other.GetComponent<Collider>();

            // 1. Отключаем физику новой детали
            clampedRb.isKinematic = true;
            clampedRb.useGravity = false;
            clampedRb.linearVelocity = Vector3.zero;
            clampedRb.angularVelocity = Vector3.zero;

            // 2. Отключаем коллайдер
            if (clampedCol != null) clampedCol.enabled = false;

            // 3. Ставим на место привязки
            UpdatePosition();

            Debug.Log("Новая заготовка установлена на стол!");
        }
    }

    private void LateUpdate()
    {
        // Двигаем текущую заготовку вместе со столом
        if (clampedObject != null && attachPoint != null)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        clampedObject.position = attachPoint.position;
        clampedObject.rotation = attachPoint.rotation;
    }
}