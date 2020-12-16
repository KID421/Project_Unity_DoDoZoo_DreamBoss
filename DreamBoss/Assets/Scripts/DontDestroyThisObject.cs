using UnityEngine;

public class DontDestroyThisObject : MonoBehaviour
{
    /// <summary>
    /// 靜態物件：只保存一開始進場景的此物件
    /// </summary>
    public static DontDestroyThisObject instance;

    private void Awake()
    {
        // 如果 靜態物件 是 空的，靜態物件 儲存 為 此物件
        if (!instance) instance = this;
        // 否則 就 刪除 此物件
        else Destroy(gameObject);
        // 載入場景時不刪除 靜態物件
        DontDestroyOnLoad(instance);
    }
}
