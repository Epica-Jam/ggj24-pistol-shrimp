using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
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
    private bool enPowerup = false;
    [SerializeField]
    private GameObject burbuCprefab; // Prefab de la burbuja chica
    [SerializeField]
    private GameObject burbuGprefab; // Prefab de la burbuja grande
    private float tiempoOriginalDisparoLigero; // Para restaurar tiempos originales
    private float tiempoOriginalDisparoCargado;

    // audio

    public AudioSource audioSource; // Referencia al AudioSource
    public AudioClip sonidoDisparoLigero; // Sonido para el disparo ligero
    public AudioClip sonidoDmg; // Sonido al ser dañado

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
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento(); // movimiento :)

        // Ataques

        if (Input.GetKeyDown(KeyCode.Mouse0) && PuedeDispararLigero()) // Disparo ligero
        {
            burbuChica();
            tiempoUltimoDisparoLigero = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && PuedeDispararCargado()) // Disparo cargado, deteccion de que se empezo a cargar
        {
            cargando = true;
            tiempoCarga = 0f;
        }

        if (cargando && Input.GetKey(KeyCode.Mouse1)) // Carga del disparo cargado
        {
            tiempoCarga += Time.deltaTime;

            if (tiempoCarga > cargaMax)
            {
                tiempoCarga = cargaMax;
            }

        }

        if (cargando && Input.GetKeyUp(KeyCode.Mouse1)) // Disparo de la burbuja cargada
        {
            cargando = false;
            burbuGrande();
            tiempoUltimoDisparoCargado = Time.time;
        }

        // Salto

        if (Input.GetKeyDown(KeyCode.Space)) // Salto normal
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
        audioSource.PlayOneShot(sonidoDisparoLigero);
    }

    void burbuGrande()
    {
        GameObject disparo = Instantiate(burbuGprefab, pistolaActualtrans.position, Quaternion.identity);

        float escalado = Mathf.Lerp(1f, tamMax, tiempoCarga/cargaMax); // Interpolacion tamaños

        disparo.transform.localScale = new Vector2(escalado,escalado);

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

    // Cosas de daño

    // Método para recibir daño
    public void RecibirDmg()
    {
        if (!invulnerable) // Solo recibe daño si no es invulnerable
        {
            audioSource.PlayOneShot(sonidoDmg);
            vidasJugador -= 1;

            Debug.Log("Daño recibido, vidas =" + vidasJugador);

            if (vidasJugador <= 0)
            {
                // Lógica de Game Over
                Debug.Log("¡Game Over!");
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
        if (collision.CompareTag("Enemigo") || collision.CompareTag("ProyectilEnemigo"))
        {
            RecibirDmg(); // Ajusta la cantidad de daño según el diseño
        }
    }
}
