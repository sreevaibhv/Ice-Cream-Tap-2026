using UnityEngine;

public class LevelProgression : MonoBehaviour
{
    public int currentLevel = 0;

    public void OnWin()
    {
        currentLevel++;
        LevelManager.Instance.LoadLevel(currentLevel);
    }

    public void OnFail()
    {
        LevelManager.Instance.LoadLevel(currentLevel); // retry
    }
}
