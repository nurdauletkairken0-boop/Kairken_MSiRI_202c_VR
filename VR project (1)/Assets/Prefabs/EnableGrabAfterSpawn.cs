using UnityEngine;

public class EnableGrabAfterSpawn : MonoBehaviour
{
    private bool hasDropped = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasDropped)
        {
            hasDropped = true;

            // Включаем все скрипты взаимодействия, которые есть на объекте
            // Это обходит проблему с недоступными напрямую именами классов
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            foreach (var script in scripts)
            {
                string scriptName = script.GetType().Name;
                if (scriptName == "Interactable" || scriptName == "Throwable")
                {
                    script.enabled = true;
                }
            }

            Debug.Log("Заготовка коснулась поверхности, VR-скрипты включены.");
        }
    }
}