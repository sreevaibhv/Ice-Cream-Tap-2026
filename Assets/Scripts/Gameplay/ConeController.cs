using UnityEngine;
using System;
using System.Collections.Generic;

public class ConeController : MonoBehaviour
{
    [Header("Order")]
    public OrderManager.ConeOrder currentOrder;
    public bool addedToTray = false;
    public float trayTriggerX = 5f;

    [Header("Settings")]
    public int maxScoops = 3;
    private List<FlavorType> scoops = new();

    [Header("Visual Setup")]
    public Transform scoopParent;
    public GameObject scoopVisualPrefab;

    [Header("Materials")]
    public Material vanillaMat;
    public Material chocolateMat;
    public Material strawberryMat;
    public Material mintMat;

    // 🔵 NEW — event for pipe system (randomize pipe color each scoop)
    public static event Action<ConeController, FlavorType> OnScoopPlaced;

    private bool isFinished = false;

    // =============== PUBLIC API ===============
    public void AddFlavor(FlavorType flavor, float amount)
    {
        TryAddScoop(flavor);
    }

    internal void AddScoop(FlavorType flavor)
    {
        TryAddScoop(flavor);
    }

    // =============== CORE LOGIC ===============
    public void TryAddScoop(FlavorType flavor)
    {
        Debug.Log("TryAddScoop Called with: " + flavor);

        if (isFinished) return;

        if (scoops.Count >= maxScoops)
        {
            GameManager.Instance.FailGame("Overflow");
            return;
        }

        scoops.Add(flavor);

        // 🔵 Notify order system
        OrderManager.Instance.ScoopAdded(this, flavor);

        // 🔵 Update order UI bubble
        OrderBubbleManager.Instance.UpdateBubble(this);

        // 🔵 Create scoop visual
        CreateScoopVisual(flavor);

        // 🔵 Notify pipe system → randomize pipes if needed
        OnScoopPlaced?.Invoke(this, flavor);

        // 🔵 If full → send to conveyor
        if (scoops.Count >= maxScoops)
        {
            HandleCompletedCone();
        }

    }

    // =============== VISUAL CREATION ===============
    private void CreateScoopVisual(FlavorType flavor)
    {
        GameObject scoopObj = Instantiate(scoopVisualPrefab, scoopParent);
        //ConveyorController.Instance?.EnqueueFinishedCone(scoopObj);
        float yOffset = 0.35f * (scoops.Count - 1);
        scoopObj.transform.localPosition = new Vector3(0, yOffset, 0);

        Renderer r = scoopObj.GetComponentInChildren<Renderer>();

        r.material = flavor switch
        {
            FlavorType.Vanilla => vanillaMat,
            FlavorType.Chocolate => chocolateMat,
            FlavorType.Strawberry => strawberryMat,
            FlavorType.Mint => mintMat,
            _ => r.material
        };

    }

    // =============== WHEN CONE IS FINISHED ===============
    private void HandleCompletedCone()
{
    if (isFinished) return;
    isFinished = true;

    transform.SetParent(null);
    this.enabled = false;

    // ✅ ENQUEUE ONLY HERE
    ConveyorController.Instance?.EnqueueFinishedCone(gameObject);
}


    // Optional helper if needed elsewhere
    public IReadOnlyList<FlavorType> GetScoops() => scoops.AsReadOnly();
}
