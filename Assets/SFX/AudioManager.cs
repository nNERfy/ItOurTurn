using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]AudioSource musicSource;
    [SerializeField]AudioSource SFXSource;

    public AudioClip backgroundSound;
    public AudioClip BattleSound;

    private void Start(){
        musicSource.clip = backgroundSound;
        musicSource.Play();
    }
}
