using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Main : MonoBehaviour
{
    [SerializeField] private AudioClip PauseSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;




    private void Awake()
    {

        pauseScreen.SetActive(false);


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //If pause screen already active unpause and viceversa
            PauseGame(!pauseScreen.activeInHierarchy);
        }
    }






    //Quit game/exit play mode if in Editor
    public void Quit()
    {
        Application.Quit(); //Quits the game (only works in build)

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Exits play mode (will only be executed in the editor)
#endif
    }


    #region Pause
    public void PauseGame(bool status)
    {
        // If status == true pause | if status == false unpause;
        SoundManager.instance.PlaySound(PauseSound);

        pauseScreen.SetActive(status);

        //When pause status is true change timescale to 0 (time stops)
        //when it's false change it back to 1 (time goes by normally)
        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }
    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
    #endregion
}