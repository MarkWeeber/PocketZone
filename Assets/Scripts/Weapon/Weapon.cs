using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class Weapon : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform firingPort;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Collider2D playerCollider;
        [Header("Settings")]
        [SerializeField] private int bulletSpawnAmount = 50;
        [SerializeField] private float bulletSpeed = 100f;
        [SerializeField] private float bulletDamage = 20f;
        [SerializeField] private float fireRatePerMinute = 600;
        [SerializeField] private CollectibleType consumeType = CollectibleType.Ammo;

        private float fireTimer = -1f;
        private List<BulletDamageEffector> bullets;
        private InventoryUI inventoryUI;
        private int currentBulletIndex;
        private int bulletCount;
        private bool validToShoot;

        private void Awake()
        {
            bullets = new List<BulletDamageEffector>();
            for (int i = 0; i < bulletSpawnAmount; i++)
            {
                GameObject _go = Instantiate(bulletPrefab, null);
                _go.transform.position = Vector2.zero;
                if (_go.TryGetComponent<BulletDamageEffector>(out BulletDamageEffector _bullet))
                {
                    _bullet.ReturnBullet();
                    _bullet.DamageValue = bulletDamage;
                    bullets.Add(_bullet);
                }
            }
            bulletCount = bullets.Count;
            currentBulletIndex = 0;
        }

        private void Start()
        {
            inventoryUI = LevelUIManager.Instance.InventoryUI;
        }

        private void Update()
        {
            HandleTimer();
        }

        private void HandleTimer()
        {
            if (fireTimer > 0f)
            {
                fireTimer -= Time.deltaTime;
            }
        }

        public void Fire()
        {
            if (fireTimer < 0f)
            {
                fireTimer = 60f / fireRatePerMinute;
                if (bulletCount > 0)
                {
                    validToShoot = true;
                    if (inventoryUI != null)
                    {
                        if (!inventoryUI.TryConsumeInventoryItem(consumeType, 1))
                        {
                            InfoUI.Instance.SendInformation(InfoUI.NO_AMMO_IN_INVENTORY, MessageType.WARNING);
                            validToShoot = false;
                        }
                    }
                    if(validToShoot)
                    {
                        bullets[currentBulletIndex].SendBullet(firingPort.position, firingPort.right, bulletSpeed, playerCollider);
                        currentBulletIndex++;
                        if (currentBulletIndex >= bulletCount - 1)
                        {
                            currentBulletIndex = 0;
                        }
                    }
                }
            }
        }
    }
}