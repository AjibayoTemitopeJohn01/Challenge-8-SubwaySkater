using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text highScore;
    public Text coinScore;
    private void Awake()
    {
        highScore.text = "High Score " + PlayerPrefs.GetInt("HighScore").ToString();
        coinScore.text = "Coin Score " + PlayerPrefs.GetInt("Coin").ToString();
    }
    
    public void OnPlayButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
