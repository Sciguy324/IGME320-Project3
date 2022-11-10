using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GenericEntity
{
    public int xp;
    public float reloadSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
