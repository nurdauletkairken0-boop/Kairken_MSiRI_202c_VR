using System;
using System.Collections;
using System.Globalization;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

// Простая структура для десериализации состояния станка
[Serializable]
public struct MachineState
{
    public bool running;
    public float x;
    public float y;
    public float z;
}

public class MachineClient : MonoBehaviour
{
    [Header("Network Settings")]
    public string getUrl = "https://cnc.kovalev.team/get/3";
    public string postUrl = "https://cnc.kovalev.team/post";
    public int channel = 1;
    public float interval = 1f;

    [Header("UI Reference")]
    public MachinePanel panel;

    [Header("Machine Nodes")]
    public Transform nodeX;
    public Transform nodeY;
    public Transform nodeZ;
    
    private Vector3 startX;
    private Vector3 startY;
    private Vector3 startZ;

    // Флаг ручной остановки станка
    private bool isManuallyStopped = false;

    private IEnumerator Start()
    {
        if (nodeX == null || nodeY == null || nodeZ == null)
        {
            Debug.LogError("Назначьте три узла станка в Inspector.");
            yield break;
        }

        startX = nodeX.localPosition;
        startY = nodeY.localPosition;
        startZ = nodeZ.localPosition;

        // Цикл опроса сервера
        while (true)
        {
            // Если станок остановлен вручную, пропускаем GET-запросы, чтобы сервер не перебивал статус
            if (!isManuallyStopped)
            {
                using (UnityWebRequest request = UnityWebRequest.Get(getUrl))
                {
                    request.timeout = 10;
                    yield return request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        try
                        {
                            ProcessData(request.downloadHandler.text);
                            if (panel != null) panel.ShowMessage(""); // Очищаем ошибку связи при успехе
                        }
                        catch (Exception error)
                        {
                            Debug.LogWarning("Ошибка чтения JSON: " + error.Message);
                            if (panel != null) panel.ShowMessage("Ошибка формата данных");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Ошибка запроса: " + request.error);
                        if (panel != null) panel.ShowMessage("Нет связи со станком");
                    }
                }
            }

            yield return new WaitForSecondsRealtime(Mathf.Max(0.1f, interval));
        }
    }

    private void ProcessData(string json)
    {
        JObject data = JObject.Parse(json);
        float x = GetCoordinate(data, "X");
        float y = GetCoordinate(data, "Y");
        float z = GetCoordinate(data, "Z");

        // Движение 3D моделей в сцене
        nodeX.localPosition = startX + Vector3.right * (x * 0.001f);
        nodeY.localPosition = startY + Vector3.forward * (y * 0.001f);
        nodeZ.localPosition = startZ + Vector3.up * (z * 0.001f);

        // Передача данных на VR-панель
        if (panel != null)
        {
            MachineState stateData = new MachineState
            {
                running = !isManuallyStopped, // Учитываем флаг остановки
                x = x,
                y = y,
                z = z
            };
            panel.ShowState(stateData);
        }
    }

    private float GetCoordinate(JObject data, string axis)
    {
        foreach (JToken section in data["data"])
        {
            if ((string)section[0] != "Оси Канала " + channel)
                continue;

            foreach (JToken row in section[1])
            {
                if ((string)row[0] == "Перемещение по оси " + axis)
                {
                    string text = ((string)row[1]).Replace("мм", "").Trim();
                    text = text.Replace(',', '.');
                    float value = float.Parse(text, CultureInfo.InvariantCulture);

                    if (float.IsNaN(value) || float.IsInfinity(value))
                        throw new FormatException("Некорректная координата " + axis);

                    return value;
                }
            }
        }
        throw new FormatException("Не найдена координата " + axis);
    }

    // Методы управления кнопками «Пуск» и «Стоп»
    public void StartMachine()
    {
        isManuallyStopped = false; // Снимаем блокировку, возобновляем опрос
        StartCoroutine(SendCommand("start"));
    }

    public void StopMachine()
    {
        isManuallyStopped = true; // Блокируем GET-опрос и переводим статус в false

        // Мгновенно отображаем остановку на панели
        if (panel != null)
        {
            MachineState stoppedState = new MachineState { running = false, x = 0, y = 0, z = 0 };
            panel.ShowState(stoppedState);
        }

        StartCoroutine(SendCommand("stop"));
    }

    private IEnumerator SendCommand(string command)
    {
        if (panel != null) panel.ShowMessage("Отправка команды...");

        WWWForm form = new WWWForm();
        form.AddField("command", command);

        using (UnityWebRequest req = UnityWebRequest.Post(postUrl, form))
        {
            req.timeout = 10;
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                if (panel != null) panel.ShowMessage("Ошибка команды: " + req.error);
                yield break;
            }

            if (panel != null) panel.ShowMessage("Команда принята. Ожидание состояния...");
        }
    }
}