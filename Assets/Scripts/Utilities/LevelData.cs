[System.Serializable]
public class LevelData
{
    public string levelId;
    public float spawnInterval;
    public float beltSpeed;
    public PipeData[] pipes;
}

[System.Serializable]
public class PipeData
{
    public FlavorType flavor;
    public float pourRate;
}
