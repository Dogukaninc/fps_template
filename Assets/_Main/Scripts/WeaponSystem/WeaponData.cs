using System;
using UnityEngine;

namespace _Main.Scripts.WeaponSystem
{
    [Serializable]
    public class WeaponData
    {
        public GameObject weaponPrefab;
        public WeaponType weaponType;
        public Transform shootPoint;
        public float damage;
        public float fireRate;
        public float range;
        public int clipSize;
        public int maxAmmo;
    }
}