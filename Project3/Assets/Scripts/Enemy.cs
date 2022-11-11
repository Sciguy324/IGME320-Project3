using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GenericEntity
{
    public int attackDamage;
    public GenericEntity targetEntity;
    public SpawnManager spawnManager;

    public Player player;

    public override void CalculateSteeringForces()
    {
        Vector3 ultimateForce = Vector3.zero;

        ultimateForce += Seek(player);

        //ultimateForce += Seperate(manager.enemies);

        //ultimateForce += StayInBounds();

        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);

        Debug.Log("Ultimate Force for Enemy:" + ultimateForce);

        ApplyForce(ultimateForce);
    }

}
