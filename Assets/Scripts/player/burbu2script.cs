using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class burbu2script : MonoBehaviour
{
    private enemigoScript enemigoOriginal;
    private playerScript player;
    [SerializeField]
    private float escalaMiniEnemy = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").GetComponent<playerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Configurar(enemigoScript enemigo)
    {
        enemigoOriginal = enemigo;

        GetComponent<SpriteRenderer>().sprite = enemigo.GetComponent<SpriteRenderer>().sprite;
        transform.localScale = enemigo.transform.localScale * escalaMiniEnemy; // Ajustar el tamaño
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Burbuja")) // Detecta si es un disparo del jugador
        {
            enemigoOriginal.Borrar(); // Mata al enemigo original
            Destroy(gameObject);     // Destruye la burbuja
            player.AddPuntos(150);
        }
    }
}
