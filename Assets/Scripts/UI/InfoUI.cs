using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PocketZone.Space
{
    public class InfoUI : MonoBehaviour
    {
        public const string LOAD_SUCCESS_NEW = "Сохранения не обнаружены, создан новый профиль прогресса";
        public const string LOAD_SUCCESS_PERSISTS = "Сохранения загружены";
        public const string CANNOT_ADD_TO_INVENTORY = "Инвентарь заполнен";
        public const string AMMO_PICKED = "Патроны подобраны";
        public const string AMMO_DROPPED = "Патроны выброшены";
        public const string HEALTH_PICKED = "Аптечка подобрана";
        public const string HEALTH_DROPPED = "Аптечка выброшена";
        public const string HEALTH_CONSUMED = "Аптечка использована";
        public const string JUNK_PICKED = "Мусор подобран";
        public const string JUNK_DROPPED = "Мусор выброшен";
        public const string NO_AMMO_IN_INVENTORY = "Недостаточно патронов в инвентаре";
        public const string NO_HEALTH_IN_INVENTORY = "Недостаточно аптечек в инвентаре";
        public const string ENEMIES_APPROACHING = "Враги приближаются";
        public const string PLAYER_DIED = "Вы погибли";
        public const string LEVEL_COMPLETE = "Уровень зачищен!";

        [Header("References")]
        [SerializeField] private Transform informationContainer;
        [Header("Settings")]
        [SerializeField] private bool showConsoleMessages = false;
        [SerializeField] private float fadeOutTime = 3f;
        [SerializeField] private Color noteColor;
        [SerializeField] private Color warningColor;
        [SerializeField] private Color errorColor;
        [SerializeField] private Color successColor;

        private static InfoUI instance;
        private List<TMP_Text> textList;
        private int index = -1;
        private int count = 0;
        public static InfoUI Instance => instance;

        private void Awake()
        {
            instance = this;
            textList = informationContainer.GetComponentsInChildren<TMP_Text>().ToList();
            count = textList.Count;
            if (showConsoleMessages)
            {
                Application.logMessageReceived += HandleConsoleMessages;
            }
        }

        private void OnDestroy()
        {
            if (showConsoleMessages)
            {
                Application.logMessageReceived -= HandleConsoleMessages;
            }
        }

        private void Update()
        {
            HandleInformationMessageShow();
        }

        private void HandleInformationMessageShow()
        {
            TMP_Text[] sortedArray = textList.OrderByDescending(t => t.color.a).ToArray(); // sorting
            for (int i = 0; i < sortedArray.Length; i++)
            {
                sortedArray[i].transform.SetSiblingIndex(i);
            }
            foreach (TMP_Text _text in textList)
            {
                float alpha = _text.color.a;
                float timer = alpha * fadeOutTime;
                if (alpha > 0.005f)
                {
                    timer -= Time.deltaTime;
                    _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, Mathf.Lerp(0f, 1f, Mathf.Abs(timer / fadeOutTime)));
                }
            }
        }

        public void SendInformation(string message, MessageType infoMessageType = MessageType.NOTE)
        {
            index++;
            if (index >= count)
            {
                index = 0;
            }
            textList[index].text = message;
            switch (infoMessageType)
            {
                case MessageType.NOTE:
                    textList[index].color = noteColor;
                    break;
                case MessageType.WARNING:
                    textList[index].color = warningColor;
                    break;
                case MessageType.ERROR:
                    textList[index].color = errorColor;
                    break;
                case MessageType.SUCCESS:
                    textList[index].color = successColor;
                    break;
                default:
                    break;
            }
        }

        private void HandleConsoleMessages(string condition, string stackTrace, LogType type)
        {
            SendInformation(condition + " + " + stackTrace, MessageType.NOTE);
        }
    }

    public enum MessageType
    {
        NOTE = 1,
        WARNING = 2,
        ERROR = 3,
        SUCCESS = 4,
    }
}