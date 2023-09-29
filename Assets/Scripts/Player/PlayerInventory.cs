using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private TriggerHandler triggerHandler;
        public InventoryUI InventoryUI { get; private set; }

        private void Start()
        {
            InventoryUI = LevelUIManager.Instance.InventoryUI;
            triggerHandler.OnTriggerEnter += HandleItemTriggerEnter;
        }

        private void OnDestroy()
        {
            triggerHandler.OnTriggerEnter -= HandleItemTriggerEnter;
        }

        private void HandleItemTriggerEnter(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent<ICollectible>(out ICollectible collectible))
            {
                InventoryUI.TryAddInventoryItem(collectible);
            }
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (Utils.CheckLayer(targetMask, collision.gameObject.layer))
        //    {
        //        if (collision.TryGetComponent<ICollectible>(out ICollectible collectible))
        //        {
        //            InventoryUI.TryAddInventoryItem(collectible);
        //        }
        //    }
        //}
    }
}