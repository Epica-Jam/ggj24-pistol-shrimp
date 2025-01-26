using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    AudioSource source;
    public Button playButton;
    public AudioClip buttonClickSound;
    // Start is called before the first frame update
    void Start()
    { 
        source = GetComponent<AudioSource>();
        playButton.onClick.AddListener(OnPlay);
    }

    void OnPlay()
    {
        source.clip = buttonClickSound;
        source.Play();
        SceneManager.LoadScene("Main");
    }

}
