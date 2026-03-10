using UnityEngine;

namespace _Main.Scripts.WeaponSystem.Weapons
{
    /// Bu sınıf sadece silahın kendisine ait olan işleri halledecek ve bu sınıfın içinde;
    /// silahın ateş etme coroutine'i olacak
    /// silah prefab'inin üzerindeki script bu olacak
    /// TODO -> WeaponData bir sonraki aşamada buraya taşınacak ve weaponholder sadece envanter slotuna göre seçilen silahı initizlie edevcek
    
    public class PrimaryWeaponMono : AbstractPlayerWeaponBase
    {       
        [SerializeField] private WeaponData currentWeaponData;

        private ShootingData _shootingData = new ShootingData();
        
        private float _lastFireTime;

        public override bool isFiring { get; protected set; }

        public override void FireWeapon()
        {
            if (currentWeaponData == null || currentWeaponData.shootPoint == null) return;
            Debug.Log("asdasda");
            float fireInterval = 1f / currentWeaponData.fireRate;
            if (Time.time - _lastFireTime < fireInterval) return;
            _lastFireTime = Time.time;
            Debug.Log("damagee");

            _shootingData.ShootDamageRay(currentWeaponData.shootPoint.position, currentWeaponData.shootPoint.forward, currentWeaponData.range, currentWeaponData);
        }
    }
}