using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    public Transform playerTransform;

    private Q_Control myRotate;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerTransform.position;

        myRotate = GameObject.Find("Frame").GetComponent<Q_Control>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + offset;

        float myEulerAngles = 180+transform.eulerAngles.y;
        //Debug.Log("myEulerAngles" + myEulerAngles);
        transform.Rotate(transform.up, myRotate.setYaw - myEulerAngles);

        
    }
}
