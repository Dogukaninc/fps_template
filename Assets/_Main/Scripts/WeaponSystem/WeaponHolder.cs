using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.WeaponSystem
{
    //TODO : WEPAON RENDER VE LOGİC NASIL AYTIRILACAK ? BUNU ÇÖZÜP ONA GÖRE BURAYI İMPLEMENTE EDELİM.
    public class WeaponHolder : MonoBehaviour
    {
        private GameInput _input;
        [SerializeField] private WeaponData currentWeaponData;
        [SerializeField] private BaseWeapon equippedWeapon;

        
        // Bu sınıfta tuşa bastığımızda istediğimiz silahı elimizde spawnlayacağız
        /// 1 -> Rifle 2 -> Pistol
        /// Seçilen prefabi elimde spawnlayacak ,
        /// WeapnData'da id ve enum bakacak. Bu weapon datada silahın sınıfı ve adı yazacak
        /// Field'da atılan weapon datayı ve fielda atılan prefabı weapon holder'a initialize edeceğiz

        [Header("Weapon Holder Properties")] [SerializeField]
        private Transform holderParentTransform;

        private void Awake()
        {
            _input = new GameInput();

            _input.Player.Fire.performed += ctx => Shoot();
            _input.Player.AltFire.performed += ctx => Scope();
        }

        private void Start()
        {
            Init();
        }


        private void Init()
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
            if (_input.Player.Fire.IsPressed())
            {
                Debug.Log("MOuse basılıııııı");
                if (equippedWeapon.isFiring)
                {
                    Shoot();
                }
            }
        }

        private void Shoot()
        {
            equippedWeapon.FireWeapon();
        }

        private void Scope()
        {

        }
    }
}