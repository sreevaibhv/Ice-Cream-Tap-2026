using UnityEngine;

public class OrderBubbleManager : MonoBehaviour
{
    public static OrderBubbleManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateBubble(ConeController cone)
    {
        // TODO: Visual feedback (icon update)
        Debug.Log("Order bubble updated.");
    }
}
