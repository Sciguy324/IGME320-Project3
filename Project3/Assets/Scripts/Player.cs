using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : GenericEntity
{
    public int xp;
    public int nextLevelEXP = 3;
    public float reloadSpeed;
    private float invincibilityTime = 1.0f;
    public float accelTimeConstant = 0.05f;

    private Animator anim;

    private bool tempInves = false;
    //GUI
    public Gui playerGUI;
    public GameObject hatObject;
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
        health = maxHealth;
        position = transform.position;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        playerGUI.expText.text = "0 / " + nextLevelEXP.ToString();
        if (wearingHat)
            unlockHat();
    }

    void WrapAround()
    {
        transform.position = Platform.Instance.WrappedPosition(transform.position);
    }

    // Update is called once per frame
    protected override void Update()
    {
        //Look at mouse
        LookAtMouse();
        Move();
        if (Input.GetMouseButtonDown(0))
        {
            FireGun();
        }

        // Wrap-around position
        position = transform.position;
        WrapAround();
    }

    private void LookAtMouse()
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = Platform.Instance.trueNearestPosition(transform.position, mousePos);
        gunBaseForRotation.up = (Vector3)(mousePos - new Vector2(transform.position.x, transform.position.y));
    }
    private void Move()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //Debug.Log(input);
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
        // Smoothly change velocity over to target velocity assuming an exponentially decaying approach
        Vector2 targetVelocity = input.normalized * speed;
        
        Vector2 diff = targetVelocity - _rigidBody.velocity;
        _rigidBody.velocity = _rigidBody.velocity + diff * (1-Mathf.Exp(-Time.deltaTime/accelTimeConstant));
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
    //Used for enemy bullets

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet" && !tempInves)
        {
            Destroy(collision.gameObject);
            Debug.Log("Ouch!");
            Damage(1);
            StartCoroutine(tempshield());
        }

    }

    public void unlockHat()
    {
        wearingHat = true;
        hatObject.SetActive(true);
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
        
        // Kill all boss bullets
        GameObject bulletPool = GameObject.Find("UbhObjectPool");
        bulletPool.GetComponent<UbhObjectPool>().ReleaseAllBullet(true);

        // Load the lose screen
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
            nextLevelEXP = Mathf.RoundToInt(nextLevelEXP * 1.5f);
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

    public void addHeart()
    {
        // Adds an extra heart to the player's health
        if (health == maxHealth)
        {
            maxHealth++;
        }   
        health++;
        playerGUI.SetHelathUI(health);
    }

}
