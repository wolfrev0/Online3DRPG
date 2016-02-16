using UnityEngine;
using UnityEngine.UI;

public class BackTumblingView : MonoBehaviour
{
    public Image cover;
    
    void Update()
    {
        cover.fillAmount = Player.mine.backTumblingCoolTimeLeft / Player.mine.backTumblingCoolTime;
    }
}