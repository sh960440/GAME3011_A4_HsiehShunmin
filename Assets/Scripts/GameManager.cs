using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text timerText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text resultText;
    private float timer;
    private int skill;
    private bool timing = false;

    void Update()
    {
        if (timing)
        {
            timer -= Time.deltaTime;

            if (timer > 0)
                timerText.text = timer.ToString("0.0");
            else
                Gameover(false);
        }
    }

    public void SetTimer(float time)
    {
        timer = time;
        timing = true;
    }

    public void Gameover(bool won)
    {
        timing = false;
        gameOverPanel.SetActive(true);

        if (won)
        {
            resultText.text = "YOU WIN";
        }
        else
        {
            resultText.text = "YOU LOSE";
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene("Game");
    }
}
