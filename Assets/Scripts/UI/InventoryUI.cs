using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PocketZone.Space
{
    public class InventoryUI : MonoBehaviour
    {
        private const int maxSlots = 9;
        [SerializeField] private GameObject inventoryPrefab;
        [SerializeField] private Transform inventoryPanel;
        [SerializeField] private Transform inventoryContainer;
        [SerializeField] private InputReader inputReader;
        private List<InventoryItemStack> inventoryStacks;

        private void Start()
        {
            inventoryStacks = new List<InventoryItemStack>();
            
            for (int i = 0; i < maxSlots; i++)
            {
                InventoryItemStack stack = new InventoryItemStack();
                stack.StackRenewed += OnStackRenewed;
                stack.StackDepleted += OnStackDepleted;
                inventoryStacks.Add(stack);
            }
            ClearAllItemsInInventoryUI();
            inputReader.InventoryOpenEvent += ToggleShowInventoryPanel;
        }

        private void OnDestroy()
        {
            inputReader.InventoryOpenEvent -= ToggleShowInventoryPanel;
            foreach (InventoryItemStack _stack in inventoryStacks)
            {
                _stack.ClearHandlers();
            }
        }

        private void OnStackRenewed(InventoryItemStack inventoryItemStack)
        {
            CreateNewItemsStackUI(inventoryItemStack);
        }

        private void OnStackDepleted(InventoryItemStack inventoryItemStack)
        {
            Destroy(inventoryItemStack.InventoryItem.gameObject);
        }

        private void ToggleShowInventoryPanel()
        {
            inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeSelf);
            if (inventoryPanel.gameObject.activeSelf)
            {
                LevelUIManager.Instance.StackUI.Push(inventoryPanel.gameObject);
            }
            else
            {
                if (LevelUIManager.Instance.StackUI.Peek() == inventoryPanel.gameObject)
                {
                    LevelUIManager.Instance.StackUI.Pop();
                }
            }
        }

        private void ClearAllItemsInInventoryUI()
        {
            InventoryItem[] _items = inventoryContainer.GetComponentsInChildren<InventoryItem>();
            foreach (var _item in _items)
            {
                Destroy(_item.gameObject);
            }
        }

        public void TryRemoveItemsStack(InventoryItem inventoryItem)
        {
            InventoryItemStack _stack = inventoryStacks.First(x => x.InventoryItem == inventoryItem);
            if (_stack != null)
            {
                _stack.DropStack();
            }
        }

        public void TryAddInventoryItem(ICollectible collectible)
        {
            bool _addResult = false;
            foreach (InventoryItemStack _stack in inventoryStacks)
            {
                _addResult = _stack.TryAddToStack(collectible);
                if (_addResult)
                {
                    collectible.Collect();
                    break;
                }
                else
                {
                    continue;
                }
            }
            if (!_addResult)
            {
                InfoUI.Instance.SendInformation(InfoUI.CANNOT_ADD_TO_INVENTORY, MessageType.WARNING);
            }
            RefreshItemsStack();
        }

        public bool TryConsumeInventoryItem(CollectibleType collectibleType, int consumeAmount)
        {
            bool _result = false;
            for (int i = inventoryStacks.Count - 1; i >= 0; i--)
            {
                _result = inventoryStacks[i].TryConsumeAmount(collectibleType, consumeAmount);
                if(_result)
                {
                    break;
                }
            }
            RefreshItemsStack();
            return _result;
        }

        private void RefreshItemsStack()
        {
            inventoryStacks = inventoryStacks.OrderByDescending(x => x.CurrentStack).ToList(); // sorting
            for (int i = 0; i < inventoryStacks.Where(x => x.CurrentStack > 0).ToList().Count; i++)
            {
                InventoryItem _inventoryItem = inventoryStacks[i].InventoryItem;
                _inventoryItem.transform.SetSiblingIndex(i);
                int _currentStack = inventoryStacks[i].CurrentStack;
                _inventoryItem.Text.text = (_currentStack > 1) ?_currentStack.ToString():"";

            }
            foreach (InventoryItemStack _stack in inventoryStacks.Where(x => x.CurrentStack <= 0 && x.InventoryItem != null))
            {
                _stack.DropStack();
            }
        }

        private void CreateNewItemsStackUI(InventoryItemStack stack)
        {
            GameObject _spawnedInventoryStack = Instantiate(inventoryPrefab, inventoryContainer);
            if (_spawnedInventoryStack.TryGetComponent<InventoryItem>(out InventoryItem _inventoryItem))
            {
                stack.InventoryItem = _inventoryItem;
                _inventoryItem.image.sprite = stack.Collectible.Sprite;
            }
        }
    }
}