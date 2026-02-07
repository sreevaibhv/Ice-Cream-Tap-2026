using System;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    public static ConveyorController Instance;

    [Header("Conveyor Settings")]
    public float beltSpeed = 1.5f;

    [Header("Conveyor Loop Path (Closed)")]
    public Transform[] pathPoints;

    // Internal item data
    private class ConveyorItem
    {
        public GameObject obj;
        public int pathIndex;
        public float progress;
    }

    private readonly List<ConveyorItem> conesOnBelt = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        MoveConesOnBelt();
    }

    // 🔹 Call this when cone/product is ready
   public void EnqueueFinishedCone(GameObject cone)
{
    if (cone == null) return;

    // 🔒 Prevent double-enqueue
    if (conesOnBelt.Exists(c => c.obj == cone))
        return;

    ConveyorItem item = new ConveyorItem
    {
        obj = cone,
        pathIndex = 0,
        progress = 0f
    };

    cone.transform.position = pathPoints[0].position;
    RotateTowardsNext(item);
    conesOnBelt.Add(item);
}
    public void Init(float speed)
    {
        beltSpeed = speed;
        Debug.Log("Conveyor speed set to: " + beltSpeed);
    }
public void MoveConesOnBelt()
{
    for (int i = conesOnBelt.Count - 1; i >= 0; i--)
    {
        var item = conesOnBelt[i];
        if (item.obj == null) continue;

        // 🔑 EXACT end-of-belt detection
        if (item.pathIndex == pathPoints.Length - 1)
        {
            // ❗ REMOVE FROM CONVEYOR FIRST
            conesOnBelt.RemoveAt(i);

            // Snap to end
            item.obj.transform.position =
                pathPoints[pathPoints.Length - 1].position;

            // Deliver immediately
            DeliveryManager.Instance.DeliverCone(item.obj);
            continue;
        }

        Transform from = pathPoints[item.pathIndex];
        Transform to = pathPoints[item.pathIndex + 1];

        float segmentLength = Vector3.Distance(from.position, to.position);
        item.progress += (beltSpeed * Time.deltaTime) / segmentLength;

        item.obj.transform.position =
            Vector3.Lerp(from.position, to.position, item.progress);

        RotateTowardsNext(item);

        if (item.progress >= 1f)
        {
            item.progress = 0f;
            item.pathIndex++; // ❌ NO modulo
        }
    }
}

private void OnConeReachedEnd(GameObject cone)
{
    // Snap to final point
    cone.transform.position = pathPoints[pathPoints.Length - 1].position;

    // Deliver
    DeliveryManager.Instance.DeliverCone(cone);
}

    public void RemoveCone(GameObject cone)
    {
        conesOnBelt.RemoveAll(c => c.obj == cone);
    }

    private void RotateTowardsNext(ConveyorItem item)
    {
        Transform from = pathPoints[item.pathIndex];
        Transform to = pathPoints[(item.pathIndex + 1) % pathPoints.Length];

        Vector3 direction = (to.position - from.position).normalized;
        if (direction != Vector3.zero)
            item.obj.transform.rotation = Quaternion.LookRotation(direction);
    }
}
