using UnityEngine;

public class testtutorial1 : MonoBehaviour
{
    public tutorial1 t_1;
    void start()
    {
        print("Сообщение от testtutorial1");
        print(t_1.GetInfo());
    }
    void Update()
    {
    
    }  
}
