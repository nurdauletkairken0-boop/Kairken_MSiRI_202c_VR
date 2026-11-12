using UnityEngine;
using UnityEngine.EventSystems; // Обязательно для работы с UI-событиями

public class TableDraggable : MonoBehaviour
{
    [Header("Лимиты движения по оси X (в метрах от старта)")]
    public float minLimitX = -0.3f; 
    public float maxLimitX = 0.3f;  

    [Header("Скорость движения от кнопок")]
    public float buttonMoveSpeed = 0.5f; // Скорость перемещения стола при зажатии кнопки

    private Vector3 startLocalPos;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Camera cam;

    private int moveDirection = 0; // -1 (влево), 1 (вправо), 0 (стоит)

    void Start()
    {
        startLocalPos = transform.localPosition;
        cam = Camera.main;
        if (cam == null) cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        // Если зажата виртуальная кнопка движения, плавно смещаем стол
        if (moveDirection != 0)
        {
            MoveTable(moveDirection * buttonMoveSpeed * Time.deltaTime);
        }
    }

    // Метод для плавного смещения с учетом лимитов
    void MoveTable(float deltaX)
    {
        Vector3 currentLocal = transform.localPosition;
        float newX = currentLocal.x + deltaX;

        // Строго ограничиваем лимитами относительно стартовой позиции
        float clampedX = Mathf.Clamp(newX, startLocalPos.x + minLimitX, startLocalPos.x + maxLimitX);

        transform.localPosition = new Vector3(clampedX, startLocalPos.y, startLocalPos.z);
    }

    // Эти публичные методы мы привяжем к виртуальным кнопкам UI
    public void StartMovingLeft()  { moveDirection = -1; }
    public void StartMovingRight() { moveDirection = 1; }
    public void StopMoving()       { moveDirection = 0; }

    // Перетаскивание мышкой (оставляем как было)
    void OnMouseDown()
    {
        if (cam == null) return;
        screenPoint = cam.WorldToScreenPoint(transform.position);
        offset = transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        if (cam == null || moveDirection != 0) return; // Если двигаем кнопкой, мышь игнорируем

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = cam.ScreenToWorldPoint(curScreenPoint) + offset;

        Vector3 localTarget;
        if (transform.parent != null)
        {
            localTarget = transform.parent.InverseTransformPoint(curPosition);
        }
        else
        {
            localTarget = curPosition;
        }

        float clampedX = Mathf.Clamp(localTarget.x, startLocalPos.x + minLimitX, startLocalPos.x + maxLimitX);

        if (transform.parent != null)
        {
            transform.localPosition = new Vector3(clampedX, startLocalPos.y, startLocalPos.z);
        }
        else
        {
            transform.position = new Vector3(clampedX, startLocalPos.y, startLocalPos.z);
        }
    }
}