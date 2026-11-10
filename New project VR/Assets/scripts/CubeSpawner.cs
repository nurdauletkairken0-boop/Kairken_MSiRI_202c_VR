using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    private string[] cubee = { "Куб 0", "Куб 1", "Куб 2", "Куб 3", "Куб 4", "Куб 5" };
    private int currentIndex = 0; // Индекс для отслеживания текущего куба из массива

    [Header("Настройки спавна")]
    [Tooltip("Точка, в которой будут появляться заготовки")]
    public Transform spawnPoint; // Ссылка на пустой объект-точку спавна

    void Update()
    {
        // Проверяем нажатие клавиши G
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Если мы еще не вышли за пределы массива
            if (currentIndex < cubee.Length)
            {
                int i = currentIndex;

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = cubee[i];

                Rigidbody rb = cube.AddComponent<Rigidbody>();
                rb.useGravity = false;

                // Закомментировано, если скрипт столкновения еще не создан:
                // cube.AddComponent<stolknovenie>();

                // Если точка спавна задана — используем её позицию и поворот, иначе спавним в начале координат
                if (spawnPoint != null)
                {
                    cube.transform.position = spawnPoint.position;
                    cube.transform.rotation = spawnPoint.rotation;
                }
                else
                {
                    // Запасной вариант, если точка не перетащена в инспектор
                    float spacing = 2f;
                    cube.transform.position = new Vector3(i * spacing, 0, 0);
                }

                // Случайный цвет куба
                cube.GetComponent<Renderer>().material.color = Random.ColorHSV();

                currentIndex++; // Переходим к следующему имени
            }
            else
            {
                Debug.Log("Все кубы из массива уже созданы!");
            }
        }
    }
}
