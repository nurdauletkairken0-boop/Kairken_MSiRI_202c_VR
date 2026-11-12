using UnityEngine;

public class DraggableWorkpiece : MonoBehaviour
{
    private Camera cam;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Rigidbody rb;
    private bool isDragging = false;
    private Transform detectedTable = null; // Стол, с которым сейчас соприкасается заготовка

    void Start()
    {
        cam = Camera.main;
        if (cam == null) cam = FindObjectOfType<Camera>();
        
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        if (cam == null) return;

        isDragging = true;
        
        // Отвязываем от стола при начале перемещения мышкой
        transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false; 
            rb.useGravity = true; // Включаем гравитацию при переносе, если нужно
        }

        screenPoint = cam.WorldToScreenPoint(transform.position);
        offset = transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        if (!isDragging || cam == null) return;

        // Отключаем физику во время активного перетаскивания мышкой
        if (rb != null) rb.isKinematic = true;

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = cam.ScreenToWorldPoint(curScreenPoint) + offset;
        
        transform.position = curPosition;
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (rb != null)
        {
            rb.isKinematic = false; // Возвращаем физику
        }

        // Если при отпускании мыши заготовка касается стола — жестко закрепляем её
        if (detectedTable != null)
        {
            AttachToTable(detectedTable);
        }
    }

    // Отслеживаем касание со столом
    void OnCollisionEnter(Collision collision)
    {
        // Проверяем, что коснулись объекта с именем "Table" (или замените на ваш тег/имя)
        if (collision.gameObject.name == "Table" || collision.transform.root.name == "Table")
        {
            detectedTable = collision.transform;

            // Если мы уже отпустили мышь и коснулись стола — сразу прикрепляем
            if (!isDragging)
            {
                AttachToTable(detectedTable);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform == detectedTable)
        {
            detectedTable = null;
        }
    }

    void AttachToTable(Transform table)
    {
        // Делаем дочерним объектом стола, чтобы двигаться вместе с ним
        transform.SetParent(table);

        // Полностью отключаем физику (Rigidbody), чтобы заготовка не падала, не дрожала и не улетала
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = true; // Коллайдер остается активным
        }

        Debug.Log("Заготовка жестко закреплена на столе!");
    }
}