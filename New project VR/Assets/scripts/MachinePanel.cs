using TMPro;
using UnityEngine;

public class MachinePanel : MonoBehaviour
{
    public TMP_Text stateText, axesText, messageText;

    // Метод для обновления состояния и координат станка
    public void ShowState(MachineState data)
    {
        stateText.text = data.running ? "Работает" : "Остановлен";
        axesText.text = $"X: {data.x:F1} мм\n" +
                        $"Y: {data.y:F1} мм\n" +
                        $"Z: {data.z:F1} мм";
    }

    // Метод для вывода текстовых сообщений/ошибок
    public void ShowMessage(string message)
    {
        messageText.text = message;
    }
}