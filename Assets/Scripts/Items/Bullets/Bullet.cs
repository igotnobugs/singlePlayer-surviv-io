using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour 
{
    public Unit fromWho;
    public float bulletDamage = 1.0f;
    public float bulletSpeed = 10.0f;
    public float bulletLifeSeconds = 10.0f;

    private Rigidbody2D rb;
    private Vector2 velocity;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public Bullet Fire(float damage, Transform fromWhere, Unit from) {
        Bullet newBullet = Instantiate(this);    
        newBullet.transform.position = fromWhere.position;
        newBullet.transform.rotation = fromWhere.rotation;
        newBullet.bulletDamage = damage;
        newBullet.fromWho = from;      
        return newBullet;
    }

    private void Update() {
        velocity = transform.up * bulletSpeed;
        Destroy(gameObject, bulletLifeSeconds);
    }

    private void FixedUpdate() {
        rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent(out IDamageable hit)) {
            hit.Damage((int)bulletDamage, fromWho);
            Destroy(gameObject);
        }
    }
}
