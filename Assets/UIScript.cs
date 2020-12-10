using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject pausePanel;
    public Text winText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        winText.text = "";
    }

    public void Win()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        winText.text = "You Win";
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        pausePanel.SetActive(true);
    }
}
