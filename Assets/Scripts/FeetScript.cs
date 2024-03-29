/*******************************************************************************
 * File Name :         FeetScript.cs
 * Author(s) :         Tyler Bouchard
 * Creation Date :     3/23/2024
 *
 * Brief Description : hi my name is tyler and i looooooooove feet and toes
 *****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FeetScript : MonoBehaviour
{
    public LayerMask GroundLayers;
    [FormerlySerializedAs("touchingGround")] public bool Grounded = false;

    public Vector3 GroundNormal;
    private bool groundedPrevFrame;
    // private void OnTriggerEnter(Collider other){
    //     //if (other.tag == "Ground") {
    //         touchingGround = true;
    //     //}
    // }
    // private void OnTriggerExit(Collider other){
    //     //if (other.tag == "Ground"){
    //         touchingGround = false;
    //     //}
    // }
    private void FixedUpdate()
    {
        groundedPrevFrame = Grounded;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 0.25f, GroundLayers))
        {
            GroundNormal = hitInfo.normal;
            if(!groundedPrevFrame)
            {
                if(AudioManager.instance != null)
                    AudioManager.instance.Play("Land");
            }
            Grounded = true;
        }
        else
        {
            GroundNormal = Vector3.zero;
            Grounded = false;
        }
    }
}
