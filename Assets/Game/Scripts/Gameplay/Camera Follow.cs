using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed = 5;

    // Start is called before the first frame update  
    //void Start()
    //{
    //    target = Object.FindFirstObjectByType<Player>().transform;
    //}

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Update is called once per frame  
    void LateUpdate()
    {
        if(target == null)
        {
            return;
        }
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}
