using UnityEngine;

public class StrictTableMover : MonoBehaviour
{
    [Header("Лимиты движения по оси X")]
    public float minX = -1.0f; // Левая граница
    public float maxX = 1.0f;  // Правая граница

    private float currentX;

    void Start()
    {
        // Запоминаем стартовую позицию
        currentX = transform.localPosition.x;
    }

    // Метод для перемещения (вызывается из кнопок или скриптов)
    public void Move(float direction, float speed)
    {
        currentX += direction * speed * Time.deltaTime;
        // Жестко ограничиваем координаты в пределах min и max
        currentX = Mathf.Clamp(currentX, minX, maxX);

        // Применяем новую позицию, сохраняя Y и Z
        transform.localPosition = new Vector3(currentX, transform.localPosition.y, transform.localPosition.z);
    }
}