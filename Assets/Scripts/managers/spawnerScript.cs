using System.Collections;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    public static spawnerScript Instance { get; private set; }


    [Header("Prefabs")]
    [SerializeField]
    private GameObject[] _enemigoPrefabs; // Prefabs de enemigos
    [SerializeField]
    private GameObject[] _obstaculoPrefabs; // Prefabs de obstáculos
    [SerializeField]
    private GameObject[] _powerupPrefabs; // Prefabs de powerups
    [SerializeField]
    private GameObject[] _bossPrefabs;

    [Header("config Spawns")]
    [SerializeField]
    private float spawnXPos = 10f; // Posición de spawn en X (fuera de la pantalla)
    [SerializeField]
    private float minYPos = -4f; // Límite inferior vertical
    [SerializeField]
    private float maxYPos = 4f; // Límite superior vertical

    public float intervaloSpawnEnemigos = 3f; // Intervalo de spawn para enemigos
    public float intervaloSpawnObjetos = 4f; // Intervalo de spawn para obstáculos
    public float intervaloMinPowerup = 8f; // Tiempo mínimo entre powerups
    public float intervaloMaxPowerup = 12f; // Tiempo máximo entre powerups

    private bool pararSpawn = false; // Dejar de spawnear?



    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

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

    public void SpawnBoss()
    {
        SpawnearObj(_bossPrefabs);
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
