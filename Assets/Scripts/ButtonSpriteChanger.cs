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

    //�}�E�X�J�[�\��������Ƃ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("On");
        btnImage.sprite = OnPointSprite;
    }

    //�}�E�X�J�[�\�����ꂽ�Ƃ�
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
