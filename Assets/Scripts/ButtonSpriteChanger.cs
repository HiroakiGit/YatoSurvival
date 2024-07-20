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
        //���\�b�h��o�^
        buttton.onClick.AddListener(OnClickThisButton);
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

    public void OnClickThisButton()
    {
        Reset();
    }

    public void Reset()
    {
        btnImage.sprite = NormalSprite;
    }
}
