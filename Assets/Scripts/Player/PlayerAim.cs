using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace PocketZone.Space
{
    public class PlayerAim : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Transform aimSpotParent;
        [SerializeField] private Transform aimSpot;
        [SerializeField] private Transform modelTransform;
        [SerializeField] private PlayerHealth health;
        [SerializeField] private TriggerHandler triggerHandler;
        [Header("Settings")]
        [SerializeField] private float aimLerprate = 20f;
        
        private Vector2 aimDirection;
        private bool lookLeft;
        private float distance;
        private bool isAlive = true;
        private bool targetOnSight;
        private List<EnemyHealth> enemiesHealthList;
        private EnemyHealth enemyHealth;
        private Transform targettedEnemy;

        private void Start()
        {
            enemiesHealthList = new List<EnemyHealth>();
            inputReader.MovementEvent += HandleMovement;
            distance = Vector2.Distance(aimSpot.position, aimSpotParent.position);
            health.OnDeath += OnDeath;
            triggerHandler.OnTriggerEnter += HandleEnemySeen;
            triggerHandler.OnTriggerExit += HandleEnemyLost;
        }

        private void OnDestroy()
        {
            inputReader.MovementEvent -= HandleMovement;
            health.OnDeath -= OnDeath;
            triggerHandler.OnTriggerEnter -= HandleEnemySeen;
            triggerHandler.OnTriggerExit -= HandleEnemyLost;
        }

        private void Update()
        {
            if (!isAlive)
            {
                return;
            }
            HandleEnemyTargeting();
            SetAimSpotParentRotation();
            HandleFaceFlip();
        }

        private void HandleEnemyTargeting()
        {
            float _minHealth = 101f;
            targetOnSight = false;
            foreach (EnemyHealth _enemyHealth in enemiesHealthList)
            {
                if(_enemyHealth.IsAlive)
                {
                    if (_minHealth > _enemyHealth.CurrentHealth)
                    {
                        _minHealth = _enemyHealth.CurrentHealth;
                        targettedEnemy = _enemyHealth.transform;
                        targetOnSight = true;
                    }
                }
            }
        }

        private void HandleFaceFlip()
        {
            float _delta = aimSpot.position.x - aimSpotParent.position.x;
            if (_delta < 0 && !lookLeft)
            {
                modelTransform.Rotate(0f, 180f, 0f);
                lookLeft = true;
            }
            else if (_delta >= 0 && lookLeft)
            {
                modelTransform.Rotate(0f, 180f, 0f);
                lookLeft = false;
            }
        }

        private void SetAimSpotParentRotation()
        {
            if (targetOnSight)
            {
                aimSpot.position = targettedEnemy.position;
            }
            else
            {
                Vector2 newPosition = (Vector2)aimSpotParent.position
                + ((aimDirection == Vector2.zero) ? (Vector2)aimSpotParent.right : aimDirection)
                * distance;
                aimSpot.position = Vector2.Lerp(aimSpot.position, newPosition, Time.deltaTime * aimLerprate);
            }
        }

        private void OnDeath()
        {
            isAlive = false;
        }

        private void HandleMovement(Vector2 moveDirection)
        {
            if (moveDirection.magnitude > 0.01f)
            {
                aimDirection = moveDirection.normalized;
            }
        }

        private void HandleEnemySeen(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent<EnemyHealth>(out enemyHealth))
            {
                if (enemyHealth.IsAlive)
                {
                    if (!enemiesHealthList.Contains(enemyHealth))
                    {
                        enemiesHealthList.Add(enemyHealth);
                    }
                }
            }
        }

        private void HandleEnemyLost(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent<EnemyHealth>(out enemyHealth))
            {
                if (enemyHealth.IsAlive)
                {
                    enemiesHealthList.Remove(enemyHealth);
                }
            }
        }
    }
}