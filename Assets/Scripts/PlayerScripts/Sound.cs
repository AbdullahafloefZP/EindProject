using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--Audio Source--")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource otherSound;
    [SerializeField] AudioSource reloadSound;
    [SerializeField] AudioSource shootSound;

    [Header("--Other Sounds--")]
    public AudioClip background;
    public AudioClip reloading;
    public AudioClip dying;
    public AudioClip zAttacking;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        otherSound.PlayOneShot(clip);
    }

    public void PlaySoundReload(AudioClip clip)
    {
        reloadSound.PlayOneShot(clip);
    }

    public void PlaySoundShoot(AudioClip clip)
    {
        shootSound.PlayOneShot(clip);
    }
}