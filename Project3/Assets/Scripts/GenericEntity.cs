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
    public Platform arena;

    private float radius; // Radius of entity

    public SpriteRenderer sprite;

    public float Radius => radius; // Accessor for the radius variable

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
        arena = GameObject.Find("Platform").GetComponent<Platform>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        radius = sprite.bounds.extents.x;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateSteeringForces();

        // This is needed for all entities
        UpdatePosition();
        WrapAround();
        SetTransform();

        // Debug wrap-around visualization
        float w = arena.width();
        float h = arena.height();
        DebugBox(0.0f, 0.0f);
        DebugBox(w, 0.0f);
        DebugBox(-w, 0.0f);
        DebugBox(0.0f, h);
        DebugBox(0.0f, -h);
        DebugBox(w, h);
        DebugBox(w, -h);
        DebugBox(-w, h);
        DebugBox(-w, -h);
    }

    void DebugBox(float dx, float dy) {
        Vector2 pos = (Vector2)position + new Vector2(dx, dy);
        Debug.DrawLine(new Vector2(pos.x - 1, pos.y - 1),
                       new Vector2(pos.x - 1, pos.y + 1));

        Debug.DrawLine(new Vector2(pos.x - 1, pos.y + 1),
                       new Vector2(pos.x + 1, pos.y + 1));

        Debug.DrawLine(new Vector2(pos.x + 1, pos.y + 1),
                       new Vector2(pos.x + 1, pos.y - 1));

        Debug.DrawLine(new Vector2(pos.x + 1, pos.y - 1),
                       new Vector2(pos.x - 1, pos.y - 1));
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
    public void SetTransform()
    {
        transform.position = position;
    }

    // Wraps the entity's position around
    void WrapAround()
    {
        position = arena.WrappedPosition(position);
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
        // Compute replica position
        targetPosition = arena.trueNearestPosition(position, targetPosition);

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
        GetComponent<Transform>().position = pos;
        
        // Set stats
        health = maxHealth;
        
        // Reactivates this enemy with new information
        gameObject.SetActive(true);
        
    }

    public abstract void CalculateSteeringForces();

}
