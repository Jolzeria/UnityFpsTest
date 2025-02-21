using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseFunc : MonoBehaviour
{
    public void StartGame()
    {
        LevelManager.Instance.StartGame();
    }
}