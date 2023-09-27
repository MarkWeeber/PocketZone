using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Transform aimSpotParent;
        [SerializeField] private Transform aimSpot;
        [SerializeField] private Transform modelTransform;
        [SerializeField] private float aimLerprate = 20f;
        [SerializeField] private bool aimAlongDirection = true;

        private Vector2 aimDirection;
        private bool lookLeft;
        public Vector2 test;
        private float distance;

        private void Start()
        {
            inputReader.MovementEvent += HandleMovement;
            distance = Vector2.Distance(aimSpot.position, aimSpotParent.position);
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
            float _delta = aimSpot.position.x - aimSpotParent.position.x;
            if (_delta < 0 && !lookLeft)
            {
                modelTransform.Rotate(0f, 180f, 0f);
                lookLeft = true;
            }
            else if(_delta >= 0 && lookLeft)
            {
                modelTransform.Rotate(0f, 180f, 0f);
                lookLeft = false;
            }
        }

        private void SetAimSpotParentRotation(float deltaTime)
        {
            if(aimAlongDirection)
            {
                Vector2 newPosition = (Vector2)aimSpotParent.position
                + ((aimDirection == Vector2.zero) ? (Vector2)aimSpotParent.right : aimDirection)
                * distance;
                aimSpot.position = Vector2.Lerp(aimSpot.position, newPosition, deltaTime * aimLerprate);
            }
            
            //aimSpot.RotateAround(aimSpotParent.position + microAdjustment, aimSpotParent.right, angle);
            //return;
            //angle = Vector2.Angle(aimSpotParent.right, aimDirection);
            //if (angle <= 180f && angle > 178f)
            //{
            //    microAdjustment = new Vector2(0.01f, 0.01f);
            //}
            //else
            //{
            //    microAdjustment = Vector2.zero;
            //}
            //aimSpotParent.right = Vector2.Lerp(aimSpotParent.right + microAdjustment, aimDirection, aimLerprate * deltaTime);
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