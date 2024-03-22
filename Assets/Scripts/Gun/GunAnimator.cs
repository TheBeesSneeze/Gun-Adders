/*******************************************************************************
* File Name :         GunAnimator.cs
* Author(s) :         Toby Schamberger
* Creation Date :     3/21/2024
*
* Brief Description : bobs gun up and down when walking
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GunAnimator : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        InputEvents.Instance.MoveStarted.AddListener(MoveStart);
        InputEvents.Instance.MoveCanceled.AddListener(MoveEnd);
        InputEvents.Instance.ShootStarted.AddListener(ShootStart);
        InputEvents.Instance.ShootCanceled.AddListener(ShootEnd);
    }

    private void MoveStart()
    {
        animator.SetBool("Walking", true);
    }

    private void MoveEnd()
    {
        animator.SetBool("Walking", false);
    }

    private void ShootStart()
    {
        animator.SetBool("Shooting", true);
    }

    private void ShootEnd()
    {
        animator.SetBool("Shooting", false);
    }

    /*
    public float bobHeightMultiplier = 0.5f;
    public float bobSpeed = 10;

    [ReadOnly] public bool bobbingGun;
    private float bobbingStartTime;
    private float startY;



    
    // Update is called once per frame
    void Start()
    {
        startY = transform.localPosition.y;

        InputEvents.Instance.MoveStarted.AddListener(StartBob);
        InputEvents.Instance.MoveCanceled.AddListener(CancelBob) ;
    }

    void Update()
    {
        if (!bobbingGun) return;

        if (InputEvents.ShootPressed) return;

        float offset = Mathf.Sin((Time.time - bobbingStartTime)* bobSpeed) * bobHeightMultiplier;
        Vector3 p = transform.localPosition;
        p.y = startY + offset;
        transform.localPosition = p;
    }

    private void StartBob()
    {
        bobbingGun = true;
        bobbingStartTime = Time.time;
    }

    private void CancelBob()
    {
        bobbingGun = false;


        transform.localPosition = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);
    }
    */
}
