using System;
using UnityEngine;

public class GlobalHotkey : MonoBehaviour
{
    private Transform twoDCanvas;
    private Transform pauseCanvas;
    // 是否暂停
    public static bool isPaused;
    // 是否显示光标
    public static bool m_isAlt;

    private void Start()
    {
        twoDCanvas = InstanceManager.Instance.Get(InstanceType.TwoDCanvas);
        pauseCanvas = InstanceManager.Instance.Get(InstanceType.PauseCanvas);
        isPaused = false;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // 开始游戏
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartGame();
        }

        // 降低难度
        if (Input.GetKeyDown(KeyCode.F3))
        {
            LevelDown();
        }

        // 提升难度
        if (Input.GetKeyDown(KeyCode.F4))
        {
            LevelUp();
        }

        // 重置游戏
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ResetGame();
        }
        
        // 暂停打开菜单
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
        // 按了alt键后解除光标的隐藏状态
        if (!isPaused && (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt)))
        {
            m_isAlt = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        // 左键点击屏幕恢复隐藏状态
        if (!isPaused && m_isAlt && Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_isAlt = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void PauseGame()
    {
        twoDCanvas.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        twoDCanvas.gameObject.SetActive(true);
        pauseCanvas.gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StartGame()
    {
        LevelManager.Instance.StartGame();
        ResumeGame();
    }

    public void LevelDown()
    {
        LevelManager.Instance.LevelDown();
    }

    public void LevelUp()
    {
        LevelManager.Instance.LevelUp();
    }

    public void ResetGame()
    {
        LevelManager.Instance.ResetGame();
    }
}