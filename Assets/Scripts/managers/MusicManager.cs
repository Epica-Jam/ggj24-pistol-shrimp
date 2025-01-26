using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    AudioSource source;
    public AudioClip mainOst;
    public AudioClip gameOverOst;
    private AudioLowPassFilter lowPassFilter;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
        lowPassFilter = GetComponent<AudioLowPassFilter>();
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

    public void SetLowPass(bool activo)
    {
        if (lowPassFilter != null)
        {
            lowPassFilter.enabled = activo;
        }
        else
        {
            Debug.LogWarning("Audio Low Pass Filter no encontrado en MusicManager.");
        }
    }
}
