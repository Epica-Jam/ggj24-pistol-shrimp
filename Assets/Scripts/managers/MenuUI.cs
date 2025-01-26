using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public Button playButton;
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(OnPlay);
    }

    void OnPlay()
    {
        SceneManager.LoadScene("Main");
    }

}
