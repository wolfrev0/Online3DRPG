using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        if (name == userName)
            FindObjectOfType<CameraController>().target = transform;
    }

    new void OnDestroy()
    {
        base.OnDestroy();
        _playersByName.Remove(name);
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
                Send(new Navigate(hit.point));
            }
        }

        if (Input.GetButtonDown("Attack"))
        {
            Send(new Attack());
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
        _navMeshAgent.destination = info.destination;
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
            Destroy();
            Send(new SwitchWorld(userName, world));
            SceneManager.LoadScene(world);
            var programInst = NetworkProgramUnity.currentInstance;
            programInst.NetworkInstantiate(programInst.pfPlayer);
        }
    }
}