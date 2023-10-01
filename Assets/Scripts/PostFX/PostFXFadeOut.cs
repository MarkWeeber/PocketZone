using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace PocketZone.Space
{
    [RequireComponent(typeof(Volume))]
    public class PostFXFadeOut : MonoBehaviour
    {
        [SerializeField] private float fadeOutRate = 4f;
        private Volume volume;
        private void Start ()
        {
            volume = GetComponent<Volume>();
        }

        private void Update ()
        {
            HandleFadeOut();
        }

        private void HandleFadeOut()
        {
            if (volume.weight > 0.01f)
            {
                volume.weight = Mathf.Lerp(volume.weight, 0f, fadeOutRate * Time.deltaTime);
            }
        }

        public void SetVolumeWeight(float weight)
        {
            volume.weight = weight;
        }

    }
}