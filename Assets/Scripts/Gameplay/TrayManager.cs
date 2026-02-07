using UnityEngine;
using System.Collections.Generic;

public class TrayManager : MonoBehaviour
{
    public static TrayManager Instance;

    public int traySize = 3;
    [SerializeField] private List<ConeController> trayCones = new();

    private void Awake()
    {
        Instance = this;
    }

    public void AddConeToTray(ConeController cone)
    {
        if (cone == null) return;

        trayCones.Add(cone);

        if (trayCones.Count >= traySize)
        {
            CheckForDelivery();
        }
    }

    void CheckForDelivery()
    {
        foreach (var cone in trayCones)
        {
            if (cone == null || cone.currentOrder == null || !cone.currentOrder.IsComplete)
            {
                Debug.Log("Tray not ready yet.");
                return;
            }
        }

        // All complete → deliver!
        // Call Deliver with List<ConeController> (DeliveryManager supports this)
        // DeliveryManager.Instance.Deliver(new List<ConeController>(trayCones));
        trayCones.Clear();
    }
}