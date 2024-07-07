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
        // Image������
        btnImage = this.GetComponent<Image>();
    }

    //�}�E�X�J�[�\��������Ƃ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        btnImage.sprite = OnPointSprite;
    }

    //�}�E�X�J�[�\�����ꂽ�Ƃ�
    public void OnPointerExit(PointerEventData eventData)
    {
        btnImage.sprite = NormalSprite;
    }
}
