using UnityEngine;

namespace _Main.Scripts.WeaponSystem
{
    public class ShootingData
    {
        // bu sınıfta ateş logicleri olacak 
        /// Burst fire, shotgun, throw grenade, single/auto fire. etc.
       
        
        //TODO bildiğimiz anlamda projectile değil aslında raycast ama biz mermiyi ifade etmek içim projectile diyoruz.
        //TODO İleride gerçek bir projectile sistemi eklemek istediğimizde bu metodu değiştirebiliriz.
        public void DefaultProjectileShooting(Vector3 origin, Vector3 direction, float range, WeaponData weaponData)
        {
            var damage = weaponData.damage;
            RaycastHit hit;
            if (SendRaycast(origin, direction, range, out hit))
            {
                var damageStatController = new DamageStatController();
                damageStatController.ApplyRegularDamage(hit.transform.gameObject, damage);
                Debug.Log("Hedefe hasar verdim");
            }
        }
        
        
        
        

        // Çarpışma bilgisini döndüren yöntem
        public bool SendRaycast(Vector3 origin, Vector3 direction, float range, out RaycastHit hit)
        {
            Ray ray = new Ray(origin, direction.normalized);
            return Physics.Raycast(ray, out hit, range);
        }

        // Sadece isabet kontrolü isteyen overload
        public bool SendRaycast(Vector3 origin, Vector3 direction, float range)
        {
            Ray ray = new Ray(origin, direction.normalized);
            return Physics.Raycast(ray, range);
        }
    }
}