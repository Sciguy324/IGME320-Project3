using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEntity : MonoBehaviour
{
    public float speed = 1.0f; // How fast the entity moves
    public int health = 1; // The amount of health that the entity has, is a number instead of like the player who has hearts
    protected Vector3 position; // Position of the entity
    protected Vector3 direction; // The direction the entity is facing
    protected Vector3 velocity; // The velocity of the entity
    protected Vector3 acceleration; // The acceleration of the entity
    protected GameManager manager; // The manager for the entire game, a reference to it
    protected GameObject arena;

    private float radius; // Radius of entity

    public MeshRenderer mesh;

    public float Radius => radius; // Accessor for the radius variable

    // The bounds of the level/screen
    protected float minX;
    protected float maxX;
    protected float minZ;
    protected float maxZ;

    private Vector3 forward = Vector3.forward; // Vector3 that represents forward direction
    private Vector3 right = Vector3.right; // Vector3 that represents right direction

    [SerializeField]
    [Min(0.001f)]
    [Tooltip("The mass of the entity")]
    protected float mass = 1f; // Mass of entity

    [SerializeField]
    [Tooltip("The radius around the entity where they should actually seperate from other vehicles")]
    private float personalSpace = 1f;

    public float maxSpeed = 2f; // The max speed of the entity
    public float maxForce = 2f; // The max movement force of the entity

    public float Mass // Getter, setter for the mass
    {
        get { return mass; }
        set { mass = value; }
    }

    public Vector3 Position // Getter, setter for the position
    {
        get { return position; }
        set { position = value; }
    }

    public Gun gun;
    public Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        // TODO: UPDATE IN TERMS OF THE ACTUAL ARENA OBJECT IN THE UNITY SCENE
        // arena = GameObject.Find("Plane");
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        minX = (arena.transform.position.x - (arena.transform.localScale.x / 2)) * 10;
        maxX = (arena.transform.position.x + (arena.transform.localScale.x / 2)) * 10;
        minZ = (arena.transform.position.z - (arena.transform.localScale.z / 2)) * 10;
        maxZ = (arena.transform.position.z + (arena.transform.localScale.z / 2)) * 10;
        radius = mesh.bounds.extents.x;
        //randomAngle = Random.Range(0, 360);
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //CalculateSteeringForces();

        // This is needed for all entities

        UpdatePosition();
        SetTransform();
    }

    /// <summary>
    /// Applies a given force to affect the vehicle's acceleration
    /// </summary>
    /// <param name="force">Force to be applied and act on this vehicle</param>
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    /// <summary>
    /// Updates the position of the vehicle using force-based movement
    /// </summary>
    private void UpdatePosition()
    {
        // Step 1: add acceleration to velocity * time
        velocity += acceleration * Time.deltaTime;

        // Don't worry about the y axis, we're just on a plane on the ground
        velocity.y = 0;

        // Step 2: add our velocity to our position
        position += velocity * Time.deltaTime;

        // Don't worry about the y axis, we're just on a plane on the ground
        position.y = 0;

        // Step 3: reset our acceleration and set our direction vector
        if (velocity != Vector3.zero)
        {
            direction = velocity.normalized;

            forward = direction;
            right = Vector3.Cross(forward, Vector3.up);
        }
        acceleration = Vector3.zero;
    }

    /// <summary>
    /// Sets the vehicle's transform to the position set in UpdatePosition()
    /// </summary>
    private void SetTransform()
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    protected Vector3 StayInBounds()
    {
        //arena.GetComponent<SpriteRenderer>()
        Vector3 futurePos = new Vector3(1.0f, 1.0f, 1.0f); //GetFuturePosition(1);
        //Vector3 posToFlee = boundaries.ClosestPoint(position);
        //float distBetween = Vector3.Distance(position, posToFlee);
        //distBetween = Mathf.Max(distBetween, 0.001f);
        if (position.x > maxX || position.x < minX ||
            position.z > maxZ || position.z < minZ)
        {
            //return Seek(posToFlee) * 2000;
        }
        else if (futurePos.x >= maxX || futurePos.x <= minX ||
            futurePos.z >= maxZ || futurePos.z <= minZ)
        {
            //return Flee(posToFlee) * (1 / distBetween);
        }


        return Vector3.zero;
    }

    /// <summary>
    /// Makes sure each entity is seperated, specifically the enemies
    /// </summary>
    /// <typeparam name="T">List of entities</typeparam>
    /// <param name="vehicles">Entities</param>
    /// <returns>Seperation force</returns>
    protected Vector3 Seperate<T>(List<T> entities) where T : GenericEntity
    {
        Vector3 seperationForce = Vector3.zero;

        foreach (GenericEntity other in entities)
        {
            float sqrDistance = GetSquaredDistanceBetween(other);

            if (sqrDistance < Mathf.Epsilon)
            {
                continue;
            }

            if (sqrDistance < 0.001)
            {
                sqrDistance = 0.001f;
            }

            float personalSpaceRadius = personalSpace * personalSpace;

            if (sqrDistance < personalSpaceRadius)
            {
                seperationForce += Flee(other) * (personalSpaceRadius / sqrDistance);
            }
        }

        return seperationForce;
    }

    protected float GetSquaredDistanceBetween(GenericEntity entity)
    {
        // Entity1 center, x coord
        float entity1CenterX = gameObject.GetComponent<BoxCollider>().bounds.center.x;
        // Vehicle center, y coord
        float entity1CenterY = gameObject.GetComponent<BoxCollider>().bounds.center.y;
        // Entity center, x coord
        float entity2CenterX = entity.GetComponent<BoxCollider>().bounds.center.x;
        // Entity center, y coord
        float entity2CenterY = entity.GetComponent<BoxCollider>().bounds.center.y;

        // Distance between vehicle and entity's center squared
        float distanceSquared = Mathf.Pow((entity1CenterX - entity2CenterX), 2) + Mathf.Pow((entity1CenterY - entity2CenterY), 2);

        // Distance between vehicle and entity
        return distanceSquared;
    }

    /// <summary>
    /// Calculates a force that will turn the vehicle object towards a specific target position
    /// </summary>
    /// <param name="targetPosition">Position of the target that is being seeked</param>
    /// <returns>A force that will turn the vehicle object towards a specific target</returns>
    public Vector3 Seek(Vector3 targetPosition)
    {
        // calculating our desired velocity
        // a vector towards our targetPosition
        Vector3 desiredVelocity = targetPosition - position;

        // Scale desired velocity to equal our max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate the seek steering force
        Vector3 seekingForce = desiredVelocity - velocity;

        //direction = seekingForce.normalized;

        return seekingForce;
    }

    /// <summary>
    /// Calculates a force that will turn the entity object towards a specific target GameObject
    /// </summary>
    /// <param name="targetObject">Object to be targeted</param>
    /// <returns>A force that will turn the entity object towards a specific target GameObject</returns>
    public Vector3 Seek(GameObject targetObject)
    {
        return Seek(targetObject.transform.position);
    }

    /// <summary>
    /// Calculates a force that will turn the entity object towards a specific target GenericEntity object
    /// </summary>
    /// <param name="targetVehicle">Vehicle to be targeted</param>
    /// <returns>A force that will turn the entity object towards a specific target GenericEntity object</returns>
    public Vector3 Seek(GenericEntity targetEntity)
    {
        return Seek(targetEntity.position);
    }

    public Vector3 Flee(Vector3 targetPosition)
    {
        // calculating our desired velocity
        // a vector away from our targetPosition
        Vector3 desiredVelocity = position - targetPosition;

        // Scale desired velocity to equal our max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate the flee steering force
        Vector3 fleeingForce = desiredVelocity - velocity;

        return fleeingForce;


    }

    public Vector3 Flee(GameObject fleeObject)
    {
        return Flee(fleeObject.transform.position);
    }

    public Vector3 Flee(GenericEntity targetEntity)
    {
        return Flee(targetEntity.Position);
    }
    //public abstract void CalculateSteeringForces();
    virtual public void OnCollisionEnter2D(Collision2D collision) {
        // Check if collided thing is a bullet
        if (collision.gameObject.GetComponent<Bullet>()) {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            // Apply damage
            Damage(bullet.damage);
        }
    }

    public bool Damage(int amount) {
        // Applies damage to the entity
        health -= amount;

        // Damage occurred
        return true;
    }

    public void SetGet(Gun newGun) {
        // Sets the entity's currently held gun
        gun = newGun;
        // FIXME: Where does the old gun go?
    }

    public void FireGun() {
        // Shoot the gun, using the current direction/position of this entity
        gun.Shoot(_rigidBody.rotation, _rigidBody.position);
    }
}
