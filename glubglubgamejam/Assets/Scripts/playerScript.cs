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

    // ataque

    [SerializeField]
    private float tiempoCarga = 0f; // T0 de carga
    private bool cargando = false; // Bool de si esta cargando o no
    private float cargaMax = 2f; // carga maxima posible
    private float tamMax = 3f; // tamaño maximo de la burbuja cargada

    // referencias ataque

    [SerializeField]
    private Transform pistola; // Transform del objeto hijo pistola, para que los disparos usen su posicion como referencia
    [SerializeField]
    private GameObject burbuCprefab; // Prefab de la burbuja chica
    [SerializeField]
    private GameObject burbuGprefab; // Prefab de la burbuja grande

    // Start is called before the first frame update
    void Start()
    {
        player_rb = GetComponent<Rigidbody2D>();
        player_rb.gravityScale = gravedad;
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento(); // movimiento :)

        // Ataques

        if (Input.GetKeyDown(KeyCode.Mouse0)) // Disparo ligero
        {
            burbuChica();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) // Disparo cargado, deteccion de que se empezo a cargar
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
        Instantiate(burbuCprefab, pistola.position, Quaternion.identity);
    }

    void burbuGrande()
    {
        GameObject disparo = Instantiate(burbuGprefab, pistola.position, Quaternion.identity);

        float escalado = Mathf.Lerp(0.4f, tamMax, tiempoCarga/cargaMax); // Interpolacion tamaños

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

    
}
