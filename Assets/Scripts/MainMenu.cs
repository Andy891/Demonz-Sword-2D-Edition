using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioClip interactSound;

    public void StartGame()
    {
        SceneManager.LoadScene("Main Scene");
        MainMenuSound.instance.PlaySound(interactSound);
    }

    public void PlaySound()
    {
        MainMenuSound.instance.PlaySound(interactSound);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        MainMenuSound.instance.PlaySound(interactSound);
    }

    public void ClearData()
    {
        MainMenuSound.instance.PlaySound(interactSound);
        PlayerPrefs.DeleteAll();
    }
}