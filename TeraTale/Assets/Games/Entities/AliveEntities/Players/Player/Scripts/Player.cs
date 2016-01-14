using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TeraTaleNet;

public class Player : AliveEntity
{
    static Dictionary<string, Player> _playersByName = new Dictionary<string, Player>();
    const float kRaycastDistance = 50.0f;

    public Transform rightHand;
    public Text nameView;
    public SpeechBubble speechBubble;

    NavMeshAgent _navMeshAgent;
    Animator _animator;
    List<ItemStack> _itemStacks = new List<ItemStack>(30);
    Weapon _weapon;
    ItemSolid _weaponSolid;
    
    public List<ItemStack> itemStacks
    {
        get { return _itemStacks; }
    }

    static public Player FindPlayerByName(string name)
    {
        return _playersByName[name];
    }

    public Weapon weapon
    {
        get { return _weapon; }

        private set
        {
            NetworkDestroy(_weaponSolid);
            _weapon = value;
            NetworkInstantiate(_weapon.solidPrefab.GetComponent<NetworkScript>(), "OnWeaponInstantiate");
        }
    }

    void OnWeaponInstantiate(ItemSolid itemSolid)
    {
        _weaponSolid = itemSolid;
        _weaponSolid.transform.parent = rightHand;
        _weaponSolid.transform.localPosition = Vector3.zero;
        _weaponSolid.transform.localRotation = Quaternion.identity;
        _weaponSolid.transform.localScale = Vector3.one;
    }

    void Awake()
    {
        for (int i = 0; i < 30; i++)
            _itemStacks.Add(new ItemStack());
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
        Sync("transform.position");
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
                Send(new Navigate(hit.point));
        }

        if (Input.GetButtonDown("Attack"))
            Send(new Attack());
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
        speechBubble.Show(chat);
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

    public void AddItem(Item item)
    {
        _itemStacks.Find((ItemStack s) => { return s.IsPushable(item); }).Push(item);
        Send(new AddItem(name, item));
    }

    public void AddItem(AddItem rpc)
    {
        Item item = (Item)rpc.item.body;
        _itemStacks.Find((ItemStack s) => { return s.IsPushable(item); }).Push(item);
    }

    public void Equip(Equipment equipment)
    {
        switch(equipment.type)
        {
            case Equipment.Type.Coat:
                break;
            case Equipment.Type.Gloves:
                break;
            case Equipment.Type.Hat:
                break;
            case Equipment.Type.Pants:
                break;
            case Equipment.Type.Shoes:
                break;
            case Equipment.Type.Weapon:
                weapon = (Weapon)equipment;
                break;
        }
    }

    public bool IsEquiping(Equipment equipment)
    {
        switch (equipment.type)
        {
            case Equipment.Type.Coat:
                break;
            case Equipment.Type.Gloves:
                break;
            case Equipment.Type.Hat:
                break;
            case Equipment.Type.Pants:
                break;
            case Equipment.Type.Shoes:
                break;
            case Equipment.Type.Weapon:
                return weapon == (Weapon)equipment;
        }
        return false;
    }
}