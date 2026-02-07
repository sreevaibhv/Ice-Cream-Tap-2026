using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelDataSO[] levels;
    private LevelDataSO currentLevel;

    public int deliveriesDone = 0;


    private void Start()
    {
        LoadLevel(1);
    }

    public void LoadLevel(int index)
    {
        Debug.Log("Function Called");

        if (index < 0 || index >= levels.Length) index = 0;

        currentLevel = levels[index];
        deliveriesDone = 0;

        // Pass the belt speed float (assumes LevelDataSO has a beltSpeed field)
        if (ConveyorController.Instance != null && currentLevel != null)
        {
            ConveyorController.Instance.Init(currentLevel.beltSpeed);
        }

        Debug.Log($"Loaded Level {currentLevel.levelId}");
    }

    public void NextDelivery()
    {
        deliveriesDone++;

        // Update belt speed dynamically
        if (ConveyorController.Instance != null)
            ConveyorController.Instance.beltSpeed =
                SpeedManager.Instance.GetCurrentSpeed(deliveriesDone);

        Debug.Log("Delivery Completed!");
    }

    public LevelDataSO GetCurrentLevel() => currentLevel;
}
