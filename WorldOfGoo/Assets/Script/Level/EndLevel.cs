using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textScore;

    [SerializeField] private TextMeshProUGUI _textScoreEnd;
    [SerializeField] private GameObject _uiEnd;
    
    private int score;
    public int _lastSkull = -99;
    public static EndLevel Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            print("win");
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            SetNewNodeOnMonsters(List_Monsters.Instance._listMonsters);
            _lastSkull = collision.GetComponent<MoveMonsters>()._indexSkull;
        }
    }

    private void SetNewNodeOnMonsters(List<MoveMonsters> monsters)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i].enabled)
            {
                monsters[i]._speed *= 2;
                monsters[i]._endGame = true;
                score++;
            }
        }
        if (score <= 0)
        {
            _textScore.text = "-1";
            SetTextScore();
        }
    }
    public void SetTextScore()
    {
        if (_textScore.text == "")
            _textScore.text = "1";

        else
        {
            _textScore.text = (int.Parse(_textScore.text) + 1).ToString();
        }

        if (_textScore.text == score.ToString())
        {
            _textScoreEnd.text = "Score : " + score.ToString();
            _uiEnd.SetActive(true);
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
