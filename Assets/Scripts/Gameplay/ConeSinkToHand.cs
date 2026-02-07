using System.Collections;
using UnityEngine;

public class ConeSinkToHand : MonoBehaviour
{
    private bool isSinking = false;

    public void StartSink(Transform handPoint, float duration = 0.4f)
    {
        if (isSinking) return;
        isSinking = true;

        // Remove from conveyor so it stops moving
        if (ConveyorController.Instance != null)
            ConveyorController.Instance.RemoveCone(gameObject);

        StartCoroutine(SinkRoutine(handPoint, duration));
    }

    private IEnumerator SinkRoutine(Transform handPoint, float duration)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 startScale = transform.localScale;

        float t = 0f;

        while (t < 1f)
{
    t += Time.deltaTime / duration;
    float smoothT = Mathf.SmoothStep(0f, 1f, t);

    // ✨ ARC movement (slight lift)
    Vector3 arc = Vector3.up * Mathf.Sin(smoothT * Mathf.PI) * 0.15f;

    transform.position =
        Vector3.Lerp(startPos, handPoint.position, smoothT) + arc;

    transform.rotation =
        Quaternion.Slerp(startRot, handPoint.rotation, smoothT);

    // ❄️ Gentle shrink (not instant vanish)
    transform.localScale =
        Vector3.Lerp(startScale, startScale * 0.15f, smoothT);

    yield return null;
}


        Destroy(gameObject);
    }
}
