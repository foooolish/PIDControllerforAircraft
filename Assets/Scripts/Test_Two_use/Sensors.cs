using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Sensors : MonoBehaviour
{

    //private float previous_angle;
    //private float now_angle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("x1:" + transform.localEulerAngles.x);
        //Debug.Log("x2:" + transform.localEulerAngles.x);

        //以下方法的得到的欧拉角和直接得到的结果一样
        //角速度输出
        //now_angle = transform.localEulerAngles.x;
        //float w = (now_angle - previous_angle) / Time.deltaTime;
        //previous_angle = now_angle;

        //float test_angle = w * Time.deltaTime;
        //if(test_angle>180)
        //{
        //    test_angle = test_angle - 360;
        //}

        ////Debug.Log("角度差：" + (now_angle - previous_angle));
        ////Debug.Log("每帧所用时间：" + Time.deltaTime);
        ////Debug.Log("x角速度：" + w);
        //Debug.Log("转动的角度：" + test_angle);
        

    }


    /*
     * 这里就直接返回欧拉角，实际是通过获取传感器的角速度和加速度计算欧拉角获得的（用四元数计算）
     * 
     */
    //返回局部坐标系的欧拉角

    //Pitch：俯仰
    public float getlocalEulerAngleX()
    {
        float myData = transform.localEulerAngles.x;
        if(myData > 180)
        {
            myData = myData - 360;
        }
        return myData;
    }

    //Yaw：偏航
    public float getlocalEulerAngleY()
    {
        float myData = transform.localEulerAngles.y;
        //Debug.Log("Yaw：" + myData);
        if (myData > 180)
        {
            myData = myData - 360;
        }
        return myData;
    }

    //Roll：侧倾
    public float getlocalEulerAngleZ()
    {
        float myData = transform.localEulerAngles.z;
        if (myData > 180)
        {
            myData = myData - 360;
        }
        return myData;
    }

    //获取悬停的地球坐标，该坐标可通过磁力计计算获得
    //Pitch：俯仰；Roll：侧倾；Yaw：偏航
    public float getFloatPositionX()
    {
        return transform.position.x;
    }

    public float getFloatPositionY()
    {
        return transform.position.y;
    }

    public float getFloatPositionZ()
    {
        return transform.position.z;
    }

}


