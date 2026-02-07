using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [Header("Cone Spawning")]
    public GameObject conePrefab;
    public Transform coneSpawnPoint;

    [System.Serializable]
    public class ConeOrder
    {
        public List<FlavorType> requiredFlavors = new();
        public bool IsComplete => requiredFlavors.Count == 0;
    }

    [Header("Order Settings")]
    public List<ConeOrder> orders = new();
    public ConeController currentCone;   // <-- Required for pipe clicking
    private int currentOrderIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Spawn first cone with 1–3 scoops
        SpawnNewCone(Random.Range(1, 4));
    }

    // ==========================================================
    //  SPAWN A NEW CONE + CREATE ORDER
    // ==========================================================
    public void SpawnNewCone(int scoopCount)
    {
        if (conePrefab == null || coneSpawnPoint == null)
        {
            Debug.LogError("❌ Cone Prefab or Spawn Point missing on OrderManager!");
            return;
        }

        // Spawn cone
        GameObject newCone = Instantiate(conePrefab, coneSpawnPoint.position, Quaternion.identity);

        // ✅ ADD TO CONVEYOR CORRECTLY
        ConveyorController.Instance.EnqueueFinishedCone(newCone);

        // Get cone script
        ConeController cone = newCone.GetComponent<ConeController>();
        if (cone == null)
        {
            Debug.LogError("❌ Cone Prefab missing ConeController script!");
            return;
        }

        currentCone = cone;

        // Create order
        ConeOrder order = CreateOrder(scoopCount);
        cone.currentOrder = order;
        cone.maxScoops = scoopCount;

        // Update UI bubble
        if (OrderBubbleManager.Instance != null)
            OrderBubbleManager.Instance.UpdateBubble(cone);

        Debug.Log("⭐ Spawned new cone with order: " + scoopCount + " scoops");
    }

    // ==========================================================
    //  CREATE ORDER WITH RANDOM FLAVORS
    // ==========================================================
    public ConeOrder CreateOrder(int scoopCount)
    {
        ConeOrder order = new ConeOrder();

        for (int i = 0; i < scoopCount; i++)
        {
            order.requiredFlavors.Add(GetRandomFlavor());
        }

        //activeOrders.Add(order);
        return order;
    }

    // ==========================================================
    //  WHEN PLAYER ADDS A SCOOP
    // ==========================================================
    public void ScoopAdded(ConeController cone, FlavorType flavor)
    {
        var order = cone.currentOrder;

        if (order == null) return;
        if (order.requiredFlavors.Count == 0) return;

        // Correct scoop
        if (order.requiredFlavors[0] == flavor)
        {
            order.requiredFlavors.RemoveAt(0);

            // Finished the order
            if (order.requiredFlavors.Count == 0)
            {
                Debug.Log("✔ Order Completed!");
            }
        }
        else
        {
            GameManager.Instance.FailGame("Wrong Flavor!");
        }
    }

    // ==========================================================
    //  RANDOM FLAVOR PICKER
    // ==========================================================
    FlavorType GetRandomFlavor()
    {
        return (FlavorType)Random.Range(0, 4); // Vanilla, Chocolate, Strawberry, Mint
    }
}
