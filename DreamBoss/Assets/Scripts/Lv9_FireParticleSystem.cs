using UnityEngine;
using System;
using System.Linq;

public class Lv9_FireParticleSystem : MonoBehaviour
{
    // 五組火焰的數量
    private int[] count = { 80, 80, 80, 80, 80 };

    /// <summary>
    /// 水柱旋轉根物件
    /// </summary>
    private Transform traRoot;
    /// <summary>
    /// 消防車上的基底：消防管旋轉軸心
    /// </summary>
    private Transform traRootOnFiretruck;
    /// <summary>
    /// 是否能旋轉
    /// </summary>
    public bool canRotate;

    private void Awake()
    {
        traRoot = transform.root;
        traRootOnFiretruck = GameObject.Find("消防管旋轉軸心").transform;
    }

    private void Update()
    {
        RotateRoot();
    }

    /// <summary>
    /// 旋轉根物件：水柱旋轉根物件、消防管旋轉軸心
    /// </summary>
    private void RotateRoot()
    {
        if (!canRotate) return;                                             // 不能旋轉 就 跳出

        if (Input.GetKey(KeyCode.Mouse0))
        {
            float y = Input.GetAxis("Mouse Y");                                 // 取得 Y 軸的值

            traRoot.Rotate(0, 0, y * Time.deltaTime * -150);                    // 水柱旋轉根物件 旋轉
            traRootOnFiretruck.Rotate(0, 0, y * Time.deltaTime * -150);         // 消防管旋轉軸心 旋轉

            float z = traRoot.rotation.z;                                       // 取得 Z 軸
            z = Mathf.Clamp(z, -0.5f, 0.2f);                                    // 鎖定 Z 軸 範圍

            Quaternion qua = traRoot.rotation;                                  // 取得角度
            qua.z = z;                                                          // 更新 Z 軸

            traRoot.rotation = qua;                                             // 指定角度
            traRootOnFiretruck.rotation = qua;                                  // 指定角度
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        WaterOnFire(other);
    }

    /// <summary>
    /// 水柱在火焰上 - 滅火
    /// </summary>
    /// <param name="other">打到的物件</param>
    private void WaterOnFire(GameObject other)
    {
        if (other.name.Contains("火焰"))                                                // 如果名稱包含 火焰
        {
            int i = int.Parse(other.name[3].ToString());                                // 取得第四個字元 - 編號

            ParticleSystem ps = other.GetComponent<ParticleSystem>();                   // 取得粒子
            ParticleSystem.EmissionModule emission = ps.emission;                       // 取得噴射模組
            count[i]--;                                                                 // 數量遞減
            emission.rateOverTime = count[i];                                           // 更新數量

            if (count[i] == 0)                                                          // 如果數量為零
            {
                CheckAllFire();                                                         // 檢查是否全部都熄滅
                other.GetComponent<Collider>().enabled = false;                         // 關閉碰撞
                StartCoroutine(Lv9_Fireman.instance.Correct());                         // 正確音效
            }
        }
    }

    /// <summary>
    /// 檢查是否全部都熄滅
    /// </summary>
    private void CheckAllFire()
    {
        var countAll = count.Where(x => x == 0);
        
        if (countAll.ToList().Count == count.Length)
        {
            GetComponent<ParticleSystem>().Stop();

            StartCoroutine(Lv9_Fireman.instance.Pass());
        }
    }
}
