using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public string m_name;
        float m_hp;
        public float m_maxHp;
        public float m_shieldHp;
        public float m_shieldMaxHp;
        public float m_movementSpeed;
        public float m_idleTime;
        public Projectile[] m_projectiles;
        Transform m_target;
        Vector3 m_originalPos;


        void Start()
        {
            m_hp = m_maxHp;
            m_originalPos = transform.position;
        }


        private void Update()
        {
            UpdateMovement();
        }

        void UpdateMovement()
        {
            if (m_target == null) transform.Translate(m_movementSpeed * Time.deltaTime * Vector2.left);
            else transform.position = Vector3.MoveTowards(transform.position, m_target.position, Time.deltaTime * m_movementSpeed);
            //if (m_isExplosive)
            //{
            //    if (Vector3.Distance(transform.position, m_originalPos) > m_explosionDistance)
            //    {
            //        Explode(); return;
            //    }
            //}
            if (!Camera.main.rect.Contains(transform.position)) Destroy(this);

        }
    }
}