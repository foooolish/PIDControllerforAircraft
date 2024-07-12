using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{

    private GameObject rotor1;
    private GameObject rotor2;
    private GameObject rotor3;
    private GameObject rotor4;

    private GameObject mainBody;
    public float speed;

    //声明四个PID
    private PID rotor1_PID;
    private PID rotor2_PID;
    private PID rotor3_PID;
    private PID rotor4_PID;

    // Start is called before the first frame update
    void Start()
    {
        //获取四个电机
        rotor1 = GameObject.Find("rotor1");
        rotor2 = GameObject.Find("rotor2");
        rotor3 = GameObject.Find("rotor3");
        rotor4 = GameObject.Find("rotor4");

        //获取主体
        mainBody = GameObject.Find("MainBody");

        //实例化四个PID控制器


    }

    // Update is called once per frame
    void Update()
    {

        //获取全局坐标系


        ////获取相对于局部坐标系的欧拉角
        //Debug.Log("x：" + mainBody.transform.localEulerAngles.x);
        //Debug.Log("y：" + mainBody.transform.localEulerAngles.y);
        //Debug.Log("z：" + mainBody.transform.localEulerAngles.z);

        ////获取局部坐标系各轴向量
        //Debug.Log(mainBody.transform.forward); //z轴
        //Debug.Log(mainBody.transform.right); //x轴
        //Debug.Log(mainBody.transform.up); //y轴

        //rotor1.GetComponent<Rigidbody>().AddForce(rotor1.transform.up * speed);
        //rotor2.GetComponent<Rigidbody>().AddForce(rotor2.transform.up * speed);

        //rotor3.GetComponent<Rigidbody>().AddForce(rotor3.transform.up * speed);
        //rotor4.GetComponent<Rigidbody>().AddForce(rotor4.transform.up * speed);

        if (Input.GetAxis("Vertical") > 0)
        {
            Debug.Log("前进");

            //float setPoint=40;//设置旋转角度
            //float measuredPoint= mainBody.transform.localEulerAngles.x;//测量现在的角度

            //Debug.Log("measuredPoint：" + measuredPoint);

            //rotor1_PID.RenewCalculData(setPoint, measuredPoint);//更新测量数据

            //float speed1 = rotor1_PID.Calcul();

            //Debug.Log("speed1：" + speed1);

            rotor3.GetComponent<Rigidbody>().AddForce(rotor3.transform.up * speed);
            rotor4.GetComponent<Rigidbody>().AddForce(rotor4.transform.up * speed);
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            Debug.Log("后退");
            rotor1.GetComponent<Rigidbody>().AddForce(rotor1.transform.up * speed);
            rotor2.GetComponent<Rigidbody>().AddForce(rotor2.transform.up * speed);
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            Debug.Log("向左");
            rotor1.GetComponent<Rigidbody>().AddForce(rotor1.transform.up * speed);
            rotor3.GetComponent<Rigidbody>().AddForce(rotor3.transform.up * speed);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            Debug.Log("向右");
            rotor2.GetComponent<Rigidbody>().AddForce(rotor2.transform.up * speed);
            rotor4.GetComponent<Rigidbody>().AddForce(rotor4.transform.up * speed);
        }

        if (Input.GetKey(KeyCode.Z))    //实现检测按键一直按下
        {
            Debug.Log("左旋");
            //rotor1.GetComponent<Rigidbody>().AddForce(rotor1.transform.up * speed);
            //rotor4.GetComponent<Rigidbody>().AddForce(rotor4.transform.up * speed);
        }

        if (Input.GetKey(KeyCode.Y))    
        {
            Debug.Log("右旋");
        }

    }

    

}

