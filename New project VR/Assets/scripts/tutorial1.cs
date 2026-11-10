using UnityEngine;

public class tutorial1 : MonoBehaviour
{
    // ПОЛЯ - состояние
    public string Owner;
    public int Balance;
    
    public int GetInfo(){
        return Balance*7;   //
    }
    private string GetName(){
    return "Пользователь "+Owner;   //
    }
    // МЕТОДЫ - поведение
    public void Deposit(int amount)
    {
        Balance += GetInfo();          // работает с полем СВОЕГО объекта
    }

    public void Withdraw(int amount)
    {
        if (Balance >= amount)
            Balance -= amount;
    }

    void Start()
    {
        Balance = 340;
        print(Owner = "Нур");
        print(GetName());
        print(Balance);
        Deposit(10);
        print(Balance);
    }

    void Update()
    {
        
    }
}
