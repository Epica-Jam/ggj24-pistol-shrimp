using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    AudioSource source;
    public AudioClip mainOst;
    public AudioClip gameOverOst;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        PlayMainOST();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMainOST()
    {
        source.clip = mainOst;
        source.Play();
    }

    public void PlayGameOverOST()
    {
        source.clip = gameOverOst;
        source.Play();
    }
}
