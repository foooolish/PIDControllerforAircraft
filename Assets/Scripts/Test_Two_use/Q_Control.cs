using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Q_Control : MonoBehaviour
{

    //public float throttle;//油门

    private GameObject rotor1;
    private GameObject rotor2;
    private GameObject rotor3;
    private GameObject rotor4;

    private Sensors mySensors;//自身的传感器

    /********************数据显示UI********************************/
    private Text currentThrottle_Text;
    private Text currentPitch_Text;
    private Text currentRoll_Text;
    private Text currentYaw_Text;
    private Text currentPosition_Text;

    private Text setThrottle_Text;
    private Text setPitch_Text;
    private Text setRoll_Text;
    private Text setYaw_Text;
    private Text setPosition_Text;
    /********************End Init********************************/

    /********************实现悬浮的PID算法参数********************************/
    private float setHigh = 3f;
    private float error;
    private float previous01_error;
    public float minThrrotle = 10;
    public  float maxThrrotle = 30;
    private float Kp = 5;
    private float Kd = 100;
    /********************实现悬浮的PID算法参数********************************/

    /********************实现欧拉角调节的PID算法参数********************************/
    private float[] errorEuler;
    private float[] previous01_errorEuler;
    private float[] limitSpeedEuler;
    private float[] KpEuler;
    private float[] KdEuler;

    private float setPitch;
    private float setRoll;
    public float setYaw;

    /********************实现欧拉角调节的PID算法参数********************************/


    /********************定义四个PID控制器********************************/
    private PID throttleControl;

    private PID pitchControl;
    private PID rollControl;
    private PID yawControl;
    


    // Start is called before the first frame update
    void Start()
    {
        //获取四个电机
        rotor1 = GameObject.Find("Rotor1");
        rotor2 = GameObject.Find("Rotor2");
        rotor3 = GameObject.Find("Rotor3");
        rotor4 = GameObject.Find("Rotor4");

        mySensors = GetComponent<Sensors>();

        setHigh = this.transform.position.y;

        /********************实现欧拉角调节的PID算法参数的初始化********************************/
        errorEuler = new float[3];
        previous01_errorEuler = new float[3];
        limitSpeedEuler = new float[3];
        limitSpeedEuler[0] = 0.1f;
        limitSpeedEuler[1] = 0.1f;
        limitSpeedEuler[2] = 0.1f;

        KpEuler = new float[3];
        KpEuler[0] = 0.1f;
        KpEuler[1] = 0.1f;
        KpEuler[2] = 2f;

        KdEuler = new float[3];
        KdEuler[0] = 5f;
        KdEuler[1] = 5f;
        KdEuler[2] = 100f;

        //初始化欧拉角
        setPitch = 0;
        setRoll = 0;
        setYaw = 0;
        /********************End Init********************************/

        /********************实例化四个PID控制器*************************/
        throttleControl = new PID(5, 0, 100, 1, 30);//Kp,Ki,Kd,minLimit,MaxLimit
        pitchControl = new PID(0.1f, 0, 5, -1, 1);
        rollControl = new PID(0.1f, 0, 5, -1, 1);
        yawControl = new PID(2, 0, 10, -10, 10);

        /********************显示UI实例化*************************/
        currentThrottle_Text = GameObject.Find("CurrentThrottle").GetComponent<Text>();
        currentPitch_Text = GameObject.Find("CurrentPitch").GetComponent<Text>();
        currentRoll_Text = GameObject.Find("CurrentRoll").GetComponent<Text>();
        currentYaw_Text = GameObject.Find("CurrentYaw").GetComponent<Text>();
        currentPosition_Text = GameObject.Find("CurrentPosition").GetComponent<Text>();

        setThrottle_Text = GameObject.Find("SetThrottle").GetComponent<Text>();
        setPitch_Text = GameObject.Find("SetPitch").GetComponent<Text>();
        setRoll_Text = GameObject.Find("SetRoll").GetComponent<Text>();
        setYaw_Text = GameObject.Find("SetYaw").GetComponent<Text>();
        setPosition_Text = GameObject.Find("SetPosition").GetComponent<Text>();
        /********************End*************************/
    }

    // Update is called once per frame
    void Update()
    {

        this.QControl();
        //按键检测
        this.KeyScan();

        //显示重心位置
        //this.GetComponent<Rigidbody>().ResetCenterOfMass();
        //Debug.Log("重心位置：" + this.GetComponent<Rigidbody>().centerOfMass);
        
    }

    private void QControl()
    {
        //油门控制
        error = setHigh + 5f - transform.position.y; //计算得到偏差，5f是稳态误差
        float Thrrotle = Kp * error + Kd * (error - previous01_error);//位置式PID控制器
        previous01_error = error;
        Thrrotle = Mathf.Clamp(Thrrotle, minThrrotle, maxThrrotle);
        //显示油门数据
        currentThrottle_Text.text = "油门：" +System.Math.Round(Thrrotle,2);


        //如果俯仰角不为零，则进行PID调节
        float pitch = mySensors.transform.eulerAngles.x;
        if(pitch>180)
        {
            pitch = pitch - 360;//这步很重要，（这是在单步执行过程中发现数据不对）
        }

        currentPitch_Text.text = "俯仰角：" + System.Math.Round(pitch,5);
        //Debug.Log("pitch：" + pitch);
        float pitch_cal=0;
        errorEuler[0] = setPitch - pitch;
        pitch_cal = KpEuler[0] * errorEuler[0] + KdEuler[0] * (errorEuler[0] - previous01_errorEuler[0]);
        previous01_errorEuler[0] = errorEuler[0];
        pitch_cal = Mathf.Clamp(pitch_cal, -1f, 1f);//限制pitch_cal的范围

        Debug.Log("pitch_cal：" + pitch_cal);


        ////如果侧倾角不为零，则进行PID调节
        float roll = mySensors.transform.eulerAngles.z;
        if (roll > 180)
        {
            roll = roll - 360;
        }
        float roll_cal = 0;
        currentRoll_Text.text = "横滚角：" + System.Math.Round(roll,5);
        errorEuler[1] = setRoll - roll;
        roll_cal = KpEuler[1] * errorEuler[1] + KdEuler[1] * (errorEuler[1] - previous01_errorEuler[1]);
        previous01_errorEuler[1] = errorEuler[1];
        roll_cal = Mathf.Clamp(roll_cal, -1f, 1f);
        Debug.Log("roll_cal：" + roll_cal);

        //如果偏航角不为零，则进行PID调节
        float yaw = mySensors.transform.localEulerAngles.y;
        //Debug.Log("yaw：" + yaw);
        if (yaw > 180)
        {
            yaw = yaw - 360;
        }
        currentYaw_Text.text = "偏航角：" + System.Math.Round(yaw,5);
        //Debug.Log("yaw：" + yaw);
        //float yaw_cal = 0;
        //errorEuler[2] = setYaw - yaw;
        //yaw_cal = KpEuler[2] * errorEuler[2] + KdEuler[2] * (errorEuler[2] - previous01_errorEuler[2]);
        //previous01_errorEuler[2] = errorEuler[2];
        //yaw_cal = Mathf.Clamp(yaw_cal, -3, 3);//0代表不产生反扭矩，


        //Debug.Log("yaw_cal：" + yaw_cal);
        transform.Rotate(transform.up, setYaw - yaw);


        //当前位置
        currentPosition_Text.text = "高度（全局）：" + System.Math.Round(transform.position.y,2);


        //将几个计算后再融合加载

        rotor1.GetComponent<Q_Rotor>().throttle = Thrrotle - pitch_cal + roll_cal;//明天再融合油门和偏航角，这两个都要PID计算
        rotor2.GetComponent<Q_Rotor>().throttle = -(Thrrotle - pitch_cal - roll_cal);
        rotor3.GetComponent<Q_Rotor>().throttle = Thrrotle + pitch_cal - roll_cal;
        rotor4.GetComponent<Q_Rotor>().throttle = -(Thrrotle + pitch_cal + roll_cal);
    }
    //实现欧拉角纠正

    private void KeyScan()
    {
        //设置高度大小
        if (Input.GetKeyDown(KeyCode.W))
        {
            setHigh++;
            //Debug.Log("setHigh：" + setHigh);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Debug.Log("setHigh：" + setHigh);
            setHigh--;
        }
        setPosition_Text.text = "高度（全局）：" + setHigh;

        //设置俯仰角最大最小值
        float pitchLimit = 30;
        //向前向后
        setPitch = Input.GetAxis("Vertical") * pitchLimit;
        setPitch_Text.text = "俯仰角：" + setPitch;

        //设置横滚角最大最小值
        float rollLimit = 30;
        //向左向右
        setRoll = -Input.GetAxis("Horizontal") * rollLimit;
        setRoll_Text.text = "横滚角：" + setRoll;

        //左旋
        if (Input.GetKeyDown(KeyCode.Z))
        {
            setYaw--;
            //Debug.Log("setYaw：" + setYaw);
        }

        //右旋
        if(Input.GetKeyDown(KeyCode.Y))
        {
            setYaw++;
            //Debug.Log("setYaw：" + setYaw);
        }

        setYaw_Text.text = "偏航角：" + setYaw;
    }

}

//PID 类，用位置式PID算法，float previous_error,float error,float Kp,float Kd,float Limit


