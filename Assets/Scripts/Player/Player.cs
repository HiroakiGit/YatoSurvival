using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerController _PlayerController;
    public PlayerAttack _PlayerAttack;
    public PlayerHealth _PlayerHealth;
    public PlayerExperience _PlayerExperience;
    public PlayerAttackIndicator _PlayerAttackIndicator;
    public PlayerAnimation _PlayerAnimation;
    public string SUserName;
    public string SPassWord;
    public string SDisplayName;
    public Transform playerTransform;
    public Transform PlayerHaveObjectsParent;
}
