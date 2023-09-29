using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private float movementSpeed = 100f;
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerHealth health;

        private Rigidbody2D rbody2D;
        private Vector2 movementInput;
        private bool isAlive = true;

        private void Start()
        {
            rbody2D = GetComponent<Rigidbody2D>();
            inputReader.MovementEvent += HandleMovement;
            health.OnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            inputReader.MovementEvent -= HandleMovement;
            health.OnDeath -= OnDeath;
        }

        private void FixedUpdate()
        {
            ManageMovement(Time.fixedDeltaTime);
        }

        private void Update()
        {
            ManageAnimatorParameters();
        }

        private void OnDeath()
        {
            isAlive = false;
        }

        private void ManageAnimatorParameters()
        {
            if (!isAlive)
            {
                return;
            }
            animator.SetFloat("MoveSpeed", animator.GetFloat("AnimationSpeedMultiplier") * rbody2D.velocity.magnitude);
        }

        private void ManageMovement(float fixedDeltaTime)
        {
            if(!isAlive)
            {
                return;
            }
            rbody2D.velocity = movementInput * fixedDeltaTime * movementSpeed;
        }

        private void HandleMovement(Vector2 movement)
        {
            movementInput = movement;
        }
    }
}