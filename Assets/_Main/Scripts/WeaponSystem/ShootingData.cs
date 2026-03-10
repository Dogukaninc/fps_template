using UnityEngine;

namespace _Main.Scripts.WeaponSystem
{
    public class ShootingData
    {
        // bu sınıfta ateş logicleri olacak 
        /// Burst fire, shotgun, throw grenade, single/auto fire. etc.
        
        public void ShootDamageRay(Vector3 origin, Vector3 direction, float range, WeaponData weaponData)
        {
            var damage = weaponData.damage;
            RaycastHit hit;
            if (SendRaycast(origin, direction, range, out hit))
            {
                var damageStatController = new DamageStatController();
                damageStatController.ApplyDamage(hit.transform.gameObject, damage);
            }
        }
        

        private bool SendRaycast(Vector3 origin, Vector3 direction, float range, out RaycastHit hit)
        {
            Ray ray = new Ray(origin, direction.normalized);
            return Physics.Raycast(ray, out hit, range);
        }
    }
}