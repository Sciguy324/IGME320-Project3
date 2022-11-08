using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEntity : MonoBehaviour
{
    private int health; // The amount of health that the entity has, is a number instead of like the player who has hearts
    private float speed; // How fast the entity moves
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
        randomAngle = Random.Range(0, 360);
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
        Bounds boundaries = manager.WorldBounds;
        Vector3 futurePos = GetFuturePosition(1);
        Vector3 posToFlee = boundaries.ClosestPoint(position);
        float distBetween = Vector3.Distance(position, posToFlee);
        distBetween = Mathf.Max(distBetween, 0.001f);
        if (position.x > maxX || position.x < minX ||
            position.z > maxZ || position.z < minZ)
        {
            return Seek(posToFlee) * 2000;
        }
        else if (futurePos.x >= maxX || futurePos.x <= minX ||
            futurePos.z >= maxZ || futurePos.z <= minZ)
        {
            return Flee(posToFlee) * (1 / distBetween);
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
    public abstract void CalculateSteeringForces();
}
