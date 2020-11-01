using UnityEngine;
using UnityEngine.UI;

public class ButtomCustomArea : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
    }
}
