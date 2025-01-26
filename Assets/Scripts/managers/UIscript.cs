using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIscript : MonoBehaviour
{
    public static UIscript Instance { get; private set; }

    [SerializeField]
    TMP_Text _levelText;
    [SerializeField]
    private TMP_Text _puntajeText;
    [SerializeField]
    private GameObject _vida1;
    [SerializeField]
    private GameObject _vida2;
    [SerializeField]
    private GameObject _vida3;

    [SerializeField]
    private SpriteRenderer _gameOver;
    [SerializeField]
    private GameObject botonMenuGameOver;
    [SerializeField]
    private GameObject textoNivelGameOver;
    [SerializeField]
    private GameObject botonPausaGameOver;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _puntajeText.text = "0";
        botonMenuGameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateLevel()
    {
        _levelText.text = $"Nivel {GameManager.Instance.m_level}";
    }

    public void UpdatePuntaje(int playerScore)
    {
        _puntajeText.text = playerScore.ToString();
    }

    public void UpdateVidas(int vidaActual)
    {
        if (vidaActual == 2)
        {
            _vida3.GetComponent<Animator>().SetTrigger("Daño");
        }
        if (vidaActual == 1)
        {
            _vida2.GetComponent<Animator>().SetTrigger("Daño");
        }
        if (vidaActual == 0)
        {
            _vida1.GetComponent<Animator>().SetTrigger("Daño");
            _gameOver.enabled = true;
            _puntajeText.transform.parent.position = new Vector2(-4.866f, -7.55f);
            botonMenuGameOver.SetActive(true);
            botonPausaGameOver.SetActive(false);
            textoNivelGameOver.SetActive(false);
        }
    }
}
