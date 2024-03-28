using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 hitPoint;
    public LayerMask shootLayers;
    private SpringJoint joint;
    private float maxDist = 1000f;
    private PlayerControler playerControler;
    private LineRenderer renderer;
    private Transform cam;
    public float maxDistanceFromPointMultiplier = 0.8f;
    public float minDistanceFromPointMultiplier = 0.25f;
    public float jointSpring = 4.5f;
    public float jointDamper = 7f;
    public float jointMassScale = 4.5f;
    public float jointForceBoost = 20f;
    [SerializeField] private Transform gunModel, gunFirePoint, gunFollowPoint, gunExitPoint;
    private Rigidbody rb;
    void Start()
    {
        InputEvents.Instance.SecondaryStarted.AddListener(StartGrapple);
        InputEvents.Instance.SecondaryCanceled.AddListener(StopGrapple);
        playerControler = GetComponent<PlayerControler>();
        renderer = gameObject.AddComponent<LineRenderer>();
        renderer.endWidth = 0.05f;
        renderer.startWidth = 0.05f;
        renderer.positionCount = 2;
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (joint == null)
        {
            renderer.positionCount = 0;
            gunModel.position = Vector3.MoveTowards(gunModel.position, gunExitPoint.position, Time.deltaTime * 2.5f);
            gunModel.rotation = Quaternion.RotateTowards(gunModel.rotation, gunExitPoint.rotation, Time.deltaTime * 2.5f);
        }
        else
        {
            gunModel.position = Vector3.MoveTowards(gunModel.position, gunFollowPoint.position, Time.deltaTime * 15f);
            gunModel.rotation = Quaternion.RotateTowards(gunModel.rotation, gunFollowPoint.rotation, Time.deltaTime * 15f);
            renderer.positionCount = 2;
            renderer.SetPosition(0, gunFirePoint.position);
            renderer.SetPosition(1, hitPoint);
        }

    }

    private void StartGrapple()
    {
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxDist, shootLayers))
        {
            hitPoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = hitPoint;

            float distanceFromPoint = Vector3.Distance(transform.position, hitPoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * maxDistanceFromPointMultiplier;
            joint.minDistance = distanceFromPoint * minDistanceFromPointMultiplier;

            //Adjust these values to fit your game.
            joint.spring = jointSpring;
            joint.damper = jointDamper;
            joint.massScale = jointMassScale;
            rb.AddForce((hitPoint - transform.position).normalized * jointForceBoost, ForceMode.Impulse);
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