/*******************************************************************************
 * File Name :         FeetScript.cs
 * Author(s) :         Tyler Bouchard
 * Creation Date :     3/23/2024
 *
 * Brief Description : hi my name is tyler and i looooooooove feet and toes
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetScript : MonoBehaviour
{
    public bool touchingGround = false;
    private void OnTriggerEnter(Collider other){
        //if (other.tag == "Ground") {
            touchingGround = true;
        //}
    }
    private void OnTriggerExit(Collider other){
        //if (other.tag == "Ground"){
            touchingGround = false;
        //}
    }
}
