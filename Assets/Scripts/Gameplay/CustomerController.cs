using System;
using System.Collections;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    private Vector3 standPosition;
    private Vector3 exitPosition;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float exitMoveSpeed = 1.2f;

    [Header("Rotation")]
    public float rotationSpeed = 8f;

    [Header("Animation")]
    public Animator animator;

    [Header("Pickup")]
    public Transform handPoint;

    private bool atReceivePoint = false;
    private bool orderReceived = false;
    private bool isWaitingAtBelt = false;

    // ================= SETUP =================
    public void SetPositions(Vector3 stand, Vector3 exit)
    {
        standPosition = stand;
        exitPosition = exit;
    }

    // ================= MOVE TO CONVEYOR =================
    public void MoveToReceivePoint()
    {
        StopAllCoroutines();

        atReceivePoint = false;
        isWaitingAtBelt = false;

        SetWalking(true);

        StartCoroutine(
            MoveTo(
                standPosition,
                moveSpeed,
                () =>
                {
                    atReceivePoint = true;
                    isWaitingAtBelt = true;
                    SetWalking(false); // ✅ STOP WALK ANIMATION
                }
            )
        );
    }

    // ================= RECEIVE ICE CREAM =================
    public void ReceiveCone(GameObject cone, Action onExitComplete)
    {
        if (!isWaitingAtBelt || orderReceived)
            return;

        orderReceived = true;
        StopAllCoroutines();
        StartCoroutine(ReceiveAndExit(cone, onExitComplete));
    }

    private IEnumerator ReceiveAndExit(GameObject cone, Action onExitComplete)
    {
        // Face conveyor
        RotateTowards(standPosition + Vector3.forward);
        SetWalking(false);

        // Pause for readability
        yield return new WaitForSeconds(0.4f);

        // Take animation (one-shot)
        if (animator != null)
            animator.Play("Take");

        // Sync with hand
        yield return new WaitForSeconds(0.35f);

        // Move cone slowly to hand
        if (cone != null)
        {
            ConeSinkToHand sink = cone.GetComponent<ConeSinkToHand>();
            if (sink != null && handPoint != null)
                sink.StartSink(handPoint, 0.8f);
            else
                Destroy(cone);
        }

        yield return new WaitForSeconds(0.3f);

        // Walk to exit
        SetWalking(true);
        yield return StartCoroutine(MoveTo(exitPosition, exitMoveSpeed));

        Destroy(gameObject, 0.01f);
        onExitComplete?.Invoke();
    }

    // ================= QUEUE MOVEMENT =================
    public void MoveToQueuePosition(Vector3 targetPos)
    {
        StopAllCoroutines();
        StartCoroutine(MoveRoutine(targetPos));
    }

    private IEnumerator MoveRoutine(Vector3 target)
    {
        SetWalking(true);

        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            RotateTowards(target);

            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                Time.deltaTime * 2.5f
            );
            yield return null;
        }

        transform.position = target;
        SetWalking(false);
    }

    // ================= GENERIC MOVE =================
    private IEnumerator MoveTo(Vector3 target, float speedOverride = -1f, Action onArrive = null)
    {
        float speed = speedOverride > 0 ? speedOverride : moveSpeed;

        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            RotateTowards(target);

            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = target;
        onArrive?.Invoke();
    }

    // ================= ROTATION =================
    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    // ================= ANIMATION CONTROL =================
    private void SetWalking(bool walking)
    {
        if (animator == null) return;
        animator.SetBool("IsWalking", walking);
    }
}
