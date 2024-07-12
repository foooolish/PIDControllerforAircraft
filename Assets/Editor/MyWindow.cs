using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;


public class MyWindow : EditorWindow
{
    private static MyWindow window;//窗体实例


    //private Sensors1 mSensors1;

    //显示窗体
    [MenuItem("MyWindow/Second Window")]
    private static void ShowWindow()
    {
        window = EditorWindow.GetWindow<MyWindow>("Window Example");
        window.Show();
    }

    //显示时调用
    private void OnEnable()
    {
        Debug.Log("OnEnable");



    }

    //绘制窗体内容
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Your Second Window", EditorStyles.boldLabel);
    }

    //固定帧数调用
    private void Update()
    {
        Debug.Log("Update");

        //Debug.Log("窗体测试：" + mSensors1.mGyroscope.getAngularVelocityX());
    }

    //隐藏时调用
    private void OnDisable()
    {
        Debug.Log("OnDisable");
    }

    //销毁时调用
    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
    }

}
