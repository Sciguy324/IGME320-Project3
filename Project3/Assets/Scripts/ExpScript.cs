using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpScript : MonoBehaviour
{
    public int value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if collision is with player if it is, collect the xp
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().GainEXP(value);
            GameManager.Instance.ReturnEXP(this.gameObject);
        }
    }
}
