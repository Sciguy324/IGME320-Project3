using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public int maxPierces = 1;
    private float speed;
    private Rigidbody2D _rigidbody;
    private int piercesRemaining = 0;
    public Gun sender;
    public Platform arena;
    private float liveTime = 0.0f;
    public string sourceTag;
    // Start is called before the first frame update
    void Start()
    {
        // Obtain component(s) for later use
        _rigidbody = GetComponent<Rigidbody2D>();
    }

   

    void returnToOrigin() {
        // Sends the bullet back to whence it came
        sender.ReturnBullet(this.gameObject);
        // Disable the bullet game object
        gameObject.SetActive(false);
        liveTime = 0.0f;
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
        // Wrap position around
        transform.position = arena.WrappedPosition(transform.position);

        // Increment timer
        liveTime += Time.fixedDeltaTime;

        // End if alive to long
        if(liveTime > 3.0f)
        {
            returnToOrigin();
        }
    }
}
