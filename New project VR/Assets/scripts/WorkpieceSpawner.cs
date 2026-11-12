using UnityEngine;

public class WorkpieceSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject workpiecePrefab; // Префаб кубика
    public Transform spawnPoint;       // Точка спавна

    void Update()
    {
        // По-прежнему можно спавнить по клавише F (если нужно)
        if (Input.GetKeyDown(KeyCode.F))
        {
            SpawnNewWorkpiece();
        }
    }

    // Делаем метод публичным, чтобы его видела виртуальная кнопка Unity UI
    public void SpawnNewWorkpiece()
    {
        if (workpiecePrefab == null)
        {
            Debug.LogWarning("Не назначен префаб заготовки!");
            return;
        }

        Vector3 spawnPos = (spawnPoint != null) ? spawnPoint.position : transform.position;
        Quaternion spawnRot = (spawnPoint != null) ? spawnPoint.rotation : Quaternion.identity;

        // Создаем новый кубик
        Instantiate(workpiecePrefab, spawnPos, spawnRot);
        Debug.Log("Заготовка успешно спавнена через UI!");
    }
}