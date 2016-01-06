using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TeraTaleNet;

public class Player : AliveEntity
{
    static Dictionary<string, Player> _playersByName = new Dictionary<string, Player>();
    const float kRaycastDistance = 50.0f;

    public Text nameView;
    public SpeechBubble _speechBubble;

    NavMeshAgent _navMeshAgent;
    Animator _animator;

    static public Player FindPlayerByName(string name)
    {
        return _playersByName[name];
    }

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    new void Start()
    {
        base.Start();
        name = owner;
        nameView.text = name;
        _playersByName.Add(name, this);
        if (name == NetworkProgramUnity.currentInstance.userName)
            FindObjectOfType<CameraController>().target = transform;
    }

    public void HandleInput()
    {
        if (!isMine)
            return;

        if (Input.GetButtonDown("Move"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, kRaycastDistance))
            {
                Send(new Navigate(RPCType.All, hit.point.x, hit.point.y, hit.point.z));
            }
        }

        if (Input.GetButtonDown("Attack"))
        {
            Send(new Attack(RPCType.All));
        }
    }

    public void Attack(Attack info)
    {
        _animator.SetTrigger("Attacking");
    }

    void Die()
    {
        _animator.SetTrigger("Dying");
    }

    public bool IsArrived()
    {
        if (_navMeshAgent.enabled == false)
            return true;
        var toDestination = _navMeshAgent.destination - transform.position;
        return toDestination.magnitude <= _navMeshAgent.stoppingDistance;
    }

    void Navigate(Navigate info)
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.destination = new Vector3(info.x, info.y, info.z);
        _animator.SetBool("Running", true);
    }

    public void NavigateStop()
    {
        _navMeshAgent.enabled = false;
        _animator.SetBool("Running", false);
    }

    public void Speak(string chat)
    {
        _speechBubble.Show(chat);
    }

    public void SwitchWorld(string world)
    {
        if (isMine)
        {
            Debug.Log(NetworkProgramUnity.currentInstance.userName + "가 " + world + "로 이동합니다.");
            //TODO : 패킷을 보내서 SwitchWorld 시키기
        }
    }
}