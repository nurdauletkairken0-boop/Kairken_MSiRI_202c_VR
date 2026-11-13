using UnityEngine;

public class WorkpieceSpawner : MonoBehaviour
{
    public GameObject workpiecePrefab; // Ссылка на ваш новый чистый префаб "zag"
    public Transform spawnPoint;       // Точка спавна в воздухе над столом

    public void SpawnWorkpiece()
    {
        if (workpiecePrefab != null && spawnPoint != null)
        {
            // 1. Создаем заготовку в точке спавна
            GameObject newObj = Instantiate(workpiecePrefab, spawnPoint.position, spawnPoint.rotation);
            
            // 2. Убираем любые возможные родительские связи
            newObj.transform.SetParent(null);

            // 3. Принудительно активируем физику и гравитацию
            Rigidbody rb = newObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.WakeUp();
            }
        }
        else
        {
            Debug.LogWarning("Не задан префаб заготовки или точка спавна!");
        }
    }
}