using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private Camera cam;

    void Start()
    {
        // Ищем камеру безопасным способом
        cam = Camera.main;
        if (cam == null && FindObjectOfType<Camera>() != null)
        {
            cam = FindObjectOfType<Camera>();
        }
    }

    void OnMouseDown()
    {
        if (cam == null) return;
        screenPoint = cam.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        if (cam == null) return;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = cam.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }
}