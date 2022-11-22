using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : GenericEntity
{
    public int xp;
    public int nextLevelEXP =3;
    public float reloadSpeed;
    private float invincibilityTime = 1.0f;

    private Animator anim;

    private bool tempInves = false;
    //GUI
    public Gui playerGUI;
    //singlton code
    private static Player instance;

    public Transform gunBaseForRotation;
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
        health = maxHealth;
        position = transform.position;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        arena = GameObject.Find("Platform").GetComponent<Platform>();
        anim = gameObject.GetComponent<Animator>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        playerGUI.expText.text = "0 / " + nextLevelEXP.ToString();
    }

    void WrapAround()
    {
        transform.position = arena.WrappedPosition(transform.position);
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
        mousePos = arena.trueNearestPosition(transform.position, mousePos);
        gunBaseForRotation.up = (Vector3)(mousePos - new Vector2(transform.position.x, transform.position.y));
    }
    private void Move()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Debug.Log(input);
        if (input.x > 0)
        {
            anim.SetBool("isRight", true);
            anim.SetBool("isUp", false);
            anim.SetBool("isDown", false);
            anim.SetBool("isLeft", false);
        }
        if (input.x < 0)
        {
            anim.SetBool("isRight", false);
            anim.SetBool("isUp", false);
            anim.SetBool("isDown", false);
            anim.SetBool("isLeft", true);
        }
        if (input.y > 0)
        {
            anim.SetBool("isRight", false);
            anim.SetBool("isUp", true);
            anim.SetBool("isDown", false);
            anim.SetBool("isLeft", false);
        }
        if (input.y < 0)
        {
            anim.SetBool("isRight", false);
            anim.SetBool("isUp", false);
            anim.SetBool("isDown", true);
            anim.SetBool("isLeft", false);
        }
        _rigidBody.velocity = input.normalized * speed;

    }

    override public void OnCollisionEnter2D(Collision2D collision) {
        // Run parent-class handling
        base.OnCollisionEnter2D(collision);

        // Additional handling: enemy collision
        if (collision.gameObject.GetComponent<Enemy>() && !tempInves) {
            Debug.Log("Ouch!");
            Damage(collision.gameObject.GetComponent<Enemy>().attackDamage);
            StartCoroutine(tempshield());
        }
    }
    IEnumerator tempshield()
    {
        tempInves = true;
        yield return new WaitForSeconds(invincibilityTime);
        tempInves = false;

    }

    public override void Die()
    {
        Debug.Log("Oh no, I died!");
        SceneManager.LoadScene("Lose Scene");
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
            xp = 0;
        }
        playerGUI.SetEXP(xp, nextLevelEXP);
    }
    public override bool Damage(int amount)
    {
        if(health-1 >= 0)
        playerGUI.SetHelathUI(health-1);
        return base.Damage(amount);
    }

}
