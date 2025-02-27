using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public static playerScript Instance { get; private set; }
    
    // movimiento

    public float velocPlayer = 4.5f; // Velocidad de movimiento horizontal
    public float impulsoSalto = 10f; // Fuerza del salto
    public float gravedad = 3f; // Caida bajo el agua
    public float gravedadFAST = 10f; // 2da gravedad, aplicada cuando se quiere caer rapido
    public float maxCaida = -5f; // Velocidad max de caida
    private Rigidbody2D player_rb;

    // Vidas y daño
    public int vidasJugador = 3;
    private bool invulnerable = false; // Bandera para invulnerabilidad
    [SerializeField]
    private float tiempoInvulnerable = 2f; // Duración de la invulnerabilidad
    private SpriteRenderer spriteRenderer; // Para efectos visuales
    [SerializeField]
    private UIscript _uiManager;
    [SerializeField]
    private int _puntaje = 0;

    // ataque

    [SerializeField]
    private float tiempoCarga = 0f; // T0 de carga
    private bool cargando = false; // Bool de si esta cargando o no
    private float cargaMax = 2f; // carga maxima posible
    private float tamMax = 3f; // tamaño maximo de la burbuja cargada

    // cooldowns
    [SerializeField]
    private float tiempoEntreDisparosLigero = 0.2f; // Tiempo mínimo entre disparos ligeros
    [SerializeField]
    private float tiempoEntreDisparosCargado = 1.5f; // Tiempo mínimo entre disparos cargados
    private float tiempoUltimoDisparoLigero = -Mathf.Infinity;
    private float tiempoUltimoDisparoCargado = -Mathf.Infinity;

    // referencias ataque

    [SerializeField]
    private Transform pistola0trans; // Transform del objeto hijo pistola, para que los disparos usen su posicion como referencia
    [SerializeField]
    private Transform pistola1trans; // tenaza especial
    [SerializeField]
    public SpriteRenderer pistola0render;
    [SerializeField]
    public SpriteRenderer pistola1render;
    private Transform pistolaActualtrans;
    private SpriteRenderer pistolaActualrender;
    [SerializeField]
    private GameObject tenaza;
    [SerializeField]
    private bool enPowerup = false;
    [SerializeField]
    private GameObject burbuCprefab; // Prefab de la burbuja chica
    [SerializeField]
    private GameObject burbuGprefab; // Prefab de la burbuja grande
    private float tiempoOriginalDisparoLigero; // Para restaurar tiempos originales
    private float tiempoOriginalDisparoCargado;

    // audio

    [SerializeField]
    private AudioSource sonidosSFX;
    public AudioClip sonidoDisparoLigero; // Sonido para el disparo ligero
    public AudioClip sonidoDmg; // Sonido al ser dañado
    public AudioClip sonidoCargaDisparo;
    public AudioClip sonidoDisparoCargado;
    public AudioClip popBurbuja;

    
    List<PowerUp> powerUps = new List<PowerUp>();
    
    void Awake() {
    if (Instance != null) return;
    Instance = this;
  }
    // Start is called before the first frame update
    void Start()
    {
        player_rb = GetComponent<Rigidbody2D>();
        player_rb.gravityScale = gravedad;

        // Guardar los tiempos originales porsia
        tiempoOriginalDisparoLigero = tiempoEntreDisparosLigero;
        tiempoOriginalDisparoCargado = tiempoEntreDisparosCargado;

        spriteRenderer = GetComponent<SpriteRenderer>(); // guardar el renderer

        pistolaActualtrans = pistola0trans;
        pistolaActualrender = pistola0render;

        _uiManager = GameObject.Find("UI").GetComponent<UIscript>();
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento(); // movimiento :)
        UpdatePowerUps();

        // Ataques
        if (powerUps.Any(p => p.m_type == PowerUpType.AutoShoot) && PuedeDispararLigero())
        {
            enPowerup = true;
            tenaza.GetComponent<Animator>().SetBool("disparando", true);
            burbuChica();
            tiempoUltimoDisparoLigero = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z) && PuedeDispararLigero()) // Disparo ligero
        {
            enPowerup = false;
            tenaza.GetComponent<Animator>().SetBool("disparando", true);
            burbuChica();
            tiempoUltimoDisparoLigero = Time.time;
        }
        else
        {
            tenaza.GetComponent<Animator>().SetBool("disparando", false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.X) && PuedeDispararCargado()) // Disparo cargado, deteccion de que se empezo a cargar
        {
            cargando = true;
            tiempoCarga = 0f;
        }

        if (cargando && Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.X)) // Carga del disparo cargado
        {
            tiempoCarga += Time.deltaTime;

            if (tiempoCarga > cargaMax)
            {
                tiempoCarga = cargaMax;
            }

            //sonidosSFX.PlayOneShot(sonidoCargaDisparo);
            //sonidosSFX.loop = true;

        }

        if (cargando && Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.X)) // Disparo de la burbuja cargada
        {
            cargando = false;
            //sonidosSFX.loop = false;
            //sonidosSFX.Stop();
            burbuGrande();
            tiempoUltimoDisparoCargado = Time.time;
        }

        // Salto

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) // Salto normal
        {
            Saltito();
        }

        if (Input.GetAxis("Vertical") < 0) // Caida rapida
        {
            player_rb.gravityScale = gravedadFAST;
        }

        if (player_rb.velocity.y < 0 && !Input.GetKey(KeyCode.Space)) // Recuperar la caida normal
        {
            player_rb.gravityScale = gravedad;
        }

        if (enPowerup) // cambiar de tenaza
        {
            pistola1render.enabled = true;
            pistolaActualtrans = pistola1trans;
            pistolaActualrender = pistola1render;
            pistola0render.enabled = false;
        }
        else
        {
            pistola0render.enabled = true;
            pistolaActualtrans = pistola0trans;
            pistolaActualrender = pistola0render;
            pistola1render.enabled = false;
        }

        suavizarCaida();

    }

    void Movimiento()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // input horizontal

        player_rb.velocity = new Vector2(horizontalInput * velocPlayer, player_rb.velocity.y);

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -6, 6), Mathf.Clamp(transform.position.y, -4, 4));

    }

    void burbuChica()
    {
        Instantiate(burbuCprefab, pistolaActualtrans.position, Quaternion.identity);
        sonidosSFX.PlayOneShot(sonidoDisparoLigero);
    }

    void burbuGrande()
    {
        GameObject disparo = Instantiate(burbuGprefab, pistolaActualtrans.position, Quaternion.identity);

        float escalado = Mathf.Lerp(1f, tamMax, tiempoCarga / cargaMax); // Interpolacion tamaños

        sonidosSFX.PlayOneShot(sonidoDisparoCargado);

        disparo.transform.localScale = new Vector2(escalado, escalado);
        disparo.GetComponent<burbuscript>()._carga = tiempoCarga;

    }

    void Saltito()
    {
        player_rb.velocity = new Vector2(player_rb.velocity.x, 0f); // Resetea la velocidad vertical

        player_rb.AddForce(Vector2.up * impulsoSalto, ForceMode2D.Impulse); // Aplica fuerza de salto

    }

    void suavizarCaida()
    {
        if (player_rb.velocity.y < maxCaida)
        {
            player_rb.velocity = new Vector2(player_rb.velocity.x, maxCaida);
        }
    }

    // Métodos para verificar si se puede disparar
    bool PuedeDispararLigero()
    {
        return Time.time >= tiempoUltimoDisparoLigero + tiempoEntreDisparosLigero;
    }

    bool PuedeDispararCargado()
    {
        return Time.time >= tiempoUltimoDisparoCargado + tiempoEntreDisparosCargado;
    }

    // Métodos públicos para modificar tiempos de disparo
    public void ModificarTiempoDisparoLigero(float nuevoTiempo)
    {
        tiempoEntreDisparosLigero = nuevoTiempo;
    }

    public void ModificarTiempoDisparoCargado(float nuevoTiempo)
    {
        tiempoEntreDisparosCargado = nuevoTiempo;
    }

    public void RestaurarTiemposOriginales()
    {
        tiempoEntreDisparosLigero = tiempoOriginalDisparoLigero;
        tiempoEntreDisparosCargado = tiempoOriginalDisparoCargado;
    }

    public void AddPowerUp(PowerUp pup)
    {
        pup.SetActive(true);
        powerUps.Add(pup);
    }

    private void UpdatePowerUps()
    {
        powerUps.RemoveAll(pup => !pup.m_active);
        foreach (PowerUp pup in powerUps)
        {
            pup.RunPowerUp();
            if (pup.GetRemainingTime() <= 0) pup.SetActive(false);
        }
    }
    // Cosas de daño

    // Método para recibir daño
    public void RecibirDmg()
    {
        if (!invulnerable) // Solo recibe daño si no es invulnerable
        {
            sonidosSFX.PlayOneShot(sonidoDmg);
            vidasJugador -= 1;

            Debug.Log("Daño recibido, vidas =" + vidasJugador);

            if (vidasJugador <= 0)
            {
                // Lógica de Game Over
                Debug.Log("¡Game Over!");
                GameManager.Instance.GameOver();
                Destroy(this);
            }
            else
            {
                StartCoroutine(ActivarInvulnerabilidad());
            }
        }
    }

    // Corrutina para invulnerabilidad
    private IEnumerator ActivarInvulnerabilidad()
    {
        invulnerable = true;

        // Feedback visual: Parpadeo
        float tiempo = 0f;
        while (tiempo < tiempoInvulnerable)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Alterna visibilidad
            pistolaActualrender.enabled = !pistolaActualrender.enabled;
            yield return new WaitForSeconds(0.1f);
            tiempo += 0.1f;
        }

        spriteRenderer.enabled = true; // Asegurar que quede visible al final
        pistolaActualrender.enabled = true;
        invulnerable = false;
    }

    // Colisionador

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo") || collision.CompareTag("Proyectil Enemigo"))
        {
            RecibirDmg(); // Ajusta la cantidad de daño según el diseño
            _uiManager.UpdateVidas(vidasJugador);
        }

        if (collision.CompareTag("Powerup"))
        {
            PowerUp pup = collision.gameObject.GetComponent<PowerUp>();
            if (pup == null) return;
            pup.gameObject.SetActive(false);
            if (pup.m_type == PowerUpType.AutoShoot) tiempoEntreDisparosLigero = 0.1f;
            AddPowerUp(pup);
        }
    }

    // Puntaje
    public void AddPuntos(int puntos)
    {
        //agregar 10 puntos y actualizarle al UI
        _puntaje += puntos;
        GameManager.Instance.AddPoints(puntos);
        _uiManager.UpdatePuntaje(GameManager.Instance.m_score);
    }

    public void MandarAudio(AudioClip audio)
    {
        sonidosSFX.PlayOneShot(audio);
    }
}
