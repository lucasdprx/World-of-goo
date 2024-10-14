using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool _gameIsPaused;
    [SerializeField] private GameObject _uiPause;
    [SerializeField] private GameObject _uiOption;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }
    public void Resume()
    {
        if (_gameIsPaused)
        {
            Time.timeScale = 1.0f;
            _gameIsPaused = false;
            _uiPause.SetActive(false);
        }
        else
        {
            Time.timeScale = 0.0f;
            _gameIsPaused = true;
            _uiPause.SetActive(true);
        }
    }
    public void OpenOption()
    {
        _uiOption.SetActive(true);
    }
    public void CloseOption()
    {
        _uiOption.SetActive(false);
    }
    public void Retry()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
