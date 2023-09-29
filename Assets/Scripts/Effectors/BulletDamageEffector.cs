using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletDamageEffector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rbody2d;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TrailRenderer trailRenderer;
        [Header("Settings")]
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float damageValue = 15f;
        [SerializeField] private float damageRayDistance = 0.25f;
        [SerializeField] private float aliveTime = 5f;
        [SerializeField] private string targetTag;

        public float DamageValue { get => damageValue; set => damageValue = value; }
        private Collider2D ignoreCollision2D;

        private IHealth health = null;
        private RaycastHit2D[] hit;
        private bool active = false;
        private float aliveTimer;

        private void Update()
        {
            ManageRaycastHit();
        }

        public void SendBullet(Vector2 position, Vector2 direction, float velocity, Collider2D ignoreCollision2D = null)
        {
            this.ignoreCollision2D = ignoreCollision2D;
            transform.position = position;
            rbody2d.velocity = direction * velocity;
            transform.right = direction;
            aliveTimer = aliveTime;
            spriteRenderer.enabled = true;
            trailRenderer.enabled = true;
            active = true;
        }

        public void ReturnBullet()
        {
            active = false;
            spriteRenderer.enabled = false;
            trailRenderer.enabled = false;
            rbody2d.velocity = Vector2.zero;
        }

        private void ManageRaycastHit()
        {
            if (active)
            {
                hit = Physics2D.RaycastAll(transform.position - (transform.right * damageRayDistance), transform.right, damageRayDistance, targetMask);
                foreach (RaycastHit2D _hit in hit)
                {
                    if (ignoreCollision2D != null && _hit.collider == ignoreCollision2D)
                    {
                        continue;
                    }
                    if (_hit.collider.gameObject.tag == targetTag)
                    {
                        if (_hit.collider.gameObject.TryGetComponent<IHealth>(out health))
                        {
                            if (health.IsAlive)
                            {
                                health.TakeDamage(damageValue);
                                ReturnBullet();
                                break;
                            }
                            else
                            {
                                continue;
                            }
                            
                        }
                    }
                    ReturnBullet();
                }
                if (aliveTimer < 0)
                {
                    ReturnBullet();
                }
                aliveTimer -= Time.deltaTime;
            }
        }
    }
}