using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;

        private void Start()
        {
            playerHealth.OnDeath += HandlePlayerDead;
        }

        private void OnDestroy()
        {
            playerHealth.OnDeath -= HandlePlayerDead;
        }

        private void HandlePlayerDead()
        {
            InfoUI.Instance.SendInformation(InfoUI.PLAYER_DIED, MessageType.WARNING);
            GameManager.Instance.SetGameState(GameState.PlayerDeadInGame);
        }
    }
}