using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 hitPoint;
    public LayerMask shootLayers;
    private SpringJoint joint;
    private float maxDist = 100f;
    private PlayerControler playerControler;
    private LineRenderer renderer;
    void Start()
    {
        InputEvents.Instance.ShootStarted.AddListener(StartGrapple);
        InputEvents.Instance.ShootCanceled.AddListener(StopGrapple);
        playerControler = GetComponent<PlayerControler>();
        renderer = gameObject.AddComponent<LineRenderer>();
        renderer.endWidth = 0.05f;
        renderer.startWidth = 0.05f;
        renderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (joint == null)
        {
            renderer.positionCount = 0;
            return;
        }

        renderer.positionCount = 2;
        renderer.SetPosition(0, transform.position);
        renderer.SetPosition(1, hitPoint);
    }

    private void StartGrapple()
    {
        if (Physics.Raycast(playerControler.cam.position, playerControler.cam.forward, out RaycastHit hit, maxDist, shootLayers)) {
            hitPoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = hitPoint;

            float distanceFromPoint = Vector3.Distance(transform.position, hitPoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.35f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 100f;
            joint.damper = 70f;
            joint.massScale = 4.5f;
        }
    }

    private void StopGrapple()
    {
        if (joint)
        {
            Destroy(joint);
        }
    }
}
