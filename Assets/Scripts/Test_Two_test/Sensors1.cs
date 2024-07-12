using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Sensors1 : MonoBehaviour
{
    public Gyroscope mGyroscope;
    public Accelerometer mAccelerometer;
    public Magnetometer mMagnetometer;
    public Barometer mBarometer;

    // Start is called before the first frame update
    void Start()
    {
        //在这里初始化三个传感器
        Rigidbody testRb = GetComponent<Rigidbody>();
        mGyroscope = new Gyroscope(testRb);
        mAccelerometer = new Accelerometer(testRb);
        mMagnetometer = new Magnetometer(testRb);
        mBarometer = new Barometer(testRb);
    }

    // Update is called once per frame
    void Update()
    {
        //测试传感器
        Debug.Log("角速度：" + mGyroscope.getAngularVelocityX());

        Debug.Log("高度：" + mBarometer.getGlobalHigh());
    }


}



//陀螺仪
public class Gyroscope
{
    public Rigidbody measureBody;

    public Gyroscope(Rigidbody measureBody)
    {
        this.measureBody = measureBody;
    }

    /*********************要加噪声********************/
    
    public float getAngularVelocityX()
    {

        return measureBody.angularVelocity.x * 180 / Mathf.PI + Random.Range(-1f, 1f);

        
    }

    public float getAngularVelocityY()
    {
        return measureBody.angularVelocity.y + Random.Range(-1f, 1f);

    }

    public float getAngularVelocityZ()
    {
        return measureBody.angularVelocity.z + Random.Range(-1f, 1f);
    }
}



//加速度计
public class Accelerometer
{
    public Rigidbody measureBody;

    public Accelerometer(Rigidbody measureBody)
    {
        this.measureBody = measureBody;
    }
    /*********************要加噪声********************/

    //先求总质量，然后求合力（重力和升力向量的和）。
}

//磁力计
public class Magnetometer
{
    public Rigidbody measureBody;

    public Magnetometer(Rigidbody measureBody)
    {
        this.measureBody = measureBody;
    }
    /*********************要加噪声********************/

}


//气压计
public class Barometer
{
    public Rigidbody measureBody;
    public Barometer(Rigidbody measureBody)
    {
        this.measureBody = measureBody;
    }
    /*********************要加噪声********************/
    public float getGlobalHigh()
    {
        return measureBody.position.y;
    }
}