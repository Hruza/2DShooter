using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public WeaponInfo weapon;

    private void Start() {
        GetComponent<SpriteRenderer>().sprite=weapon.sprite;    
    }
    
    public delegate void Notify();
    public event Notify NotifyOnInteraction;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            other.gameObject.SendMessage("GotWeapon",weapon);
        }
        NotifyOnInteraction?.Invoke();
        Destroy(this.gameObject);
    }
}
