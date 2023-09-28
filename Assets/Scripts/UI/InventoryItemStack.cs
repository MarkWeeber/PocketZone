using System;
using UnityEngine;

namespace PocketZone.Space
{
    [System.Serializable]
    public class InventoryItemStack
    {
        private ICollectible collectible;
        public ICollectible Collectible => collectible;
        private int currentStack;
        public int CurrentStack => currentStack;
        public InventoryItem InventoryItem { get; set; }
        public event Action<InventoryItemStack> StackRenewed;
        public event Action<InventoryItemStack> StackDepleted;
        private CollectibleType type;
        private int maxStack;

        public InventoryItemStack()
        {
            type = default(CollectibleType);
        }

        public void DropStack()
        {
            StackDepleted?.Invoke(this);
            InventoryItem = null;
            type = default(CollectibleType);
            currentStack = 0;
            maxStack = 0;
        }

        public bool TryAddToStack(ICollectible collectible)
        {
            if (type == default(CollectibleType))
            {
                type = collectible.Type;
                maxStack = collectible.MaxStack;
                currentStack = 0;
                this.collectible = collectible;
                StackRenewed?.Invoke(this);
            }
            if (collectible.Type != type)
            {
                return false;
            }
            else
            {
                int sum = currentStack + collectible.Quantity;
                int overstack = sum - maxStack;
                if (sum > maxStack)
                {
                    currentStack = maxStack;
                    collectible.Quantity = overstack;
                    return false;
                }
                else
                {
                    currentStack = sum;
                    collectible.Quantity = 0;
                    return true;
                }
            }
        }

        public void ClearHandlers()
        {
            foreach (Action<InventoryItemStack> _action in StackRenewed.GetInvocationList())
            {
                StackRenewed -= _action;
            }
            
            foreach (Action<InventoryItemStack> _action in StackDepleted.GetInvocationList())
            {
                StackDepleted -= _action;
            }
        }
    }
}
