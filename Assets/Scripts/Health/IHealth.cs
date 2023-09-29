using System;

namespace PocketZone.Space
{
    public interface IHealth
    {
        public bool IsAlive { get; }
        public float CurrentHealth { get; }
        public float MaxHealth { get; }
        public void TakeDamage(float damage);
        public void Heal(float healAmount);
        public event Action<float> HealthBarUpdate;
    }
}