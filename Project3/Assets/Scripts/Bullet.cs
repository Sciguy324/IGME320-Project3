using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public int maxPierces = 1;
    private float speed;
    private Rigidbody2D _rigidbody;
    private int piercesRemaining = 0;
    // private [Type not yet implemented] sender;

    // Start is called before the first frame update
    void Start()
    {
        // Obtain component(s) for later use
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void shoot(float degree) {
        // Set velocity vector
        Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad*degree), Mathf.Sin(Mathf.Deg2Rad*degree));
        _rigidbody.velocity = speed * direction;

        // Set the number of enemies that can be pierced
        piercesRemaining = maxPierces;
    }

    void returnToOrigin() {
        // Sends the bullet back to whence it came

        // Destroy the bullet game object
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Check if other object is an entity
        // If it is, damage it and decrement the number of pierces
        // Note: Checking if we've already hit this enemy will not be necessary if the enemy has a damage cooldown.
        //       OR, we can have a list of enemies (or uuid's) that we've already hit
        // We also need to check if this bullet has pierced enough enemies, and send it back to its origin
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
