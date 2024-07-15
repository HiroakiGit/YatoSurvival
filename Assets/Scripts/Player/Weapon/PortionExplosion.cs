using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionExplosion : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        //アニメーション終了時このオブジェを消す
        Destroy(this.transform.parent.gameObject);
    }
}
