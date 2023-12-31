using PocketZone.Space;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class JunkPickup : MonoBehaviour, ICollectible
    {
        private const int maxStack = 1;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private int quantity = 30;
        [SerializeField] private int itemId = 3;
        public int ItemId => itemId;
        public int MaxStack => maxStack;
        public Sprite Sprite => spriteRenderer.sprite;
        public int Quantity { get => quantity; set => quantity = value; }
        public CollectibleType Type => CollectibleType.Junk;
        public void Collect()
        {
            InfoUI.Instance.SendInformation(InfoUI.JUNK_PICKED, MessageType.NOTE);
            Destroy(gameObject);
        }
    }
}