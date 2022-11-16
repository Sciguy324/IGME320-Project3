using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCopy : MonoBehaviour
{
    private SpriteRenderer ParentSpriteRenderer;
    private Transform ParentTransform;
    public Vector3 Offset;
    private Platform arena;
    
    // Start is called before the first frame update
    void Start()
    {
        arena = GameObject.Find("Platform").GetComponent<Platform>();
        SetBoxDims(arena);
        GameObject parent = this.transform.parent.gameObject;
        ParentTransform = parent.GetComponent<Transform>();
        ParentSpriteRenderer = parent.GetComponent<SpriteRenderer>();

        GetComponent<SpriteRenderer>().sprite = ParentSpriteRenderer.sprite;
    }

    // Sets the distance of the offset based on the provided box dimensions
    public void SetBoxDims(Vector2 newDimensions)
    {
        Vector3 newOffset = new Vector3(0.0f, 0.0f, 0.0f);

        if (Offset.x > 0.0f) {
            newOffset.x = newDimensions.x;
        } else if (Offset.x < 0.0f) {
            newOffset.x = -newDimensions.x;
        }

        if (Offset.y > 0.0f) {
            newOffset.y = newDimensions.y;
        } else if (Offset.y < 0.0f) {
            newOffset.y = -newDimensions.y;
        }

        Offset = newOffset;
    }

    public void SetBoxDims(float width, float height)
    {
        SetBoxDims(new Vector2(width, height));
    }

    public void SetBoxDims(Platform platform)
    {
        SetBoxDims(platform.width(), platform.height());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = ParentTransform.position + Offset;
        transform.rotation = ParentTransform.rotation;
    }
}
