using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget1 : MonoBehaviour
{

    public Transform playerTransform;

    private Q_Control1 myRotate;


    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerTransform.position;

        myRotate = GameObject.Find("Frame").GetComponent<Q_Control1>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + offset;

        if(Input.GetKeyDown(KeyCode.D))
        {

            transform.RotateAround(transform.position, transform.up, 1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            transform.RotateAround(transform.position, transform.up, -1);
        }

    }
}
