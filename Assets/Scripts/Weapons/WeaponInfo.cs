using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Weapon", menuName = "Custom/Weapon", order = 1)]
public class WeaponInfo : ScriptableObject
{
    public Sprite sprite;
    public int damage=1;
    public int maxAmmo=1;
    public float shootDelay=0.5f; 
    public float projectileSpeed=1;
    public float spread=0;
    public GameObject bullet;
}
