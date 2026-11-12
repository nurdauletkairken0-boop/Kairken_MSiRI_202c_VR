using UnityEngine;
using System.Collections;

public class SpindleController : MonoBehaviour
{
    [Header("Настройки перемещения фрезы")]
    public float downOffset = 0.2f;   // На какое расстояние опускается фреза
    public float speed = 2f;          // Скорость движения
    public float holdTime = 0.5f;     // Время задержки в нижнем положении

    private Vector3 startPosition;
    private Coroutine currentCycle;   // Ссылка на текущую корутину движения
    private bool isRunning = false;

    void Start()
    {
        // Запоминаем исходное (верхнее) положение фрезы
        startPosition = transform.localPosition;
    }

    // Запуск цикла кнопкой «Пуск»
    public void StartCycle()
    {
        if (!isRunning)
        {
            currentCycle = StartCoroutine(CycleRoutine());
        }
    }

    // Экстренная остановка кнопкой «Стоп»
    public void StopCycle()
    {
        if (isRunning)
        {
            // Останавливаем текущую корутину движения
            if (currentCycle != null)
            {
                StopCoroutine(currentCycle);
            }

            // Плавный или моментальный возврат наверх
            StartCoroutine(ReturnToStartImmediate());
        }
    }

    private IEnumerator CycleRoutine()
    {
        isRunning = true;
        Vector3 targetPosition = startPosition - new Vector3(0, downOffset, 0);

        // 1. Опускание вниз
        yield return Move(targetPosition);

        // 2. Пауза внизу
        yield return new WaitForSeconds(holdTime);

        // 3. Подъем обратно наверх
        yield return Move(startPosition);

        isRunning = false;
    }

    private IEnumerator Move(Vector3 target)
    {
        while (Vector3.Distance(transform.localPosition, target) > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);
            yield return null;
        }
        transform.localPosition = target;
    }

    // Возврат фрезы наверх при нажатии «Стоп»
    private IEnumerator ReturnToStartImmediate()
    {
        while (Vector3.Distance(transform.localPosition, startPosition) > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPosition, speed * 1.5f * Time.deltaTime);
            yield return null;
        }
        transform.localPosition = startPosition;
        isRunning = false;
    }
}