using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : GenericEntity
{
    public int attackDamage;
    public GenericEntity targetEntity;
    public int expValue;
    public Sprite enemyVisuals;
    UnityEvent bossKilled;

    private void OnEnable()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = enemyVisuals;
        //if this is the boss enemy, sign up to game won event on death
        if(gameObject.tag=="Boss")
       { 
            if (bossKilled == null)
                bossKilled = new UnityEvent();

            bossKilled.AddListener(GameManager.Instance.GameWon); 
        }
    }
    public override void CalculateSteeringForces()
    {
        Vector3 ultimateForce = Vector3.zero;

        ultimateForce += Seek(targetEntity);

        //ultimateForce += Seperate(manager.enemies);

        //ultimateForce += StayInBounds();

        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);

        //Debug.Log("Ultimate Force for Enemy:" + ultimateForce);

        ApplyForce(ultimateForce);
    }
    public override void Die() {
        // Deactivate this entity
        gameObject.SetActive(false);
        GameManager.Instance.SpawnEXP(position, expValue);
        if (gameObject.tag == "Boss")
        {
            bossKilled.Invoke();
        }
    }

    protected override void Update()
    {   
        // Run base update functions
        base.Update();

        // Just fire the gun
        if (gun != null) {
            AimGun();
            FireGun();
        }
    }

    protected void AimGun()
    {
        // Aims gun at player
        // Compute direction vector
        Vector3 targetPosition = Platform.Instance.trueNearestPosition(position, Player.Instance.transform.position);
        Vector3 direction = targetPosition - position;

        // Aim gun with normalized direction vector
        gunBaseForRotation.transform.up = direction.normalized;
    }

}
