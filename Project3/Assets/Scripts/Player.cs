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

        position = transform.position;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        // TODO: UPDATE IN TERMS OF THE ACTUAL ARENA OBJECT IN THE UNITY SCENE
        // arena = GameObject.Find("Plane");
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        minX = (arena.transform.position.x - (arena.transform.localScale.x / 2)) * 10;
        maxX = (arena.transform.position.x + (arena.transform.localScale.x / 2)) * 10;
        minY = (arena.transform.position.y - (arena.transform.localScale.y / 2)) * 10;
        maxY = (arena.transform.position.y + (arena.transform.localScale.y / 2)) * 10;
        //randomAngle = Random.Range(0, 360);
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Look at mouse
        LookAtMouse();
        Move();

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
            Debug.Log("Ouch!");
            Damage(collision.gameObject.GetComponent<Enemy>().attackDamage);
        }
    }

    public override void Die()
    {
        Debug.Log("Oh no, I died!");
    }

    public override void CalculateSteeringForces()
    {

    }
}
