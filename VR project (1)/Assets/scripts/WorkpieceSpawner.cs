using UnityEngine;

public class WorkpieceSpawner : MonoBehaviour
{
    public GameObject workpiecePrefab; // Ссылка на префаб заготовки
    public Transform spawnPoint;       // Точка, где появится заготовка

    // Этот метод мы привяжем к виртуальной кнопке UI
    public void SpawnWorkpiece()
    {
        if (workpiecePrefab != null && spawnPoint != null)
        {
            // Создаем копию префаба в позиции и с поворотом точки спавна
            Instantiate(workpiecePrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Не задан префаб заготовки или точка спавна!");
        }
    }
}