using UnityEngine;
using System.Collections;
using System;
using sunho;

public class Dragon : BossEnemy
{
    public GameObject Fire;
    public ParticleSystem[] _breath;
    public GameObject MagicCircle;
    float lerpTime = 1f;
    float currentLerpTime;

    private IEnumerator KnonckBack()
    {
        {
            yield return new WaitForSeconds(1);
          
            Vector3 pos = _playerpos + transform.forward * 0.15f;

            float perc = currentLerpTime / lerpTime;
            // perc = Mathf.Sin(perc * Mathf.PI * 0.5f);
            perc = 1f - Mathf.Cos(perc * Mathf.PI * 0.5f);

            if (_player)
                _player.transform.localPosition = Vector3.Lerp(_playerpos, pos, perc);
        }

    }

    new void Update()
    {
        base.Update();

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        /*if(Time.time - _fDelay >= 2.0f)
        {
            OnThirdPatten();

            _fDelay = Time.time;
        }*/
        if (_distance.magnitude <= 3)
        {
            OnSecondPatten();
        }
    }

    protected override void OnNavi(Vector3 pos)
    {
        _nav.enabled = true;
        _nav.destination = pos;
        _animator.SetBool("Attack", false);
    }

    protected override void OffNavi()
    {
        _nav.enabled = false;
        _animator.SetBool("Attack", true);
    }

    protected override void Stay()
    {
        _animator.SetBool("Attack", false);
        OnNavi(_prePos);  
    }
    #region EventFunc
    void OnBreath()
    {
        Fire.SetActive(true);
        for (int i = 0; i < 2; i++)
        {
            _breath[i].Play();
        }
        AudioSource _audio = GetComponentInChildren<AudioSource>();
        _audio.Play();
    }

    void OffBreath()
    {
        Fire.SetActive(false);
    }
    #endregion

    protected override void OnFirstPatten()
    {
        _animator.SetBool("Attack",true);
    }

    protected override void OnSecondPatten()
    {
        StartCoroutine(KnonckBack());
    }

    protected override void OnThirdPatten()
    {
        GameObject obj = (GameObject)Instantiate(MagicCircle);
        obj.transform.localPosition = _player.transform.localPosition;
        obj.transform.localScale = Vector3.one *2;
    }

    protected override void Die()
    {
        throw new NotImplementedException();
    }
}
