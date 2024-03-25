using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    public GameObject crossHairImage;
    public GameObject newCrossHairImage;
    public float gunRange = 1000f;
    public LayerMask mask;
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        //Debug.DrawLine(transform.position, transform.forward, Color.red);
        bool hitEnemy = Physics.Raycast(ray, out RaycastHit hit, gunRange, mask);
        if (hitEnemy) {
            if (hit.collider.gameObject.GetComponent<EnemyType>() != null) {
                newCrossHairImage.SetActive(true);
                crossHairImage.SetActive(false);
            }
        }else {
            newCrossHairImage.SetActive(false);
            crossHairImage.SetActive(true);
        }
    }
}