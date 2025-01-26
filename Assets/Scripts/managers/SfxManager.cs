using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }
    AudioSource source;
    public AudioClip buttonSound;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }
    void Start()
    {
        source = GetComponent<AudioSource>();   
    }
    public void Play(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
    public void PlayButtonSound()
    {
        Play(buttonSound);
    }
}
