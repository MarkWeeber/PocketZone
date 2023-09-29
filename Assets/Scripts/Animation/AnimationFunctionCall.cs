using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PocketZone.Space
{
    public class AnimationFunctionCall : MonoBehaviour
    {
        public UnityEvent ActionEvent;

        public void Activate()
        {
            ActionEvent?.Invoke();
        }
    }
}