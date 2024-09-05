using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineChangeAdaptor : MonoBehaviour
{
    [Header("JoyStick")]
    public FloatingJoystick inputMove; //左画面JoyStick
    public FloatingJoystick inputRotate; //右画面JoyStick
    public float moveSpeed = 5.0f; //移動する速度
    public float rotateSpeed = 5.0f;  //回転する速度
    [Header("UI")]
    public GameObject PCNaviCanvas;
    public GameObject PhoneJoyStickCanvas;

    public void Initalize()
    {
#if UNITY_STANDALONE
        PCNaviCanvas.SetActive(true);
        PhoneJoyStickCanvas.SetActive(false);
#endif
#if UNITY_ANDROID || UNITY_IOS
        PCNaviCanvas.SetActive(false);
        PhoneJoyStickCanvas.SetActive(true);
#endif
    }
}
