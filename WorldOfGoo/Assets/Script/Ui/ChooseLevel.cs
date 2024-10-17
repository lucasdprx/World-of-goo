using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseLevel : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _textScoreLevel;
    [SerializeField] private List<Button> _listButtonLevel;
    private void OnEnable()
    {
        SetScore();
        SetButton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
            SetScore();
            SetButton();
        }
    }
    private void SetScore()
    {
        for (int i = 0; i < _textScoreLevel.Count; i++)
        {
            _textScoreLevel[i].text = "Score : " + PlayerPrefs.GetInt("Level " + (i + 1)).ToString();
        }
    }
    private void SetButton()
    {
        for (int i = 0; i < _listButtonLevel.Count; i++)
        {
            _listButtonLevel[i].interactable = PlayerPrefs.HasKey("Level " + (i + 1));
        }
    }
    public void EnterLevel(int indexScene)
    {
        SceneManager.LoadScene(indexScene);
    }
}
