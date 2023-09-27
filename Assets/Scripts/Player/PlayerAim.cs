using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Transform aimSpotParent;
        [SerializeField] private Transform modelTransform;
        [SerializeField] private float aimLerprate = 20f;

        private Vector2 aimDirection;
        private Vector3 microAdjustment;
        private float angle;
        private bool lookLeft;

        private void Start()
        {
            inputReader.MovementEvent += HandleMovement;
        }

        private void OnDestroy()
        {
            inputReader.MovementEvent -= HandleMovement;
        }

        private void Update()
        {
            SetAimSpotParentRotation(Time.deltaTime);
            HandleFaceFlip();
        }

        private void HandleFaceFlip()
        {
            if (aimSpotParent.right.x < 0 && !lookLeft)
            {
                modelTransform.Rotate(0f, 180f, 0f);
                lookLeft = true;
            }
            else if(aimSpotParent.right.x >= 0 && lookLeft)
            {
                modelTransform.Rotate(0f, 180f, 0f);
                lookLeft = false;
            }
        }

        private void SetAimSpotParentRotation(float deltaTime)
        {
            angle = Vector2.Angle(aimSpotParent.right, aimDirection);
            if (angle <= 180f && angle > 178f)
            {
                microAdjustment = new Vector2(0.01f, 0.01f);
            }
            else
            {
                microAdjustment = Vector2.zero;
            }
            aimSpotParent.right = Vector2.Lerp(aimSpotParent.right + microAdjustment, aimDirection, aimLerprate * deltaTime);
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