using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public interface ICollectible
    {
        public int ItemId { get; }
        public int MaxStack { get; }
        public int Quantity { get; set; }
        public Sprite Sprite { get; }
        public CollectibleType Type { get; }
        public void Collect();
    }

    public enum CollectibleType
    {
        Default = 0,
        Ammo = 1,
        Health = 2,
        Junk = 3,
    }
}