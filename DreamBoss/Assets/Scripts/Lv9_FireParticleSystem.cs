using UnityEngine;
using System;

public class Lv9_FireParticleSystem : MonoBehaviour
{
    // 五組火焰的數量
    private int[] count = { 80, 80, 80, 80, 80 };
    
    private void OnParticleCollision(GameObject other)
    {
        if (other.name.Contains("火焰"))
        {
            int i = int.Parse(other.name[3].ToString());

            ParticleSystem ps = other.GetComponent<ParticleSystem>();
            ParticleSystem.EmissionModule emission = ps.emission;
            count[i]--;
            emission.rateOverTime = count[i];

            if (count[i] == 0) other.GetComponent<Collider>().enabled = false;
        }
    }
}
