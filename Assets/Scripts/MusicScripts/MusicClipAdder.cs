
using UnityEngine;

public class MusicClipAdder : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private AudioClip myBackgroundMusic;
    private AudioSource backgroundMusicAudioSource;
    void Start()
    {
        backgroundMusicAudioSource = BackgroundMusic.current.gameObject.GetComponent<AudioSource>();
        backgroundMusicAudioSource.clip = myBackgroundMusic;
        backgroundMusicAudioSource.Play();
    }


}
