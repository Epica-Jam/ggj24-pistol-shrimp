using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    void Start()
    {
        if (gameObject.tag != "Powerup") gameObject.tag = "Powerup";
        m_remainingTime = m_duration;
    }
    void SetPlayer(playerScript p) => player = p;
    public float GetRemainingTime () => m_remainingTime;
    private void Update()
    {
        //if (!gameObject.activeInHierarchy) return;
        UpdateMovement();
    }

    void UpdateMovement()
    {
        transform.Translate(m_movementSpeed * Time.deltaTime * Vector2.left);
        //if (!Camera.main.rect.Contains(transform.position)) Destroy(this);
    }

    public void RunPowerUp()
    {
        m_remainingTime -= Time.deltaTime;
    }


}
