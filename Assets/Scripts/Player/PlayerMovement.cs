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

        private Rigidbody2D rbody2D;
        private Vector2 movementInput;

        private void Start()
        {
            rbody2D = GetComponent<Rigidbody2D>();
            inputReader.MovementEvent += HandleMovement;
        }

        private void OnDestroy()
        {
            inputReader.MovementEvent -= HandleMovement;
        }

        private void FixedUpdate()
        {
            ManageMovement(Time.fixedDeltaTime);
        }

        private void ManageMovement(float fixedDeltaTime)
        {
            rbody2D.velocity = movementInput * fixedDeltaTime * movementSpeed;
        }

        private void HandleMovement(Vector2 movement)
        {
            movementInput = movement;
        }
    }
}