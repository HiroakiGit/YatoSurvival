using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionExplosion : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        //�A�j���[�V�����I�������̃I�u�W�F������
        Destroy(this.transform.parent.gameObject);
    }
}
