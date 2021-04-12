using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public WeaponInfo defaultWeapon;
    public SpriteRenderer weaponSpritePivot;
    public GameObject weaponSpritePivotGO;

    private SpriteRenderer pivot;
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
        weaponInventory = new List<weaponInInventory>{new weaponInInventory(defaultWeapon.maxAmmo,defaultWeapon)};
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

    public void changeWeaponDirection(bool right){
        if(right){
            weaponSpritePivot.flipX = false;
            weaponSpritePivotGO.transform.localPosition=new Vector3(transform.localPosition.x,
                                                            transform.localPosition.y,
                                                            transform.localPosition.z);
        }
        else{
            weaponSpritePivot.flipX = true;
            weaponSpritePivotGO.transform.localPosition=new Vector3(-transform.localPosition.x,
                                                             transform.localPosition.y,
                                                             transform.localPosition.z);
        }
    }
}