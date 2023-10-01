using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class EnemyHealth : PlayerHealth
    {
        private Collider2D col;
        public event Action OnTakeDamage;
        public event Action<EnemyHealth> OnEnemyDeath;
        protected override void Awake()
        {
            base.Awake();
            col = GetComponent<Collider2D>();
        }
        protected override void Die()
        {
            col.enabled = false;
            OnEnemyDeath?.Invoke(this);
            base.Die();
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            OnTakeDamage?.Invoke();
        }
    }
}