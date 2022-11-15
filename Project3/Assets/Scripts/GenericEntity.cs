using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericEntity : MonoBehaviour
{
    public float speed = 1.0f; // How fast the entity moves
    public int maxHealth = 1; // The default number of hearts the entity will have upon spawning
    private int health; // The amount of health that the entity has, is a number instead of like the player who has hearts
    protected Vector3 position; // Position of the entity
    protected Vector3 direction; // The direction the entity is facing
    protected Vector3 velocity; // The velocity of the entity
    protected Vector3 acceleration; // The acceleration of the entity
    protected GameManager manager; // The manager for the entire game, a reference to it
    public GameObject arena;

    private float radius; // Radius of entity

    public SpriteRenderer sprite;

    public float Radius => radius; // Accessor for the radius variable

    // The bounds of the level/screen
    protected float minX;
    protected float maxX;
    protected float minY;
    protected float maxY;

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
        health = maxHealth;
        position = transform.position;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        // TODO: UPDATE IN TERMS OF THE ACTUAL ARENA OBJECT IN THE UNITY SCENE
        // arena = GameObject.Find("Plane");
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        minX = (arena.transform.position.x - (arena.transform.localScale.x / 2)) * 10;
        maxX = (arena.transform.position.x + (arena.transform.localScale.x / 2)) * 10;
        minY = (arena.transform.position.y - (arena.transform.localScale.y / 2)) * 10;
        maxY = (arena.transform.position.y + (arena.transform.localScale.y / 2)) * 10;
        radius = sprite.bounds.extents.x;
        //randomAngle = Random.Range(0, 360);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateSteeringForces();

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

        // Don't worry about the z axis, it is 2d
        velocity.z = 0;

        // Step 2: add our velocity to our position
        position += velocity * Time.deltaTime;

        // Don't worry about the z axis, it is 2d
        position.z = 0;

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
    }

    // Determines the closest replica target position of the provided target position
    Vector3 trueNearestPosition(Vector3 targetPosition)
    {
        float targetX = targetPosition.x;
        float targetY = targetPosition.y;

        float width = maxX-minX;
        float height = maxY-minY;

        // Find closest x-position
        if (Mathf.Abs(targetPosition.x - position.x) > width/2)
        {
            if (targetPosition.x > position.x)
            {
                targetX -= width;
            } else {
                targetX += width;
            }
        }

        // Find closest y-position
        if (Mathf.Abs(targetPosition.y - position.y) > height/2)
        {
            if (targetPosition.y > position.y)
            {
                targetY -= height;
            } else {
                targetY += height;
            }
        }

        return new Vector3(targetX, targetY, 0.0f);
    }

    protected Vector3 StayInBounds()
    {
        //arena.GetComponent<SpriteRenderer>()
        Vector3 futurePos = new Vector3(1.0f, 1.0f, 1.0f); //GetFuturePosition(1);
        //Vector3 posToFlee = boundaries.ClosestPoint(position);
        //float distBetween = Vector3.Distance(position, posToFlee);
        //distBetween = Mathf.Max(distBetween, 0.001f);
        if (position.x > maxX || position.x < minX ||
            position.y > maxY || position.y < minY)
        {
            //return Seek(posToFlee) * 2000;
        }
        else if (futurePos.x >= maxX || futurePos.x <= minX ||
            futurePos.y >= maxY || futurePos.y <= minY)
        {
            //return Flee(posToFlee) * (1 / distBetween);
        }


        return Vector3.zero;
    }

    /// <summary>
    /// Makes sure each entity is separated, specifically the enemies
    /// </summary>
    /// <typeparam name="T">List of entities</typeparam>
    /// <param name="entities">Entities</param>
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
        return Seek(targetEntity.transform.position);
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

        // Handle the case of zero-health
        if (health <= 0) {
            Die();
        }

        // Damage occurred
        return true;
    }

    virtual public void Die() {
        // Handle what to do upon death.  Override in subclass
    }

    public void SetGun(Gun newGun) {
        // Sets the entity's currently held gun
        gun = newGun;
        // FIXME: Where does the old gun go?
    }

    public void FireGun() {
        // Shoot the gun, using the current direction/position of this entity
        if (gun != null) {
            gun.Shoot();
        }
    }

    public void Respawn(Vector2 pos) {
    
        // Set position
        position = pos;
        
        // Set stats
        health = maxHealth;
        
        // Reactivates this enemy with new information
        gameObject.SetActive(true);
        
    }

    public abstract void CalculateSteeringForces();

}
