using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class grapplingHook : MonoBehaviour
{
    public float distance = 30f;
    public float spring = 4.5f;
    public float damping = 7f;
    public float massScale = 4.5f;

    public Rigidbody playerRb;

    bool grappling = false;

    SpringJoint sj = new SpringJoint();
    RaycastHit hit;
    LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(3))
        {
            if(Physics.Raycast(transform.position, transform.forward, out hit, distance))
            {
                sj = playerRb.AddComponent<SpringJoint>();
                sj.autoConfigureConnectedAnchor = false;
                sj.damper = damping;
                sj.spring = spring;
                sj.maxDistance = Vector3.Distance(transform.position, hit.point) * 0.8f; 
                sj.connectedAnchor = hit.point;
                sj.massScale = massScale;
                lr.SetPosition(1, hit.point);
                grappling = true;
            }
        }
        if (Input.GetMouseButtonUp(3))
        {
            if (sj != null)
            {
                Destroy(sj);
                grappling = false;
            }
        }
        if (grappling)
        {
            lr.SetPosition(0, transform.position);
        }
        else
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position);
        }
    }
}
