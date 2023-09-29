using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private LayerMask targetMask;
        public InventoryUI InventoryUI { get; private set; }

        private void Start()
        {
            InventoryUI = LevelUIManager.Instance.InventoryUI;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Utils.CheckLayer(targetMask, collision.gameObject.layer))
            {
                if (collision.TryGetComponent<ICollectible>(out ICollectible collectible))
                {
                    InventoryUI.TryAddInventoryItem(collectible);
                }
            }
        }
    }
}