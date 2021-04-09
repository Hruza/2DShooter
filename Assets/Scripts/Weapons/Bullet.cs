using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public int damage;

    private Rigidbody2D rb;

    public bool keepDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public enum onMapCollisionBehaviour { destroy, elasticBounce, nothing }
    public enum onPlayerCollisionBehaviour { destroy, elasticBounce, pierce }

    public onMapCollisionBehaviour mapCollisionBehaviour;
    public onPlayerCollisionBehaviour playerCollisionBehaviour;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Collided(other.gameObject);    
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        Collided(other.gameObject);
    }

    private void Update()
    {
        if (keepDirection)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void Collided(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("TakeDamage", new Damage(damage));
            switch (playerCollisionBehaviour)
            {
                case onPlayerCollisionBehaviour.destroy:
                    Destroy(this.gameObject);
                    break;
                case onPlayerCollisionBehaviour.elasticBounce:
                    BounceOfSurface(other);
                    break;
                case onPlayerCollisionBehaviour.pierce:
                    
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (mapCollisionBehaviour)
            {
                case onMapCollisionBehaviour.destroy:
                    Destroy(this.gameObject);
                    break;
                case onMapCollisionBehaviour.elasticBounce:
                    BounceOfSurface(other);
                    break;
                case onMapCollisionBehaviour.nothing:
                    
                    break;
                default:
                    break;
            }
        }
    }

    private void BounceOfSurface(GameObject other)
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position -(Time.deltaTime*rb.velocity),
                                             rb.velocity,1,LayerMask.GetMask(LayerMask.LayerToName(other.layer)));
        if (!hit)
            return;
        Vector2 proj = Vector2.Dot(hit.normal, rb.velocity) * hit.normal;
        rb.velocity = rb.velocity -(2*proj);
    }
}

public class Damage
{
    public int damage;
    public float knockback;

    public Damage(int damage)
    {
        this.damage = damage;
    }
}
