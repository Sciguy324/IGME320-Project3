using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve size information
        Vector3 scale = GetComponent<Transform>().localScale;
        minX = -scale.x/2;
        maxX = scale.x/2;
        minY = -scale.y/2;
        maxY = scale.y/2;
    }

    public float width()
    {
        return maxX - minX;
    }

    public float height()
    {
        return maxY - minY;
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
        
    }
}
