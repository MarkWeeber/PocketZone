using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private LayerMask targetMask;
        private InventoryUI inventoryUI;

        private void Start()
        {
            inventoryUI = LevelUIManager.Instance.InventoryUI;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Utils.CheckLayer(targetMask, collision.gameObject.layer))
            {
                if (collision.TryGetComponent<ICollectible>(out ICollectible collectible))
                {
                    inventoryUI.TryAddInventoryItem(collectible);
                }
            }
        }
    }
}