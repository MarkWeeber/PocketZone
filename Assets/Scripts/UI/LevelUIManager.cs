using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class LevelUIManager : MonoBehaviour
    {
        public InventoryUI InventoryUI;

        private static LevelUIManager instance;
        public static LevelUIManager Instance { get => instance; }

        private void Awake()
        {
            instance = this;
        }
    }
}