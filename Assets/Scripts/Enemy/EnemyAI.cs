using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TriggerHandler triggerHandler;
        [SerializeField] private Rigidbody2D rbody2D;
        [SerializeField] private MeleeDamageEffector meleeDamager;
        [SerializeField] private Animator animator;
        [SerializeField] private EnemyHealth health;
        [SerializeField] private Transform modelTransform;
        [Header("Settings")]
        [SerializeField] private float moveSpeed = 80f;
        [SerializeField] private float damageValue = 20f;
        [SerializeField] private float reachDistance = 1f;
        [SerializeField] private float attackDuration = 1f;

        private Transform targetTransform;
        private Vector2 directionToTarget;
        private float distanceToTarget;
        private float attackTimer;
        private float meleeDamagerOffsetDistance;
        private bool targetFound;
        private bool lookLeft;
        private bool isAlive = true;

        private void Awake()
        {
            meleeDamager.DamageValue = damageValue;
            triggerHandler.OnTriggerEnter += OnTargetEnterSight;
            health.OnDeath += OnDeath;
            meleeDamagerOffsetDistance = Vector2.Distance(meleeDamager.transform.position, transform.position);
            health.OnTakeDamage += HandleDamageTaken;
        }
        private void OnDestroy()
        {
            triggerHandler.OnTriggerEnter -= OnTargetEnterSight;
            health.OnDeath -= OnDeath;
            health.OnTakeDamage -= HandleDamageTaken;
        }

        private void Update()
        {
            if (!targetFound || !isAlive)
            {
                return;
            }
            HandleFaceFlip();
            HandleMeleeDamagerPlaceTowardsToTarget();
            if (attackTimer >= 0)
            {
                attackTimer -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            if (!targetFound || !isAlive)
            {
                return;
            }
            HandleMoveTowardsTarget();
        }

        private void HandleDamageTaken()
        {
            PlayerInventory _playerInventory = FindObjectOfType<PlayerInventory>();
            if (_playerInventory != null)
            {
                targetTransform = _playerInventory.transform;
                targetFound = true;
            }
        }

        private void OnTargetEnterSight(Collider2D targetCollider)
        {
            targetTransform = targetCollider.transform;
            targetFound = true;
        }

        private void OnDeath()
        {
            isAlive = false;
            rbody2D.velocity = Vector2.zero;
        }

        private void HandleMoveTowardsTarget()
        {
            directionToTarget = targetTransform.position - transform.position;
            distanceToTarget = directionToTarget.magnitude;
            if (distanceToTarget <= reachDistance)
            {
                rbody2D.velocity = Vector2.zero;
                animator.SetBool("Attacking", true);
                animator.SetFloat("MoveSpeed", 0);
                attackTimer = attackDuration;
            }
            else if (attackTimer < 0f)
            {
                rbody2D.velocity = directionToTarget.normalized * moveSpeed * Time.fixedDeltaTime;
                animator.SetBool("Attacking", false);
                animator.SetFloat("MoveSpeed", rbody2D.velocity.magnitude);
            }
        }

        private void HandleFaceFlip()
        {
            float _xDirection = targetTransform.position.x - transform.position.x;
            if (_xDirection < 0 && !lookLeft)
            {
                modelTransform.Rotate(0f, 180f, 0f);
                lookLeft = true;
            }
            else if (_xDirection >= 0 && lookLeft)
            {
                modelTransform.Rotate(0f, 180f, 0f);
                lookLeft = false;
            }
        }

        private void HandleMeleeDamagerPlaceTowardsToTarget()
        {
            meleeDamager.transform.position = (Vector2)transform.position + directionToTarget.normalized * meleeDamagerOffsetDistance;
        }
    }
}