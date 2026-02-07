using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelDataSO : ScriptableObject
{
    [Header("General Settings")]
    public int levelId = 0;

    public float beltSpeed = 1f;
    public float spawnInterval = 1.5f;

    public int deliveriesToWin = 3;
    public float pipeRandomIntervel = 3f;
    [Header("Pipe Layout")]
    public PipeSetup[] pipes;

    [Header("Win / Fail Conditions")]
    public int allowedOverflows = 1;
    public int targetScore = 50;
}

[System.Serializable]
public class PipeSetup
{
    public string pipeName;
    public FlavorType flavor;
    public float pourRate = 1.0f;
    public Vector3 position;
}
