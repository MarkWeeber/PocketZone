using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class EnemyGroupAlerter : MonoBehaviour
    {
        public event Action<Transform> OnAlert;
        public void Alert(Transform target)
        {
            OnAlert?.Invoke(target);
        }
    }
}