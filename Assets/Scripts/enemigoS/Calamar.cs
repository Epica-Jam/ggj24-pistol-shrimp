using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calamar : MonoBehaviour
{
    enum AttackType
    {
        None,
        Melee,
        Underground
    }
    public float m_maxHp = 100f;
    float m_hp;
    public bool m_idle = false;
    public float m_idleTime = 0f;
    float m_minIdleTime = 2f;
    float m_maxIdleTime = 5f;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public PolygonCollider2D rootCollider;
    public int colliderStep = 0;
    AttackType m_lastAttack = AttackType.None;

    public GameObject musica;

    void Start()
    {
        spawnerScript.Instance.StopSpawning();
        m_hp = m_maxHp;
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        musica = GameObject.Find("MusicManager");
        musica.GetComponent<MusicManager>().PlayBossOST();
    }

    // Update is called once per frame
    void Update()
    {
        m_idleTime += Time.deltaTime;
        if (m_idle && m_idleTime > Random.Range(m_minIdleTime, m_maxIdleTime))
            if (m_lastAttack == AttackType.None) PlayMeleeAttack();
            else PlayRandomAttack();
    }
    void PlayRandomAttack()
    {
        if (m_lastAttack == AttackType.Underground) { PlayMeleeAttack(); return; }
        if (Random.Range(0, 100) < 20) PlayMeleeAttack();
        else PlayUndergroundAttack();
    }

    void PlayMeleeAttack()
    {
        animator.SetTrigger("Melee");
        m_lastAttack = AttackType.Melee;
    }

    void PlayUndergroundAttack()
    {
        animator.SetTrigger("Underground");
        m_lastAttack = AttackType.Underground;

    }
    public void UpdateCollider()
    {
        List<Vector2> physicsShape = new List<Vector2>();
        spriteRenderer.sprite.GetPhysicsShape(0, physicsShape);
        Vector2[] tempArray = physicsShape.ToArray();
        rootCollider.SetPath(0, tempArray);
    }
    public void SetIdle(int idle)
    {
        if (idle == 2) { m_idle = true; return; }
        m_idle = idle == 1;
        if (m_idle) m_idleTime = 0f;
    }
    public void SetColliderStep(int step)
    {
        bool newIdle = spriteRenderer.sprite.name.Contains("calamar-right");
        if (!m_idle && newIdle) m_idleTime = 0f;
        m_idle = newIdle;
        colliderStep = step;
        UpdateCollider();
    }
    IEnumerator DamageFlicker()
    {
        for (int i = 0; i < 6; i++)
        {
            if (spriteRenderer.color == Color.white) spriteRenderer.color = Color.red;
            else spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Burbuja")) return;
        if (other.GetComponent<burbuscript>().esTrampa == false)
        {
            Destroy(other.gameObject);
            if (playerScript.Instance != null)
            {
                playerScript.Instance.AddPuntos(50);
            }
            StartCoroutine(DamageFlicker());
            m_hp -= 1f;
            if (m_hp <= 0)
            {
                if (playerScript.Instance != null)
                {
                    playerScript.Instance.AddPuntos(1000);
                }
                UIscript.Instance.ShowCredits();
                Destroy(this.gameObject);
            }
        }
        else if (other.GetComponent<burbuscript>().esTrampa == true && other.GetComponent<burbuscript>()._carga < 1)
        {
            Destroy(other.gameObject);
        }

    }
}
