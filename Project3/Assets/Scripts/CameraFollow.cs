using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GenericEntity FollowedEntity;
    public float TimeConstant = 0.01f;
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Smooth pan over to target position assuming an exponentially decaying approach
        Vector3 diff = FollowedEntity.transform.position - _transform.position;
        _transform.position = _transform.position + diff * (1-Mathf.Exp(-Time.deltaTime/TimeConstant));
        
        // Force z-component
        _transform.position = new Vector3(_transform.position.x, _transform.position.y, -1.0f);
    }
}
