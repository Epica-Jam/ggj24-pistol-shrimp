using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemigoScript : MonoBehaviour
{
    // 0 = coral
    // 1 = pez globo
    // 2 = anguila

    [SerializeField]
    private int _enemigoID;
    public float _hpEnemigo;
    private SpriteRenderer spriteRenderer;

    private playerScript _player;
    private float targetY; // Posicion eje Y del jugador
    private float vertSpeed = 2f; // Velocidad de movimiento vertical

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.Find("player").GetComponent<playerScript>();

        if (_enemigoID == 0)
        {
            transform.position = new Vector2(10, -4);
            _hpEnemigo = 5f;
        }
        if (_enemigoID == 1)
        {
            _hpEnemigo = 3f;

            if (_player != null)
            {
                targetY = _player.transform.position.y;
            }
            else
            {
                targetY = transform.position.y; // Si no hay jugador que quede en su posición actual
            }
        }
        if (_enemigoID == 2)
        {
            _hpEnemigo = 2f;
            if (_player != null)
            {
                targetY = _player.transform.position.y;
            }
            else
            {
                targetY = transform.position.y; // Si no hay jugador que quede en su posición actual
            }
            transform.position = new Vector2(10, targetY);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemigoID == 0)
        {
            transform.Translate(Vector2.left * 3f * GameManager.Instance.m_movementSpeedMultiplier * Time.deltaTime);
        }
        else if (_enemigoID == 1)
        {
            transform.Translate(Vector2.left * 4f * GameManager.Instance.m_movementSpeedMultiplier * Time.deltaTime);

            // Movimiento vertical
            float step = vertSpeed * Time.deltaTime;
            transform.position = new Vector2(
                transform.position.x,
                Mathf.MoveTowards(transform.position.y, targetY, step)
            );
        }
        else if (_enemigoID == 2)
        {
            transform.Translate(Vector2.left * 7f * GameManager.Instance.m_movementSpeedMultiplier * Time.deltaTime);
        }
        else
        {
            Debug.LogError("ID invalida o no asignada correctamente.");
        }

        // Destruir si sale del rango vertical
        if (transform.position.y < -7f)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator DamageFlicker()
    {
        for (int i = 0; i < 6; i++)
        {
            if (spriteRenderer.color == Color.white) spriteRenderer.color = Color.red;
            else spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Burbuja")) return;
        Destroy(other.gameObject);
        if (_player != null)
        {
            _player.AddPuntos(50);
        }
        StartCoroutine(DamageFlicker());
        _hpEnemigo -=1f;
        if (_hpEnemigo <= 0)
        {
            if (_player != null)
            {
                _player.AddPuntos(100);
            }
            Destroy(this.gameObject);
        }
    }
}
