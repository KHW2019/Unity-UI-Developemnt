using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knifes : MonoBehaviour
{
    private Rigidbody rb;

    private bool TargetIsHit = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //it will only stick to the first target it hits
        if (TargetIsHit)
        {
            return;
        }
        else
        {
            TargetIsHit = true;
        }

        // make sure the knifes stickes to the surfaces of the object
        rb.isKinematic = true;

        //if the objects moves, it will move witht the object
        transform.SetParent(collision.transform);

    }
}
