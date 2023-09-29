using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class TriggerHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private string targetTag;
        [SerializeField] private bool activateOnTriggerEnter;
        [SerializeField] private bool activateOnTriggerStay;
        [SerializeField] private bool activateOnTriggerExit;
        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerStay;
        public event Action<Collider2D> OnTriggerExit;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!activateOnTriggerEnter)
            {
                return;
            }
            if (Utils.CheckLayer(targetMask, collision.gameObject.layer))
            {
                if (collision.gameObject.tag == targetTag)
                {
                    OnTriggerEnter?.Invoke(collision);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!activateOnTriggerStay)
            {
                return;
            }
            if (Utils.CheckLayer(targetMask, collision.gameObject.layer))
            {
                if (collision.gameObject.tag == targetTag)
                {
                    OnTriggerStay?.Invoke(collision);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!activateOnTriggerExit)
            {
                return;
            }
            if (Utils.CheckLayer(targetMask, collision.gameObject.layer))
            {
                if (collision.gameObject.tag == targetTag)
                {
                    OnTriggerExit?.Invoke(collision);
                }
            }
        }
    }
}