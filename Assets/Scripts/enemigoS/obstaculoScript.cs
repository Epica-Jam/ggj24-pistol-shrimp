using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstaculoScript : MonoBehaviour
{
    [SerializeField]
    private float _obsSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * _obsSpeed * Time.deltaTime);

        if (transform.position.x < -10f) // se destruye despues del valor
        {
            Destroy(this.gameObject);
        }
    }
}
