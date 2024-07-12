using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q_Rotor1 : MonoBehaviour
{
    public float throttle;//转速
    public GameObject propeller;//螺旋桨
    public GameObject centerAxis;//螺旋桨旋转轴
    public GameObject stressedObjectForce;//受力对象
    public GameObject stressedObjectTorque;//受力对象

    public float Kf;//升力比例系数
    public float Km;//反扭矩比例系数

    public float upForce;
    public float counterTorque;

    //限制中心物体和螺旋桨的相对位置
    private Vector3 mOffset;

    // Start is called before the first frame update
    void Start()
    {
        //初始化两个比例系数
        Kf = 0.01f;
        Km = 0.01f;

        mOffset = centerAxis.transform.position - propeller.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //螺旋桨旋转
        propeller.transform.RotateAround(centerAxis.transform.position, centerAxis.transform.up, 100f * throttle * Time.deltaTime);
        //propeller.transform.position = centerAxis.transform.position + mOffset;//


        //添加升力，升力与转速的平方成正比
        upForce = Kf * throttle * throttle;
        stressedObjectForce.GetComponent<Rigidbody>().AddForce(stressedObjectForce.transform.up * upForce);

        //添加反扭矩，反扭矩的大小与转速的平方成正比
        counterTorque = Km * throttle * throttle;
        stressedObjectTorque.GetComponent<Rigidbody>().AddRelativeTorque(stressedObjectTorque.transform.up * counterTorque * (-1) * throttle / Mathf.Abs(throttle));
    }
}
