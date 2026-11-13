using System.Collections;
using UnityEngine;

public class SpindleController : MonoBehaviour
{
    [Header("Настройки движения фрезы")]
    public Transform spindle;       // Сама фреза (или шпиндель)
    public float minY = 0.0f;       // Нижняя точка погружения (локальная или мировая Y)
    public float maxY = 0.5f;       // Верхняя исходная точка
    public float speed = 2.0f;      // Скорость движения

    private bool isRunning = false;
    private Coroutine millingRoutine;

    // Этот метод вешаем на зеленую кнопку «Старт» (OnClick)
    public void StartMilling()
    {
        if (!isRunning)
        {
            isRunning = true;
            if (millingRoutine != null) StopCoroutine(millingRoutine);
            millingRoutine = StartCoroutine(MillingLoop());
            Debug.Log("Фреза запущена: циклическое фрезерование.");
        }
    }

    // Этот метод вешаем на красную кнопку «Стоп» (OnClick)
    public void StopMilling()
    {
        isRunning = false;
        if (millingRoutine != null)
        {
            StopCoroutine(millingRoutine);
        }
        Debug.Log("Фреза остановлена.");
    }

    private IEnumerator MillingLoop()
    {
        // Бесконечный цикл, пока isRunning == true
        while (isRunning)
        {
            // 1. Опускаемся вниз до minY
            yield return MoveToY(minY);

            // Небольшая пауза внизу (опционально, можно убрать)
            yield return new WaitForSeconds(0.2f);

            // 2. Поднимаемся вверх до maxY
            yield return MoveToY(maxY);

            // Небольшая пауза вверху
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator V(float targetY)
    {
        Vector3 pos = spindle.localPosition;
        // Если используете мировые координаты, поменяйте localPosition на position, а y на targetY в мировом пространстве
        while (Mathf.Abs(spindle.localPosition.y - targetY) > 0.001f)
        {
            if (!isRunning) yield break;

            float newY = Mathf.MoveTowards(spindle.localPosition.y, targetY, speed * Time.deltaTime);
            spindle.localPosition = new Vector3(spindle.localPosition.x, newY, spindle.localPosition.z);
            
            yield return null; // ждем следующий кадр
        }
    }
    
    // Вспомогательный метод движения
    private IEnumerator MoveToY(float targetY)
    {
        while (Mathf.Abs(spindle.localPosition.y - targetY) > 0.001f)
        {
            if (!isRunning) yield break;

            float newY = Mathf.MoveTowards(spindle.localPosition.y, targetY, speed * Time.deltaTime);
            spindle.localPosition = new Vector3(spindle.localPosition.x, newY, spindle.localPosition.z);
            
            yield return null;
        }
    }
}