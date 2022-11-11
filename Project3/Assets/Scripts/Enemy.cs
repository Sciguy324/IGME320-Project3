using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GenericEntity
{
    public int attackDamage;
    public GenericEntity targetEntity;

    public override void CalculateSteeringForces()
    {
        Vector3 ultimateForce = Vector3.zero;

        ultimateForce += Seek(targetEntity);

        //ultimateForce += Seperate(manager.enemies);

        //ultimateForce += StayInBounds();

        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);

        Debug.Log("Ultimate Force for Enemy:" + ultimateForce);

        ApplyForce(ultimateForce);
    }

    public override void Die() {
        // Deactivate this entity
        gameObject.SetActive(false);
    }

}
