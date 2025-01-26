using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fondoManager : MonoBehaviour
{
    [SerializeField]
    public float scrollSpeed = 2f; // Velocidad del fondo
    [SerializeField]
    public float backgroundWidth = 1200f; // Ancho del fondo (ajustar)
    private Transform[] backgrounds; // Array para guardar los fondos
    // Start is called before the first frame update
    void Start()
    {
        // Obtén todos los hijos de este GameObj como fondos
        backgrounds = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            backgrounds[i] = transform.GetChild(i);
        }
        backgroundWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
       // Mueve los fondos
        foreach (Transform background in backgrounds)
        {
            background.position += Vector3.left * scrollSpeed * GameManager.Instance.m_movementSpeedMultiplier * Time.deltaTime;

            // Reposicion cuando está fuera de la cámara
            if (background.position.x < -backgroundWidth)
            {
                float limiteX = sacarLimX();
                background.position = new Vector3(limiteX + backgroundWidth, background.position.y, background.position.z);
            }
        }
    }

    float sacarLimX()
    {
        float limX = float.MinValue;
        foreach (Transform background in backgrounds)
        {
            if (background.position.x > limX)
            {
                limX = background.position.x;
            }
        }
        return limX;
    }
}
