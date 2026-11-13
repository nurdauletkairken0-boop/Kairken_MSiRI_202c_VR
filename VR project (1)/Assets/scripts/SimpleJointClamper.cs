using UnityEngine;

public class SimpleJointClamper : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, что на стол упала заготовка (ищем по тегу или имени)
        if (collision.gameObject.name.Contains("zag") || collision.gameObject.CompareTag("Workpiece"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            FixedJoint joint = collision.gameObject.GetComponent<FixedJoint>();

            // Если у детали есть сустав и он еще не привязан к столу
            if (joint != null && joint.connectedBody == null)
            {
                // Подключаем жесткую связь к нашему столу
                joint.connectedBody = GetComponent<Rigidbody>();
                
                Debug.Log("Заготовка физически приварена к столу через FixedJoint!");
            }
        }
    }
}