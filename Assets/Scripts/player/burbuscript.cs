using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class burbuscript : MonoBehaviour
{
    [SerializeField]
    private float _dispSpeed = 7f;
    public bool esTrampa = false;
    public float _carga;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * _dispSpeed * Time.deltaTime);

        if (transform.position.x > 10f) // se destruye despues del valor
        {
            Destroy(this.gameObject);
        }
    }
}
