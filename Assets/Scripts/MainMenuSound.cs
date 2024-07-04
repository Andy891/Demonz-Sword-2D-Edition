
using UnityEngine;

public class MainMenuSound : MonoBehaviour {
    public static MainMenuSound instance { get; private set; }
    private AudioSource source;

    private void Awake() {
        instance = this;
        source = GetComponent<AudioSource>();
        if (SoundManager.instance) {
            SoundManager.instance.StopMusic();
        }
    }

    public void PlaySound(AudioClip _sound) {
        source.PlayOneShot(_sound);
    }
}
