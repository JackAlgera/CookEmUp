using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public static MenuController instance;

    public Text scoreText;

	void Awake () {
		if(instance == null)
        {
            instance = this;
        }

        if(!PlayerPrefs.HasKey("HighScore"))
        {
            FullReset();
        }

        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "" + PlayerPrefs.GetInt("HighScore");
    }

    public void CheckHighScore(int newHighScore)
    {
        if(newHighScore > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", newHighScore);
        }
    }

    public void FullReset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("FirstLaunch", 1);

        PlayerPrefs.SetInt("HighScore", 0);
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

}
