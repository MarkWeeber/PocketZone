using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PocketZone.Space
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class HealthPickup : MonoBehaviour, ICollectible
    {
        private const int maxStack = 5;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private int quantity = 30;
        private int itemId = 2;
        public int ItemId => itemId;
        public int MaxStack => maxStack;
        public Sprite Sprite => spriteRenderer.sprite;
        public int Quantity { get => quantity; set => quantity = value; }
        public CollectibleType Type => CollectibleType.Health;
        public void Collect()
        {
            InfoUI.Instance.SendInformation(InfoUI.HEALTH_PICKED, MessageType.NOTE);
            Destroy(gameObject);
        }
    }
}