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
    public Gun sender;

    // Start is called before the first frame update
    void Start()
    {
        // Obtain component(s) for later use
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Shoot(float degree, Vector2 origin) {
        // Activate bullet
        gameObject.SetActive(true);

        // Set position
        _rigidbody.position = origin;

        // Set velocity vector
        Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad*degree), Mathf.Sin(Mathf.Deg2Rad*degree));
        _rigidbody.velocity = speed * direction;

        // Set the number of enemies that can be pierced
        piercesRemaining = maxPierces;
    }

    void returnToOrigin() {
        // Sends the bullet back to whence it came

        // Disable the bullet game object
        gameObject.SetActive(false);
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Check if other object is an entity
        // If it is, decrement the number of pierces
        if (collision.gameObject.GetComponent<GenericEntity>()) {
            // Decrement the number of pierces remaining.  Return to sender if applicable
            piercesRemaining--;
            if (piercesRemaining <= 0) {
                returnToOrigin();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
