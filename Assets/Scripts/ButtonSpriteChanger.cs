using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSpriteChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image btnImage;

    public Sprite NormalSprite;
    public Sprite OnPointSprite;

    void Start()
    {
        // Imageを所得
        btnImage = this.GetComponent<Image>();
    }

    //マウスカーソル乗ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        btnImage.sprite = OnPointSprite;
    }

    //マウスカーソル離れたとき
    public void OnPointerExit(PointerEventData eventData)
    {
        btnImage.sprite = NormalSprite;
    }
}
