using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button button;
    Image btnImage;

    public Sprite NormalSprite;
    public Sprite OnPointSprite;
    public AudioClip choiceButtonSoundClip;
    public AudioClip pushedButtonSoundClip;

    void Start()
    {
        button = GetComponent<Button>();
        btnImage = GetComponent<Image>();
        //���\�b�h��o�^
        button.onClick.AddListener(OnClickThisButton);
    }

    //�}�E�X�J�[�\��������Ƃ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        SEAudio.Instance.PlayOneShot(choiceButtonSoundClip, 0.1f);
        btnImage.sprite = OnPointSprite;
    }

    //�}�E�X�J�[�\�����ꂽ�Ƃ�
    public void OnPointerExit(PointerEventData eventData)
    {
        btnImage.sprite = NormalSprite;
    }

    public void OnClickThisButton()
    {
        SEAudio.Instance.PlayOneShot(pushedButtonSoundClip, 0.2f);
        Initalize();
    }

    public void Initalize()
    {
        btnImage.sprite = NormalSprite;
    }
}
