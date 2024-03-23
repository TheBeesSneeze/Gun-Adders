using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetScript : MonoBehaviour
{
    public bool touchingGround = false;
    private void OnTriggerEnter(Collider other){
        if (other.tag == "Ground") {
            touchingGround = true;
        }
    }
    private void OnTriggerExit(Collider other){
        if (other.tag == "Ground"){
            touchingGround = false;
        }
    }
}
