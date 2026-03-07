using System.Collections;
using UnityEngine;

namespace _Main.Scripts
{
    /// <summary>
    ///  fire damage, explosion damage,bleeding damage,  etc. gibi farklı hasar türlerini yönetmek için bu sınıfı kullanabiliriz.
    /// </summary>
    public class DamageStatController
    {
        public void ApplyRegularDamage(GameObject target, float damage)
        {
            Damage(target, damage);
        }
        
        private IEnumerator ApplyBleedingDamageCoroutine(GameObject target, float damage, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                Damage(target, damage);
                elapsed += 1f; // Apply damage every second
                yield return new WaitForSeconds(1f);
            }
        }
        
        private void Damage(GameObject target, float damage)
        {
            var health = target.GetComponent<Health>();
            if (health != null)
            {
                health.DecreaseHealth(damage);
            }   
        }
        
        
    }
}