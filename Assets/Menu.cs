using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public AudioSource clickSource;
    

    void Start()
    {
        clickSource.Stop();
    }

    public void LoadGame()
    {
        PlayButtonClickSound();
        SceneManager.LoadScene("SampleScene");
    }
    public void Exit()
    {
        PlayButtonClickSound();
        Application.Quit();
    }
    private void PlayButtonClickSound()
    {
        if (clickSource != null)
        {
            clickSource.Play();
        }
    }
    public void LoadTutorialScene()
    {
        if (clickSource != null)
        {
            clickSource.Play();
        }
        SceneManager.LoadScene("Tutorial");
    }
}
