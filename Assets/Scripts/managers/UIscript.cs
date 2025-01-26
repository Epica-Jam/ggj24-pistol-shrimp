using TMPro;
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
    private SpriteRenderer sprite1;
    [SerializeField]
    private GameObject _vida2;
    private SpriteRenderer sprite2;
    [SerializeField]
    private GameObject _vida3;
    private SpriteRenderer sprite3;
    [SerializeField]
    private Sprite _vidaVacia;

    [SerializeField]
    private SpriteRenderer _gameOver;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _puntajeText.text = "0";
        sprite1 = _vida1.GetComponent<SpriteRenderer>();
        sprite2 = _vida2.GetComponent<SpriteRenderer>();
        sprite3 = _vida3.GetComponent<SpriteRenderer>();
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
            sprite3.sprite = _vidaVacia;
        }
        if (vidaActual == 1)
        {
            sprite3.sprite = _vidaVacia;
            sprite2.sprite = _vidaVacia;
        }
        if (vidaActual == 0)
        {
            sprite3.sprite = _vidaVacia;
            sprite2.sprite = _vidaVacia;
            sprite1.sprite = _vidaVacia;
            _gameOver.enabled = true;
            _puntajeText.transform.parent.position = new Vector2(Camera.main.rect.center.x, Camera.main.rect.center.y);
        }
    }
}
