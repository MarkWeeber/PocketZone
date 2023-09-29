using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class MeleeDamageEffector : MonoBehaviour
    {
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float damageValue = 15f;
        [SerializeField] private float damageRadius = 1f;
        [SerializeField] private string targetTag;
        [SerializeField] private Transform damageDealPoint;
        [SerializeField] private bool drawGizmo = true;
        public float DamageValue { get => damageValue; set => damageValue = value; }
        private IHealth health = null;

        public void DealMeleeDamage()
        {
            Collider2D[] _colliders = Physics2D.OverlapCircleAll(damageDealPoint.position, damageRadius);
            foreach (Collider2D _collider in _colliders)
            {
                if(Utils.CheckLayer(targetMask, _collider.gameObject.layer))
                {
                    if (_collider.gameObject.tag == targetTag)
                    {
                        if (_collider.gameObject.TryGetComponent<IHealth>(out health))
                        {
                            health.TakeDamage(damageValue);
                        }
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (drawGizmo)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.position, damageRadius);
            }
        }
    }
}