using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pausaScript : MonoBehaviour
{
    public static bool Pausado = false;
    public GameObject pausaMenuUI;

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Pausado)
            {
                Resumir();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Resumir()
    {
        pausaMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Pausado = false;
    }

    public void Pausar()
    {
        pausaMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Pausado = true;
    }
}
