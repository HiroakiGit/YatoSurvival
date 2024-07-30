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
        //メソッドを登録
        button.onClick.AddListener(OnClickThisButton);
    }

    //マウスカーソル乗ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        SEAudio.Instance.PlayOneShot(choiceButtonSoundClip, 0.1f);
        btnImage.sprite = OnPointSprite;
    }

    //マウスカーソル離れたとき
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
