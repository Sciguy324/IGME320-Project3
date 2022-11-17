using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCopy : MonoBehaviour
{
    private SpriteRenderer ParentSpriteRenderer;
    private Transform ParentTransform;
    private Platform arena;
    public bool visX;
    public bool visY;
    
    // Start is called before the first frame update
    void Start()
    {
        arena = GameObject.Find("Platform").GetComponent<Platform>();
        GameObject parent = this.transform.parent.gameObject;
        ParentTransform = parent.GetComponent<Transform>();
        ParentSpriteRenderer = parent.GetComponent<SpriteRenderer>();

        GetComponent<SpriteRenderer>().sprite = ParentSpriteRenderer.sprite;
    }

    private Vector3 ComputeOffset()
    {   
        // Determines where to place the visual copy when rendered
        Vector3 Offset = new Vector3(0.0f, 0.0f, 0.0f);

        // Apply horizontal offset, if applicable
        if (visX) {
            if (ParentTransform.position.x < arena.center().x) {
                Offset.x = arena.width();
            } else {
                Offset.x = -arena.width();
            }
        }

        // Apply vertical offset, if applicable
        if (visY)
        {
            if (ParentTransform.position.y < arena.center().y) {
                Offset.y = arena.height();
            } else {
                Offset.y = -arena.height();
            }
        }

        return Offset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ParentTransform.position + ComputeOffset();
        transform.rotation = ParentTransform.rotation;
    }
}
