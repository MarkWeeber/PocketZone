using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [Header("Settings")]
        [SerializeField] private float aimLerprate = 20f;
        [SerializeField] private bool aimAlongDirection = true;

        private Vector2 aimDirection;
        private bool lookLeft;
        public Vector2 test;
        private float distance;
        private bool isAlive = true;

        private void Start()
        {
            inputReader.MovementEvent += HandleMovement;
            distance = Vector2.Distance(aimSpot.position, aimSpotParent.position);
            health.OnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            inputReader.MovementEvent -= HandleMovement;
            health.OnDeath -= OnDeath;
        }

        private void Update()
        {
            SetAimSpotParentRotation(Time.deltaTime);
            HandleFaceFlip();
        }

        private void HandleFaceFlip()
        {
            if (!isAlive)
            {
                return;
            }
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

        private void SetAimSpotParentRotation(float deltaTime)
        {
            if (!isAlive)
            {
                return;
            }
            if (aimAlongDirection)
            {
                Vector2 newPosition = (Vector2)aimSpotParent.position
                + ((aimDirection == Vector2.zero) ? (Vector2)aimSpotParent.right : aimDirection)
                * distance;
                aimSpot.position = Vector2.Lerp(aimSpot.position, newPosition, deltaTime * aimLerprate);
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
    }
}