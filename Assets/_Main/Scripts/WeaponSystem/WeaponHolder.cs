using System;
using System.Collections;
using _Main.Input.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace _Main.Scripts.WeaponSystem
{
    //TODO : WEPAON RENDER VE LOGİC NASIL AYTIRILACAK ? BUNU ÇÖZÜP ONA GÖRE BURAYI İMPLEMENTE EDELİM.
    public class WeaponHolder : MonoBehaviour
    {
        private WeaponData currentWeaponData;
        [SerializeField] private AbstractPlayerWeaponBase equipped;

        // Bu sınıfta tuşa bastığımızda istediğimiz silahı elimizde spawnlayacağız
        /// 1 -> Rifle 2 -> Pistol
        /// Seçilen prefabi elimde spawnlayacak ,
        /// WeapnData'da id ve enum bakacak. Bu weapon datada silahın sınıfı ve adı yazacak
        /// Field'da atılan weapon datayı ve fielda atılan prefabı weapon holder'a initialize edeceğiz
        
        [Header("Weapon Holder Properties")] [SerializeField]
        private Transform holderParentTransform;

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        public void Init()
        {
            if (currentWeaponData == null)
            {
                Debug.LogWarning("Current Weapon Data is null !!!");
                return;
            }

            //Initializing weapon renderer
            // var weaponPrefab = currentWeaponData.weaponPrefab;
            // var weaponInstance = Instantiate(weaponPrefab, holderParentTransform.position, holderParentTransform.rotation);
            // weaponInstance.transform.SetParent(holderParentTransform);
        }

        private void Update()
        {
            if (BaseInputManager.Instance.Fire.IsActive) Shoot();
            if (BaseInputManager.Instance.Scope.IsActive) Scope();
        }

        private void Shoot()
        {
            equipped.FireWeapon();
            Debug.Log("SHOOTING");
        }

        private void Scope()
        {
            Debug.Log("Scope Açıldı");
        }

        private IEnumerator ScopeCoroutine()
        {
            while (true)
            {
                // Weapon aim settings will be handled from weapon data or custom weapon scriptable object.
                holderParentTransform.position = Vector3.Lerp(holderParentTransform.position, holderParentTransform.position + holderParentTransform.forward * 0.1f, Time.deltaTime * 5f);

            }

            // Scope açma animasyonu veya efektleri burada işlenebilir
            yield return new WaitForSeconds(0.5f); // Örnek olarak 0.5 saniye bekleyelim
            Debug.Log("Scope Coroutine tamamlandı");
        }
    }
}