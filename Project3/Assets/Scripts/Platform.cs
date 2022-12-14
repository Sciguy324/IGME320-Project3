using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float minX { get; private set; }
    public float maxX { get; private set; }
    public float minY { get; private set; }
    public float maxY { get; private set; }
    public static Platform Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve size information
        Vector3 scale = GetComponent<Transform>().localScale;
        minX = -scale.x/2;
        maxX = scale.x/2;
        minY = -scale.y/2;
        maxY = scale.y/2;
        Instance = this;
    }

    public float width()
    {
        return maxX - minX;
    }

    public float height()
    {
        return maxY - minY;
    }

    public Vector3 center()
    {
        return new Vector3((minX + maxX)/2, (minY + maxY)/2, 0.0f);
    }

    // Returns the closest square-distance between two points in a wrapped coordinate system
    public float TrueDistanceSquared(Vector3 firstPosition, Vector3 secondPosition)
    {
        // Find replica position of second position
        Vector3 trueSecondPosition = trueNearestPosition(firstPosition, secondPosition);

        // Get distance between two points and return the result
        return (trueSecondPosition - firstPosition).sqrMagnitude;
    }

    public Vector3 trueNearestPosition(Vector3 firstPosition, Vector3 targetPosition)
    {
        Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);

        // Find closest x-position
        if (Mathf.Abs(targetPosition.x - firstPosition.x) > width()/2)
        {
            if (targetPosition.x > firstPosition.x)
            {
                offset.x -= width();
            } else {
                offset.x += width();
            }
        }

        // Find closest y-position
        if (Mathf.Abs(targetPosition.y - firstPosition.y) > height()/2)
        {
            if (targetPosition.y > firstPosition.y)
            {
                offset.y -= height();
            } else {
                offset.y += height();
            }
        }

        return targetPosition + offset;
    }

    public Vector3 WrappedPosition(Vector3 position) {
        Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);

        // Wrap x-position around
        if (position.x > maxX)
        {
            offset.x -= width();
        } else if (position.x < minX)
        {
            offset.x += width();
        }

        // Wrap y-position around
        if (position.y > maxY)
        {
            offset.y -= height();
        } else if (position.y < minY)
        {
            offset.y += height();
        }

        // Apply offset and return the result
        return position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(new Vector2(minX, minY), new Vector2(minX, maxY));
        Debug.DrawLine(new Vector2(minX, maxY), new Vector2(maxX, maxY));
        Debug.DrawLine(new Vector2(maxX, maxY), new Vector2(maxX, minY));
        Debug.DrawLine(new Vector2(maxX, minY), new Vector2(minX, minY));
    }
}
