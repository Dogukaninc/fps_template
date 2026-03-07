using UnityEngine;

namespace _Main.Scripts.WeaponSystem
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public abstract bool isFiring { get; protected set; }
        public abstract void FireWeapon();
    }
}