using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor : MonoBehaviour
{
    
    private Rigidbody m_Rigidbody;
    //PID算法参数
    private float setHigh = 10;
    private float error;
    private float previous01_error;
    private float limitForce = 10;
    private float Kp = 5;
    private float Kd = 1000;

    // Start is called before the first frame update

    public GameObject rotor;
    public GameObject centerPoint;
    public float speed = 10;
    void Start()
    {
        m_Rigidbody = gameObject.GetComponent<Rigidbody>(); //获取本物体的刚体组件
        //setHigh = m_Rigidbody.transform.position.y;//初始化高度
    }
    // Update is called once per frame
    void Update()
    {
        //保持平衡，实现悬浮  
        /*********************************************PID算法********************************************/
        //设置高度大小
        if (Input.GetKeyDown(KeyCode.W))
        {
            setHigh++;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            setHigh--;
        }
        /***********位置式PID算法实现，确切说是PD算法***********/
        error = setHigh + - m_Rigidbody.transform.position.y; //计算得到偏差，2.22f是稳态误差
        float upForce = Kp * error + Kd * (error - previous01_error);//位置式PID控制器
        previous01_error = error;

        if (upForce > limitForce)
        {
            upForce = limitForce;
        }

        if (upForce < 0)
        {
            upForce = 0;//飞行器的升力不可以是负数，最小只能为0
        }
        Debug.Log("upForce：" + upForce);
        m_Rigidbody.AddForce(m_Rigidbody.transform.up* upForce);
        //Debug.Log("设置高度：" + setHigh);
        //Debug.Log("当前高度：" + m_Rigidbody.GetComponent<Rigidbody>().transform.position.y);
        /**********************************************************************************************************/

        //螺旋桨旋转实现
        rotor.transform.RotateAround(centerPoint.transform.position, centerPoint.transform.up, 100f* speed * Time.deltaTime);

        //添加反扭矩，大小与转数的平方成正比
        //m_Rigidbody.AddTorque(transform.up * direction);
    }
}
