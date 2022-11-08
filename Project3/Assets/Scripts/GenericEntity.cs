using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEntity : MonoBehaviour
{
    public float speed = 1.0f;
    public int health = 1;
    public Gun gun;
    public Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual public void OnCollisionEnter2D(Collision2D collision) {
        // Check if collided thing is a bullet
        if (collision.gameObject.GetComponent<Bullet>()) {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            // Apply damage
            Damage(bullet.damage);
        }
    }

    public bool Damage(int amount) {
        // Applies damage to the entity
        health -= amount;

        // Damage occurred
        return true;
    }

    public void SetGet(Gun newGun) {
        // Sets the entity's currently held gun
        gun = newGun;
        // FIXME: Where does the old gun go?
    }

    public void FireGun() {
        // Shoot the gun, using the current direction/position of this entity
        gun.Shoot(_rigidBody.rotation, _rigidBody.position);
    }
}
