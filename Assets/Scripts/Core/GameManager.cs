using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { Menu, Playing, Paused, Win, Fail }
    public GameState CurrentState { get; private set; }

    private void Start() => SetState(GameState.Menu);

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        UIManager.Instance?.UpdateUI(newState);
    }

    public void StartGame() => SetState(GameState.Playing);
    public void WinGame() => SetState(GameState.Win);
    public void FailGame(string reason)
    {
        Debug.Log($"Fail reason: {reason}");
        SetState(GameState.Fail);
    }
}
