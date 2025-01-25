using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject[] _enemigoPrefabs; // Prefabs de enemigos
    [SerializeField]
    private GameObject[] _obstaculoPrefabs; // Prefabs de obstáculos
    [SerializeField]
    private GameObject[] _powerupPrefabs; // Prefabs de powerups

    [Header("config Spawns")]
    [SerializeField]
    private float spawnXPos = 10f; // Posición de spawn en X (fuera de la pantalla)
    [SerializeField]
    private float minYPos = -4f; // Límite inferior vertical
    [SerializeField]
    private float maxYPos = 4f; // Límite superior vertical

    [SerializeField]
    private float intervaloSpawnEnemigos = 3f; // Intervalo de spawn para enemigos
    [SerializeField]
    private float intervaloSpawnObjetos = 4f; // Intervalo de spawn para obstáculos
    [SerializeField]
    private float intervaloMinPowerup = 8f; // Tiempo mínimo entre powerups
    [SerializeField]
    private float intervaloMaxPowerup = 12f; // Tiempo máximo entre powerups

    private bool pararSpawn = false; // Dejar de spawnear?

    // Start is called before the first frame update
    void Start()
    {
        SpawnStart();
    }

    public void SpawnStart()
    {
        StartCoroutine(SpawnEnemigosRoutine());
        StartCoroutine(SpawnObstaculosRoutine());
        StartCoroutine(SpawnPowerupsRoutine());
    }

    IEnumerator SpawnEnemigosRoutine()
    {
        yield return new WaitForSeconds(2.0f); // Pequeño retraso inicial
        while (!pararSpawn)
        {
            SpawnearObj(_enemigoPrefabs);
            yield return new WaitForSeconds(intervaloSpawnEnemigos);
        }
    }

    IEnumerator SpawnObstaculosRoutine()
    {
        yield return new WaitForSeconds(1.0f); 
        while (!pararSpawn)
        {
            SpawnearObj(_obstaculoPrefabs);
            yield return new WaitForSeconds(intervaloSpawnObjetos);
        }
    }

    IEnumerator SpawnPowerupsRoutine()
    {
        yield return new WaitForSeconds(5.0f); 
        while (!pararSpawn)
        {
            float interval = Random.Range(intervaloMinPowerup, intervaloMaxPowerup);
            SpawnearObj(_powerupPrefabs);
            yield return new WaitForSeconds(interval);
        }
    }

    private void SpawnearObj(GameObject[] prefabs)
    {
        if (prefabs.Length == 0) return;

        // Seleccionar un prefab aleatorio del conjunto
        int randomIndex = Random.Range(0, prefabs.Length);

        // Generar posición aleatoria de spawn
        Vector3 spawnPos = new Vector3(
            spawnXPos,
            Random.Range(minYPos, maxYPos),
            0);

        // Instanciar el objeto
        Instantiate(prefabs[randomIndex], spawnPos, Quaternion.identity);
    }

    public void StopSpawning() // Para dejar de spawnear
    {
        pararSpawn = true;
    }
}
