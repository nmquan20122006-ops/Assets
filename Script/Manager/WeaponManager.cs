using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour, ICollectible
{
    [SerializeField] private Weapon[] weapons;
    private Weapon currentWeapont;
    public Weapon CurrentWeapon => currentWeapont;
    void Start()
    {
        EquipWeapon(0);
    }
        
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EquipWeapon(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            EquipWeapon(1);
    }
    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;
        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == index);
        }
        currentWeapont = weapons[index];
    }
    public bool Collect(PlayerWeaponData data)
    {
        if (data == null) return false;
        currentWeapont.SetWeaponData(data);
        return true;
    }
}
