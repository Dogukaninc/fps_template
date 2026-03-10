using UnityEngine;

namespace _Main.Scripts.WeaponSystem.Scriptables
{
    [CreateAssetMenu(fileName = "so_weapon_settings_", menuName = "Scriptable Objects/Weapons/WeaponCustomSettingsSO", order = 0)]
    public class WeaponCustomSettings : ScriptableObject
    {
        //Bu sınıf ile silahların prefableri , ateşleme konfigürasyonları gibi özellikleri tutup asset olarak WeaponRenderView'larına atayacağız.
		// Animasyonları, tepme durumları gibi özellikler de burada tutulabilir.
    }
}