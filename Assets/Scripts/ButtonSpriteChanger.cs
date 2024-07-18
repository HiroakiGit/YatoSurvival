using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSpriteChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image btnImage;

    public Sprite NormalSprite;
    public Sprite OnPointSprite;

    //マウスカーソル乗ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("On");
        btnImage.sprite = OnPointSprite;
    }

    //マウスカーソル離れたとき
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Off");
        btnImage.sprite = NormalSprite;
    }

    public void Reset()
    {
        btnImage.sprite = NormalSprite;
    }
}
