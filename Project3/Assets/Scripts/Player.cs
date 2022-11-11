using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GenericEntity
{
    public int xp;
    public float reloadSpeed;
    private float invincibilityTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        speed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        //Look at mouse
        LookAtMouse();
        Move();
        if (Input.GetMouseButtonDown(0))
        {
            gun.Shoot();
        }
    }

    private void LookAtMouse()
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = (Vector3)(mousePos - new Vector2(transform.position.x, transform.position.y));
    }
    private void Move()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _rigidBody.velocity = input.normalized * speed;

    }

    override public void OnCollisionEnter2D(Collision2D collision) {
        // Run parent-class handling
        base.OnCollisionEnter2D(collision);

        // Additional handling: enemy collision
        if (collision.gameObject.GetComponent<Enemy>()) {
            Damage(collision.gameObject.GetComponent<Enemy>().attackDamage);
        }
    }
}
