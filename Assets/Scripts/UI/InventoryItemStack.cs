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
            switch (type)
            {
                case CollectibleType.Ammo:
                    InfoUI.Instance.SendInformation(InfoUI.AMMO_DROPPED, MessageType.WARNING);
                    break;
                case CollectibleType.Health:
                    InfoUI.Instance.SendInformation(InfoUI.HEALTH_DROPPED, MessageType.WARNING);
                    break;
                case CollectibleType.Junk:
                    InfoUI.Instance.SendInformation(InfoUI.JUNK_DROPPED, MessageType.WARNING);
                    break;
                default:
                    break;
            }
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

        public bool TryConsumeAmount(CollectibleType collectibleType, int consumeAmount)
        {
            if (type == default(CollectibleType))
            {
                return false;
            }
            if (type == collectibleType)
            {
                currentStack -= consumeAmount;
                if (currentStack <= 0)
                {
                    DropStack();
                }
                switch (type)
                {
                    case CollectibleType.Health:
                        InfoUI.Instance.SendInformation(InfoUI.HEALTH_CONSUMED, MessageType.SUCCESS);
                        break;
                    default:
                        break;
                }
                return true;
            }
            else
            {
                return false;
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
