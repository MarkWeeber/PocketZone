using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PocketZone.Space
{
    public class CustomEvent : MonoBehaviour
    {
        public UnityEvent OnAwake;
        public UnityEvent OnStart;

        private void Awake()
        {
            OnAwake?.Invoke();   
        }

        private void Start()
        {
            OnStart?.Invoke();
        }
    }
}