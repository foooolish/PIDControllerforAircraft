using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Q_Control1 : MonoBehaviour
{

    //public float throttle;//油门

    public GameObject rotor1;
    public GameObject rotor2;
    public GameObject rotor3;
    public GameObject rotor4;

    private Sensors1 mySensors;//自身的传感器

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
    public float minThrrotle = 10;
    public  float maxThrrotle = 30;

    /********************实现欧拉角调节的PID算法参数********************************/
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
        //rotor1 = GameObject.Find("Rotor1");
        //rotor2 = GameObject.Find("Rotor2");
        //rotor3 = GameObject.Find("Rotor3");
        //rotor4 = GameObject.Find("Rotor4");

        mySensors = GetComponent<Sensors1>();

        setHigh = this.transform.position.y;

        /********************实现欧拉角调节的PID算法参数的初始化********************************/
        //初始化欧拉角
        setPitch = 0;
        setRoll = 0;
        setYaw = 0;
        /********************实例化四个PID控制器*************************/
        throttleControl = new PID(5, 0, 100, minThrrotle, maxThrrotle);//Kp,Ki,Kd,minLimit,MaxLimit
        pitchControl = new PID(0.05f, 0, 30, -1, 1);
        rollControl = new PID(0.05f, 0, 30, -1, 1);
        yawControl = new PID(0.1f, 0, 10, -1, 1);
        /********************End Init********************************/

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
        throttleControl.RenewCalculData(setHigh + 5f, transform.position.y);//5是稳态误差
        float Thrrotle = throttleControl.Calcul();
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
        pitchControl.RenewCalculData(setPitch, pitch);
        pitch_cal = pitchControl.Calcul();
        Debug.Log("pitch_cal：" + pitch_cal);


        //如果侧倾角不为零，则进行PID调节
        float roll = mySensors.transform.eulerAngles.z;
        if (roll > 180)
        {
            roll = roll - 360;
        }
        float roll_cal = 0;
        rollControl.RenewCalculData(setRoll, roll);
        roll_cal = rollControl.Calcul();
        currentRoll_Text.text = "横滚角：" + System.Math.Round(roll,5);
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
        float yaw_cal = 0;
        //偏航角要在俯仰角和横滚角平衡后调，或者pitch，roll在一定稳态范围才开始调节，或者以上两者结合
        if ((pitch > -5f && pitch < 5f) && (roll > -5f && roll < 5f))
        {
            yawControl.RenewCalculData(setYaw, yaw);
            yaw_cal = yawControl.Calcul();
        }

        //当前位置
        currentPosition_Text.text = "高度（全局）：" + System.Math.Round(transform.position.y, 2);


        //将几个计算后再融合加载
        rotor1.GetComponent<Q_Rotor1>().throttle = Thrrotle - pitch_cal + roll_cal- yaw_cal;//明天再融合油门和偏航角，这两个都要PID计算，搞不懂
        rotor2.GetComponent<Q_Rotor1>().throttle = -(Thrrotle - pitch_cal - roll_cal + yaw_cal);
        rotor3.GetComponent<Q_Rotor1>().throttle = Thrrotle + pitch_cal - roll_cal - yaw_cal;
        rotor4.GetComponent<Q_Rotor1>().throttle = -(Thrrotle + pitch_cal + roll_cal + yaw_cal);
    }

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
        setPosition_Text.text = "高度（全局）：" + (setHigh + 2.2f);

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



//卡尔曼滤波器
public class CalManFilter
{
    //基本参数
    public float LastP;
    public float NowP;
    public float Output;
    public float Kg;
    public float Q;
    public float R;


    //构造方法
    public CalManFilter(float LastP, float NowP, float Kg, float Q, float R)
    {
        this.LastP = LastP;
        this.NowP = NowP;
        this.Kg = Kg;
        this.Q = Q;
        this.R = R;
    }

    public float CalOut(float Input)
    {
        NowP = LastP + Q;
        Kg = NowP / (NowP + R);
        Output = Output + Kg * (Input - Output);
        LastP = (1 - Kg) * NowP;
        return Output;
    }

}

//PID计算器
public class PID
{
    private float setPoint;
    private float measuredPoint;

    private float integral;//积分器

    private float previous_error;
    private float now_error;

    private float Kp;
    private float Ki;
    private float Kd;
    private float minLimit;
    private float maxLimit;

    //构造方法，用来初始化对象
    public PID(float Kp, float Ki, float Kd, float minLimit, float maxLimit)
    {
        //将初始化的数据公开到对象上，以便各个方法使用
        //this.setPoint = setPoint;
        //this.measuredPoint = measuredPoint;
        this.Kp = Kp;
        this.Ki = Ki;
        this.Kd = Kd;
        this.minLimit = minLimit;
        this.maxLimit = maxLimit;
    }

    public void RenewCalculData(float setPoint, float measuredPoint)
    {
        this.setPoint = setPoint;
        this.measuredPoint = measuredPoint;
    }

    //位置式PID算法核心计算
    public float Calcul()
    {
        /***********位置式PID算法实现，确切说是PD算法***********/
        now_error = setPoint - measuredPoint; //计算得到偏差，2.22f是稳态误差

        integral += now_error;

        float Output = Kp * now_error + Ki * integral + Kd * (now_error - previous_error);//位置式PID控制器
        Output = Mathf.Clamp(Output, this.minLimit, this.maxLimit);

        previous_error = now_error;

        return Output;
    }

}