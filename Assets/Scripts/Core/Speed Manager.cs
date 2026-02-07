using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public static SpeedManager Instance;

    public float baseSpeed = 1f;
    public float speedIncreasePerDelivery = 0.2f;
    public float maxSpeed = 3.5f;

    private void Awake()
    {
        Instance = this;
    }

    public float GetCurrentSpeed(int deliveriesDone)
    {
        return Mathf.Min(
            baseSpeed + (deliveriesDone * speedIncreasePerDelivery),
            maxSpeed
        );
    }
}

