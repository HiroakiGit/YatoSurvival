using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button buttton;
    Image btnImage;

    public Sprite NormalSprite;
    public Sprite OnPointSprite;
    public AudioClip choiceButtonSoundClip;
    public AudioClip pushedButtonSoundClip;

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
        BGMAndSEAudio.Instance.PlayOneShot(choiceButtonSoundClip, 0.2f);
        btnImage.sprite = OnPointSprite;
    }

    //�}�E�X�J�[�\�����ꂽ�Ƃ�
    public void OnPointerExit(PointerEventData eventData)
    {
        btnImage.sprite = NormalSprite;
    }

    public void OnClickThisButton()
    {
        BGMAndSEAudio.Instance.PlayOneShot(pushedButtonSoundClip, 0.5f);
        Reset();
    }

    public void Reset()
    {
        btnImage.sprite = NormalSprite;
    }
}
