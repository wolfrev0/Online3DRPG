using UnityEngine;
using UnityEngine.UI;

public class BackTumblingView : MonoBehaviour
{
    public Image cover;
    
    void Update()
    {
        if (Player.mine)
            cover.fillAmount = Player.mine.backTumblingCoolTimeLeft / Player.mine.backTumblingCoolTime;
    }
}