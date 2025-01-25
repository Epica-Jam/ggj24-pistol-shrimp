using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 m_originalPos;
    public float m_projectileSpeed = 5f;
    public bool m_isExplosive = false;
    public float m_explosionDistance = 5f;
    public int m_explosionStage = 0;
    public Transform m_target;

    private void Start()
    {
        m_originalPos = transform.position;
    }

    private void Update()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        if (m_target == null) transform.Translate(m_projectileSpeed * Time.deltaTime * Vector2.left);
        else transform.position = Vector3.MoveTowards(transform.position, m_target.position, Time.deltaTime * m_projectileSpeed);
        //if (m_isExplosive)
        //{
        //    if (Vector3.Distance(transform.position, m_originalPos) > m_explosionDistance)
        //    {
        //        Explode(); return;
        //    }
        //}
        if (!Camera.main.rect.Contains(transform.position)) Destroy(this);
        
    }

    void Explode() { DoAreaDamage(); Destroy(this); }

    void DoAreaDamage()
    {

    }

}