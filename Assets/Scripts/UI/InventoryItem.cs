using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PocketZone.Space
{
    public class InventoryItem : MonoBehaviour
    {
        public event Action Action;
        public ICollectible Collectible;
        public TMP_Text Text;
        public Image image;
        public void Activate()
        {
            Action?.Invoke();
        }
    }
}