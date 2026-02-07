using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    public enum RandomizeMode { OnLevelStart, OnEveryScoop, Interval }
    public RandomizeMode randomizeMode = RandomizeMode.OnLevelStart;

    [Tooltip("List of possible flavors this pipe can output.")]
    public List<FlavorType> possibleFlavors = new List<FlavorType>();

    [Tooltip("Reference to a Renderer used to tint the pipe for visual feedback.")]
    public Renderer pipeRenderer;

    [Tooltip("Interval in seconds when using Interval mode.")]
    public float intervalSeconds = 2f;

    public FlavorType CurrentFlavor { get; private set; }

    public static event Action<PipeController, FlavorType> OnPipeFlavorChanged;

    private Coroutine intervalRoutine;

    //public LevelManager levelManager;

    private void Start()
    {
        RandomizeFlavor();
        StartCoroutine(InitializeRandomizeFlavor());
    }

    private IEnumerator InitializeRandomizeFlavor()
    {
        yield return new WaitForSeconds(0.25f);

        intervalSeconds = LevelManager.Instance.GetCurrentLevel().pipeRandomIntervel;

        if (randomizeMode == RandomizeMode.OnLevelStart)
            RandomizeFlavor();

        if (randomizeMode == RandomizeMode.Interval)
            intervalRoutine = StartCoroutine(IntervalRandomize());
    }

    private void OnEnable()
    {
        ConeController.OnScoopPlaced += HandleScoopPlaced;
    }

    private void OnDisable()
    {
        ConeController.OnScoopPlaced -= HandleScoopPlaced;
        if (intervalRoutine != null) StopCoroutine(intervalRoutine);
    }

    private void HandleScoopPlaced(ConeController cone, FlavorType addedFlavor)
    {
        if (randomizeMode == RandomizeMode.OnEveryScoop)
            RandomizeFlavor();
    }

    private IEnumerator IntervalRandomize()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(intervalSeconds);
            RandomizeFlavor();
        }
    }

    // ================================
    //      CLICK TO ADD SCOOP
    // ================================
    private void OnMouseDown()
    {
        // 1. Get active cone from OrderManager
        ConeController activeCone = OrderManager.Instance.currentCone;

        if (activeCone == null)
        {
            Debug.Log("❌ No active cone to place scoop.");
            return;
        }

        // 2. Get flavor from this pipe
        FlavorType flavor = CurrentFlavor;

        // 3. Add scoop to cone
        activeCone.TryAddScoop(flavor);

        // 4. Optional feedback
        //SetPouring(true);
        //StartCoroutine(StopPouring());
        SetPouring(false);
    }

    private IEnumerator StopPouring()
    {
        yield return new WaitForSeconds(0.1f);
        SetPouring(false);
    }

    public void SetPouring(bool state)
    {
        Debug.LogWarning("Pipe pouring: " + state);
        if(state == true)
        {

            OnMouseDown();
        }
    }

    // ================================
    //      RANDOMIZE FLAVOR LOGIC
    // ================================
    [ContextMenu("Randomize Flavor")]
    public void RandomizeFlavor()
    {
        if (possibleFlavors == null || possibleFlavors.Count == 0)
            return;

        int idx = UnityEngine.Random.Range(0, possibleFlavors.Count);
        CurrentFlavor = possibleFlavors[idx];

        UpdateVisual();
        OnPipeFlavorChanged?.Invoke(this, CurrentFlavor);
    }

    private void UpdateVisual()
    {
        if (pipeRenderer == null) return;

        Color col = FlavorColorMap.GetColorForFlavor(CurrentFlavor);
        if (pipeRenderer.material != null)
            pipeRenderer.material.color = col;
    }

    // Call this externally if needed
    public FlavorType DispenseFlavor()
    {
        if (randomizeMode == RandomizeMode.OnEveryScoop)
            RandomizeFlavor();

        return CurrentFlavor;
    }
}
