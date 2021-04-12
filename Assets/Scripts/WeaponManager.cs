using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public WeaponInfo defaultWeapon;
    public SpriteRenderer weaponSpritePivot;
    private List<weaponInInventory> weaponInventory;
    private int currentWeaponIndex;

    struct weaponInInventory
    {
        public int ammo;
        public WeaponInfo weapon;
        public weaponInInventory(int ammo, WeaponInfo weapon)
        {
            this.ammo = ammo;
            this.weapon = weapon;
        }
    }

    void Start()
    {
        weaponSpritePivot.sprite = defaultWeapon.sprite;
    }

    void Update()
    {
        
    }

    public void switchWeaponUp()
    {
        currentWeaponIndex++;
        if(currentWeaponIndex == weaponInventory.Count){
            currentWeaponIndex = 0;
        }

        changeWeaponVisual(currentWeaponIndex);
    }

    public void switchWeaponDown()
    {
        currentWeaponIndex--;
        if(currentWeaponIndex < 0){
            currentWeaponIndex = weaponInventory.Count - 1;
        }

        changeWeaponVisual(currentWeaponIndex);
    }

    public void GotWeapon(WeaponInfo acquiredWeapon)
    {
        weaponInventory.Add(new weaponInInventory(acquiredWeapon.maxAmmo, acquiredWeapon));
    }

    public void changeWeaponVisual(int index)
    {
        weaponSpritePivot.sprite = weaponInventory[index].weapon.sprite;
    }
}