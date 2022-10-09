using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private Animator gameOverAnimator;
    [SerializeField] private Text endScoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Text tapToStartText;


    private void Start()
    {
        gameOverAnimator.speed = 0;
    }

    public void UpdateScore (int score)
    {
        scoreText.text = "SCORE: " + score;
    }

    public void GameOver(int score)
    {
        StartCoroutine(GameEndUI(score));
    }

    public void Restart ()
    {
        SceneManager.LoadScene(0);
    }

    public void HideTapToStart()
    {
        Destroy(tapToStartText.gameObject);
    }

    private IEnumerator GameEndUI(int score)
    {
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreText.text = "NEW HIGH SCORE!";
        }
        else
            highScoreText.text = "CURRENT HIGH SCORE: " + PlayerPrefs.GetInt("HighScore");

        scoreText.enabled = false;
        endScoreText.text = scoreText.text;
        yield return new WaitForSeconds(3f);
        gameOverAnimator.speed = 1f;
        yield return new WaitForSeconds(2f);
        gameOverScreen.SetActive(true);
        
    }
}
