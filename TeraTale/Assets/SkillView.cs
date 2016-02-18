using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    public Image cover;

    void Update()
    {
        if (Player.mine)
            cover.fillAmount = Player.mine.skillCoolTimeLeft / Player.mine.skillCoolTime;
    }
}