using UnityEngine;

public class TableMover : MonoBehaviour
{
    [Header("Настройки движения")]
    public float moveSpeed = 0.5f;       // Скорость движения
    public float maxTravelDistance = 0.8f; // Максимальное расстояние в одну сторону от старта

    private float startX;
    private float currentX;
    private int moveDirection = 0;       // -1 (влево), 0 (стоит), 1 (вправо)

    void Start()
    {
        // Запоминаем исходную позицию стола при старте игры
        startX = transform.localPosition.x;
        currentX = startX;
    }

    void Update()
    {
        if (moveDirection != 0)
        {
            currentX += moveDirection * moveSpeed * Time.deltaTime;
            
            // Ограничиваем движение симметрично относительно стартовой позиции
            float clampedOffset = Mathf.Clamp(currentX - startX, -maxTravelDistance, maxTravelDistance);
            
            Vector3 pos = transform.localPosition;
            pos.x = startX + clampedOffset;
            transform.localPosition = pos;
            
            currentX = pos.x; // Синхронизируем текущую позицию
        }
    }

    public void StartMovingLeft()  { moveDirection = -1; }
    public void StartMovingRight() { moveDirection = 1; }
    public void StopMoving()       { moveDirection = 0; }
}