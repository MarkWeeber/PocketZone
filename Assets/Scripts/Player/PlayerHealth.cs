using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PocketZone.Space
{
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        [SerializeField] protected float currentHealth = 100f;
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected Transform healthBarParent;
        [SerializeField] protected Image healthBarFillImage;
        [SerializeField] protected Animator animator;

        public UnityEvent OnTakeDamageUI;

        public bool IsAlive => isAlive;
        protected bool isAlive = true;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public event Action<float> HealthBarUpdate;
        public event Action OnDeath;

        protected virtual void Awake()
        {
            HealthBarUpdate += HandleHealthBarFillUpdate;
        }

        protected void OnDestroy()
        {
            HealthBarUpdate -= HandleHealthBarFillUpdate;
        }

        public void Heal(float healAmount)
        {
            if (!isAlive)
            {
                return;
            }
            currentHealth = Mathf.Clamp(currentHealth + healAmount, 0f, maxHealth);
            HealthBarUpdate?.Invoke(currentHealth / maxHealth);
        }

        public virtual void TakeDamage(float damage)
        {
            if(!isAlive)
            {
                return;
            }
            OnTakeDamageUI?.Invoke();
            currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
            HealthBarUpdate?.Invoke(currentHealth / maxHealth);
            if (currentHealth == 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            OnDeath?.Invoke();
            healthBarParent.gameObject.SetActive(false);
            animator.SetTrigger("Die");
            isAlive = false;
        }

        protected void HandleHealthBarFillUpdate(float rate)
        {
            healthBarFillImage.fillAmount = Mathf.Clamp(rate, 0f, 1f);
        }
    }
}