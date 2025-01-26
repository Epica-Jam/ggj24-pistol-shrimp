using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public AnimationCurve m_experienceCurve;
    public bool m_gameOver = false;
    public int m_level = 1;
    public float m_gameTime = 0f;
    public int m_score = 0;
    public int m_firstLevelScore = 1000;
    float m_nextLevelScore;
    public float m_nextLevelFactor = 1.1f;
    public float m_movementSpeedMultiplier = 1f;
    public float m_movementSpeedIncrease = 0.1f;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_nextLevelScore = m_firstLevelScore;
    }

    // Update is called once per frame
    void Update()
    {
        m_gameTime += Time.deltaTime;

    }

    public void AddPoints(int points)
    {
        m_score += points;
        if (m_score < m_nextLevelScore) return;
        m_level++;
        m_nextLevelScore = m_nextLevelScore + m_nextLevelScore * m_nextLevelFactor;
        m_movementSpeedMultiplier += m_movementSpeedIncrease;
        spawnerScript.Instance.intervaloSpawnObjetos -= 0.2f;
        spawnerScript.Instance.intervaloSpawnEnemigos -= 0.2f;
        UIscript.Instance.UpdateLevel();
        if (m_level == 5) spawnerScript.Instance.SpawnBoss();
    }

    public void GameOver()
    {
        MusicManager.Instance.PlayGameOverOST();
    }
}
