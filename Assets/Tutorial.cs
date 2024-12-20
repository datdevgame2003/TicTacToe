using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    public AudioSource clickButSource;


    void Start()
    {
        clickButSource.Stop();
    }

    public void LoadMenu()
    {
        PlayButtonClickSound();
      
        SceneManager.LoadScene("Menu");
    }
    private void PlayButtonClickSound()
    {
        if (clickButSource != null)
        {
            clickButSource.Play();
        }
    }
}
