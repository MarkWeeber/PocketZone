using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Weapon weapon;

        private bool firing;

        private void Start()
        {
            if (weapon == null)
            {
                return;
            }
            inputReader.FireEvent += HandleWeaponFire;
        }

        private void OnDestroy()
        {
            if (weapon == null)
            {
                return;
            }
            inputReader.FireEvent -= HandleWeaponFire;
        }

        private void Update()
        {
            if (firing)
            {
                FireWeapon();
            }
        }

        private void FireWeapon()
        {
            weapon.Fire();
        }

        private void HandleWeaponFire(bool pressing)
        {
            firing = pressing;
        }
    }
}