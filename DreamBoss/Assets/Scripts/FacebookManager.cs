using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System.Collections.Generic;
using System;

public class FacebookManager : MonoBehaviour
{
    public static FacebookManager instance;

    /// <summary>
    /// FB 按鈕
    /// </summary>
    private Button btnFacebook;

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        btnFacebook = GameObject.Find("Facebook 登錄").GetComponent<Button>();
        btnFacebook.onClick.AddListener(Login);

        Initialize();
    }

    /// <summary>
    /// 初始化 FB
    /// </summary>
    private void Initialize()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    /// <summary>
    /// 初始後的回呼
    /// </summary>
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
    }

    /// <summary>
    /// 登入功能
    /// </summary>
    private void Login()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, LoginCallback);
    }

    /// <summary>
    /// 登入回呼
    /// </summary>
    /// <param name="result"></param>
    private void LoginCallback(ILoginResult result)
    {
        print("登入成功");
    }

    public void ShareFB()
    {
        FB.ShareLink
        (
            new Uri("https://www.facebook.com/ALICEMISA/")
        );
    }
}
