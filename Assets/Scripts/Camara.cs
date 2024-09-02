using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public GameObject target; 
    public Vector3 offset; 

    void Start()
    {
        transform.position = target.transform.position + target.transform.TransformDirection(offset);
        transform.LookAt(target.transform); 
    }

    public void FollowPlayer()
    {
        if (target)
        {
            transform.position = target.transform.position + target.transform.TransformDirection(offset);
            transform.rotation = target.transform.rotation;
        }
    }

    void LateUpdate() 
    {
        FollowPlayer();
    }
}
