using UnityEngine;

public enum PowerUpType
{
    None,
    AutoShoot
}
public class PowerUp : MonoBehaviour
{
    playerScript player;
    public float m_movementSpeed;
    public PowerUpType m_type;
    public float m_duration;
    float m_remainingTime;
    public bool m_active;
    void Start()
    {
        if (gameObject.tag != "Powerup") gameObject.tag = "Powerup";
        m_remainingTime = m_duration;
    }
    void SetPlayer(playerScript p) => player = p;
    public float GetRemainingTime() => m_remainingTime;
    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        UpdateMovement();
    }

    public void SetActive(bool active)
    {
        m_active = active;
        if (!m_active) Destroy(gameObject, 1); 
    }

    void UpdateMovement()
    {
        transform.Translate(m_movementSpeed * Time.deltaTime * GameManager.Instance.m_movementSpeedMultiplier * Vector2.left);
        //if (!Camera.main.rect.Contains(transform.position)) Destroy(this);
    }

    public void RunPowerUp()
    {
        m_remainingTime -= Time.deltaTime;
    }
}
