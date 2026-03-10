using UnityEngine;

namespace _Main.Scripts
{
    public class Health : MonoBehaviour // This class only hold health data and its functions
    {
        public float currentHealth;

        public void IncreaseHealth(float amount)
        {
            currentHealth += amount;
        }

        public void DecreaseHealth(float amount)
        {
            currentHealth -= amount;
            Debug.Log("Hedef Hasar Aldı");
        }
        
    }
}