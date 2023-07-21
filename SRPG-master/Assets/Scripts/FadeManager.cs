using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Image = UnityEngine.UI.Image;

public class FadeManager : MonoBehaviour
{
    //�V���O���g��
    static public FadeManager _instance;

    [SerializeField]
    private Image _image;

    private void Awake()
    {
        if( _instance == null )
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    //�V���O���g���I��� 

    /// <summary>
    /// �t�F�[�h�C��
    /// </summary>
    public void FadeIn()
    {
        _image.DOFade(0, 2f);
    }

    /// <summary>
    /// �t�F�[�h�A�E�g
    /// </summary>
    public void FadeOut()
    {
        _image.DOFade(1,2f);
    }


    public void FadeOutToIn(TweenCallback action = null)
    {
        _image.DOFade(1, 2f).OnComplete(() => { action(); FadeIn(); });
    }


}
