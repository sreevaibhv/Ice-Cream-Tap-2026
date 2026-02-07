using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public int coins = 0;

    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log("Coins: " + coins);
        // Optionally update UI here
    }
}
