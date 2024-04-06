using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GrapplingHook : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 hitPoint;
    public LayerMask shootLayers;
    private SpringJoint joint;
    private float maxDist = 1000f;
    private LineRenderer hookRenderer;
    private Transform cam;

    

    [Tooltip("How much of the distance between the grapple hook and the player will be used.")]
    public float maxDistanceFromPointMultiplier = 0.8f;

    [Tooltip("Multiplier between the min distance between the player and the hook point.")]
    public float minDistanceFromPointMultiplier = 0.25f;

    [Tooltip("Spring to keep the player moving towards the target.")]
    public float jointSpring = 4.5f;

    [Tooltip("Damper force of the joint.")]
    public float jointDamper = 7f;

    [Tooltip("Spring for when player is holding space")]
    public float jumpingSpring = 9;

    [Tooltip("Damper for when player is holding space")]
    public float jumpingDamper = 4f;

    [Tooltip("Scalar for mass of the player and potential connected objects.")]
    public float jointMassScale = 4.5f;

    [Tooltip("Force to send the player in the direction of the grapple.")]
    public float jointForceBoost = 20f;

    [SerializeField] private Transform gunModel, gunFirePoint, gunFollowPoint, gunExitPoint;
    private Rigidbody rb;

    [Header("Snake town")]
    [SerializeField] private Transform Head;
    [SerializeField] private Material BodyMaterial;

    [ReadOnly] public static bool isGrappling;

    void Start()
    {
        InputEvents.Instance.SecondaryStarted.AddListener(StartGrapple);
        InputEvents.Instance.SecondaryCanceled.AddListener(StopGrapple);
        hookRenderer = gameObject.AddComponent<LineRenderer>();
        //hookRenderer.endWidth = 0.05f;
        //hookRenderer.startWidth = 0.05f;
        hookRenderer.positionCount = 2;
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();

        //snake
        hookRenderer.material = BodyMaterial;
        hookRenderer.startWidth = 1;
        hookRenderer.endWidth = 1;
        hookRenderer.textureMode = LineTextureMode.Tile;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (PauseMenu.IsPaused)
            return;

        if (joint == null)
        {
            hookRenderer.positionCount = 0;
            gunModel.position = Vector3.MoveTowards(gunModel.position, gunExitPoint.position, Time.deltaTime * 2.5f);
            gunModel.rotation =
                Quaternion.RotateTowards(gunModel.rotation, gunExitPoint.rotation, Time.deltaTime * 2.5f);
        }
        else
        {
            gunModel.position = Vector3.MoveTowards(gunModel.position, gunFollowPoint.position, Time.deltaTime * 15f);
            gunModel.rotation =
                Quaternion.RotateTowards(gunModel.rotation, gunFollowPoint.rotation, Time.deltaTime * 15f);
            hookRenderer.positionCount = 2;
            hookRenderer.SetPosition(0, gunFirePoint.position);
            hookRenderer.SetPosition(1, hitPoint);

            if(Head != null) //so no head?
            {
                Head.position = hitPoint;
                Head.eulerAngles = gunModel.forward;
            }
        }
    }

    private void Update()
    {
        if (joint == null) return;

        if(InputEvents.Instance.JumpPressed)
        {
            joint.spring = jumpingSpring;
            joint.damper = jumpingDamper;
        }
        else
        {
            joint.spring = jointSpring;
            joint.damper = jointDamper;
        }
    }

    private void StartGrapple()
    {
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxDist, shootLayers))
        {
            isGrappling = true;

            hitPoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = hitPoint;

            float distanceFromPoint = Vector3.Distance(transform.position, hitPoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * maxDistanceFromPointMultiplier;
            joint.minDistance = distanceFromPoint * minDistanceFromPointMultiplier;

            joint.spring = jointSpring;
            joint.damper = jointDamper;
            joint.massScale = jointMassScale;
            rb.AddForce((hitPoint - transform.position).normalized * jointForceBoost, ForceMode.Impulse);
        }
    }

    private void StopGrapple()
    {
        isGrappling = false;
        if (joint)
        { 
            Destroy(joint);
        }
    }
}