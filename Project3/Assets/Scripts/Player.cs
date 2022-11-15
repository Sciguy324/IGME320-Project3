using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GenericEntity
{
    public int xp;
    public int nextLevelEXP =3;
    public float reloadSpeed;
    private float invincibilityTime = 0.0f;
    
    //singlton code
    private static Player instance;
    public static Player Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = 10;

        position = transform.position;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        // TODO: UPDATE IN TERMS OF THE ACTUAL ARENA OBJECT IN THE UNITY SCENE
        // arena = GameObject.Find("Plane");
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        minX = (arena.transform.position.x - (arena.transform.localScale.x / 2)) * 1.0f;
        maxX = (arena.transform.position.x + (arena.transform.localScale.x / 2)) * 1.0f;
        minY = (arena.transform.position.y - (arena.transform.localScale.y / 2)) * 1.0f;
        maxY = (arena.transform.position.y + (arena.transform.localScale.y / 2)) * 1.0f;
        //randomAngle = Random.Range(0, 360);
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    void WrapAround()
    {
        Vector2 offset = new Vector2(0.0f, 0.0f);

        // Wrap x-position around
        if (transform.position.x > maxX)
        {
            offset.x -= maxX - minX;
        } else if (transform.position.x < minX)
        {
            offset.x += maxX - minX;
        }

        // Wrap y-position around
        if (transform.position.y > maxY)
        {
            offset.y -= maxY - minY;
        } else if (position.y < minY)
        {
            offset.y += maxY - minY;
        }

        // Apply offset
        transform.position = transform.position + (Vector3)offset;
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

        // Wrap-around position
        position = transform.position;
        WrapAround();
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

    public void GainEXP(int value)
    {
        xp += value;
        if (xp >= nextLevelEXP)
        {
            //Level Up
            GameManager.Instance.LevelUp();
            nextLevelEXP = nextLevelEXP * 2;

        }
    }
}
