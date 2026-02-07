using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;

    [Header("Customer Setup")]
    public GameObject customerPrefab;
    public Transform customerSpawnPoint;

    public Transform customerReceivePoint;

    public Transform customerExitPoint;

    [Header("Queue Settings")]
    public int maxQueue = 3;
    public float queueSpacing = 0.6f;

    private Queue<CustomerController> customerQueue = new();
    private CustomerController activeCustomer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < maxQueue; i++)
            SpawnCustomer();

        MoveNextCustomerToReceivePoint();
    }

    // ================= SPAWNING =================
    void SpawnCustomer()
    {
        if (customerQueue.Count >= maxQueue) return;

        Vector3 spawnPos =
            customerSpawnPoint.position +
            Vector3.back * queueSpacing * customerQueue.Count;

        GameObject go = Instantiate(customerPrefab, spawnPos, Quaternion.identity);
        CustomerController customer = go.GetComponent<CustomerController>();

        customer.SetPositions(
    customerReceivePoint.position,
    customerExitPoint.position
);

        customerQueue.Enqueue(customer);
    }

    // ================= QUEUE FLOW =================
    public void MoveNextCustomerToReceivePoint()
    {
        if (activeCustomer != null) return;
        if (customerQueue.Count == 0) return;

        activeCustomer = customerQueue.Dequeue();
        activeCustomer.MoveToReceivePoint();

        RepositionQueue();
    }

    void RepositionQueue()
    {
        int index = 0;
        foreach (var customer in customerQueue)
        {
            Vector3 pos =
                customerSpawnPoint.position +
                Vector3.back * queueSpacing * index;

            customer.MoveToQueuePosition(pos);
            index++;
        }
    }

    // ================= DELIVERY =================
    public void DeliverCone(GameObject cone)
    {
        if (activeCustomer == null || cone == null) return;

        activeCustomer.ReceiveCone(cone, () =>
        {
            activeCustomer = null;

            SpawnCustomer();
            MoveNextCustomerToReceivePoint();



            // Optional: spawn next cone here
            OrderManager.Instance.SpawnNewCone(Random.Range(1, 4));
        });
    }
}
