using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_bow : MonoBehaviour
{

    public Transform startPoint;
    public GameObject arrow;
    public Camera playerCamera;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Shoot")) {
            ShootArrow();
        }
        
    }

    public void ShootArrow() {

        Arrow arrow_i = Instantiate(arrow, startPoint.position, Quaternion.identity).GetComponent<Arrow>();        
        arrow_i.direction = playerCamera.transform.forward;

    }
}
