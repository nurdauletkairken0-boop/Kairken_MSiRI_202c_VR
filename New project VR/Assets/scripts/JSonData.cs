using System;
using System.Collections;
using System.Globalization;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class JSonData : MonoBehaviour
{
    public string url = "https://cnc.kovalev.team/get/3";
    public int channel = 1;
    public float interval = 1f;

    [Header("UI Reference")]
    public MachinePanel panel;

    public Transform nodeX;
    public Transform nodeY;
    public Transform nodeZ;
    
    private Vector3 startX;
    private Vector3 startY;
    private Vector3 startZ;

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

        while (true)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.timeout = 1;
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        MoveNodes(request.downloadHandler.text);
                        if (panel != null) panel.ShowMessage("");
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

            yield return new WaitForSecondsRealtime(Mathf.Max(0.1f, interval));
        }
    }

    private void MoveNodes(string json)
    {
        JObject data = JObject.Parse(json);
        float x = GetCoordinate(data, "X");
        float y = GetCoordinate(data, "Y");
        float z = GetCoordinate(data, "Z");

        nodeX.localPosition = startX + Vector3.right * (x * 0.001f);
        nodeY.localPosition = startY + Vector3.forward * (y * 0.001f);
        nodeZ.localPosition = startZ + Vector3.up * (z * 0.001f);

        if (panel != null)
        {
            MachineState stateData = new MachineState
            {
                running = true,
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
}