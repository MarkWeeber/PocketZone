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
        private Stack<GameObject> stackUI;
        [SerializeField] private InputReader inputReader;
        public Stack<GameObject> StackUI { get => stackUI; set => stackUI = value; }

        private void Awake()
        {
            instance = this;
            StackUI = new Stack<GameObject>();
            inputReader.EscapeEvent += HandleEscape;
        }

        private void OnDestroy()
        {
            inputReader.EscapeEvent -= HandleEscape;
        }

        private void HandleEscape()
        {
            if (stackUI.Count > 0)
            {
                GameObject UIElement = stackUI.Peek();
                if (UIElement != null)
                {
                    UIElement.gameObject.SetActive(false);
                    stackUI.Pop();
                }
            }
        }
    }
}