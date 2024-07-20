using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSpriteChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button buttton;
    Image btnImage;

    public Sprite NormalSprite;
    public Sprite OnPointSprite;

    void Start()
    {
        buttton = GetComponent<Button>();
        btnImage = GetComponent<Image>();
        //メソッドを登録
        buttton.onClick.AddListener(OnClickThisButton);
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

    public void OnClickThisButton()
    {
        Reset();
    }

    public void Reset()
    {
        btnImage.sprite = NormalSprite;
    }
}
