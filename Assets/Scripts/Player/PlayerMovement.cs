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
        private bool isActive = true;

        private void Start()
        {
            rbody2D = GetComponent<Rigidbody2D>();
            inputReader.MovementEvent += HandleMovement;
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDestroy()
        {
            inputReader.MovementEvent -= HandleMovement;
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void FixedUpdate()
        {
            ManageMovement(Time.fixedDeltaTime);
        }

        private void Update()
        {
            ManageAnimatorParameters();
        }

        private void ManageAnimatorParameters()
        {
            if (!isActive)
            {
                return;
            }
            animator.SetFloat("MoveSpeed", animator.GetFloat("AnimationSpeedMultiplier") * rbody2D.velocity.magnitude);
        }

        private void ManageMovement(float fixedDeltaTime)
        {
            if (!isActive)
            {
                return;
            }
            rbody2D.velocity = movementInput * fixedDeltaTime * movementSpeed;
        }

        private void HandleMovement(Vector2 movement)
        {
            movementInput = movement;
        }

        private void HandleGameStateChanged(GameState state)
        {
            if (state == GameState.LevelComplete || state == GameState.PlayerDeadInGame)
            {
                rbody2D.velocity = Vector2.zero;
                animator.SetFloat("MoveSpeed", animator.GetFloat("AnimationSpeedMultiplier") * rbody2D.velocity.magnitude);
                isActive = false;
            }
        }
    }
}