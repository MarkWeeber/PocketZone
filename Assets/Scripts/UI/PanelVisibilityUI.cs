using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PocketZone.Space
{
    public class PanelVisibilityUI : MonoBehaviour, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private bool pointerOver;

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
            LevelUIManager.Instance.StackUI.Push(gameObject);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (!pointerOver)
            {
                gameObject.SetActive(false);
                if (LevelUIManager.Instance.StackUI.Peek() == gameObject)
                {
                    LevelUIManager.Instance.StackUI.Pop();
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            pointerOver = true;
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointerOver = false;
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}