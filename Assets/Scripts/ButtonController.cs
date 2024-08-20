using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button button;
    Image btnImage;

    private RectTransform rectTransform;
    public Sprite NormalSprite;
    public Sprite OnPointSprite;
    public GameObject ButtonBG;
    public bool isNormal = true;
    private Text buttonText;

    [Header("Audio")]
    public AudioClip choiceButtonSoundClip;
    public AudioClip pushedButtonSoundClip;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        btnImage = GetComponent<Image>();

        if (isNormal)
        {
            buttonText = transform.GetChild(0).GetComponent<Text>();
        }

        //���\�b�h��o�^
        button.onClick.AddListener(OnClickThisButton);

        Initalize();
    }

    //�}�E�X�J�[�\��������Ƃ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ButtonBG != null) 
        {
            ButtonBG.SetActive(true);
        }
        else
        {
            btnImage.sprite = OnPointSprite;
        }

        if (isNormal)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500);
            buttonText.color = Color.black;
        }
        
        SEAudio.Instance.PlayOneShot(choiceButtonSoundClip, 0.1f, true);
    }

    //�}�E�X�J�[�\�����ꂽ�Ƃ�
    public void OnPointerExit(PointerEventData eventData)
    {
        Initalize();
    }

    public void OnClickThisButton()
    {
        SEAudio.Instance.PlayOneShot(pushedButtonSoundClip, 0.2f, true);
        Initalize();
    }

    public void Initalize()
    {
        btnImage.sprite = NormalSprite;

        if(ButtonBG != null) ButtonBG.SetActive(false);

        if(isNormal)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            buttonText.color = Color.white;
        }
    }
}
