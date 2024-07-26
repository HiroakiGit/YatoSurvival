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
        //メソッドを登録
        buttton.onClick.AddListener(OnClickThisButton);
    }

    //マウスカーソル乗ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        BGMAndSEAudio.Instance.PlayOneShot(choiceButtonSoundClip, 0.2f);
        btnImage.sprite = OnPointSprite;
    }

    //マウスカーソル離れたとき
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
