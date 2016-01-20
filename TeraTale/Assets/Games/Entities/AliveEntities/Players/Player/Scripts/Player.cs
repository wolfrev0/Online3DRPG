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
    public Camera playerRenderCamera;

    NavMeshAgent _navMeshAgent;
    Animator _animator;
    List<ItemStack> _itemStacks = new List<ItemStack>(30);
    Weapon _weapon;
    ItemSolid _weaponSolid;
    //Rename Attacker to AttackSubject??
    Attacker _attacker;
    //StreamingSkill (Base Attack) Management
    float _attackStackTimer = 0;
    int _attackStack = 0;
    
    public List<ItemStack> itemStacks
    {
        get { return _itemStacks; }
    }

    static Player _mine;
    static public Player mine
    {
        get
        {
            if (_mine == null)
                _mine = FindPlayerByName(userName);
            return _mine;
        }
    }

    static public Player FindPlayerByName(string name)
    {
        try
        { return _playersByName[name]; }
        catch (KeyNotFoundException)
        { return null; }
    }

    public Weapon weapon
    {
        get { return _weapon; }

        private set
        {
            _weapon = value;
            if (isServer)
            {
                NetworkDestroy(_weaponSolid);
                NetworkInstantiate(_weapon.solidPrefab.GetComponent<NetworkScript>(), _weapon, "OnWeaponInstantiate");
            }
        }
    }

    void OnWeaponInstantiate(ItemSolid itemSolid)
    {
        _weaponSolid = itemSolid;
        _weaponSolid.transform.parent = rightHand;
        _weaponSolid.transform.localPosition = Vector3.zero;
        _weaponSolid.transform.localRotation = Quaternion.identity;
        _weaponSolid.transform.localScale = Vector3.one;
        _weaponSolid.GetComponent<Floater>().enabled = false;
        _weaponSolid.GetComponent<ItemSpawnEffector>().enabled = false;
        //Should Make AttackerNULL and AttackerImpl for ProjectileWeapon
        _attacker = _weaponSolid.GetComponent<Attacker>();
        _attacker.enabled = false;
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
        if (isMine)
        {
            FindObjectOfType<CameraController>().target = transform;
            GameObject.FindWithTag("PlayerStatusView").GetComponent<StatusView>().target = this;
            playerRenderCamera.gameObject.SetActive(true);
        }
        Equip(new WeaponNull());
    }

    void Update()
    {
        _attackStackTimer -= Time.deltaTime;
        if (_attackStackTimer < 0)
            _attackStack = 0;
    }

    new void OnDestroy()
    {
        base.OnDestroy();
        _playersByName.Remove(name);
    }

    void AttackBegin()
    {
        _attacker.enabled = true;
    }

    void AttackEnd()
    {
        _attacker.enabled = false;
    }

    public void HandleInput()
    {
        if (!isMine)
            return;

        if (Input.GetButtonDown("Move"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, kRaycastDistance, 1 << LayerMask.NameToLayer("Terrain")))
                Send(new Navigate(hit.point));
        }

        if (Input.GetButtonDown("Attack"))
            Send(new Attack());
    }

    public void Attack(Attack info)
    {
        _animator.SetTrigger("Attack");
        _animator.SetInteger("WeaponType", (int)_weapon.weaponType);
        _animator.SetInteger("BaseAttackStack", _attackStack++);
        _attackStackTimer = 3;

        //Stack Overflow
        if (_attackStack > 2)
        {
            _attackStack = 0;
            _attackStackTimer = 0;
        }
    }

    protected override void Die()
    {
        //_animator.SetTrigger("Die");
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
        _animator.SetBool("Run", true);
    }

    public void NavigateStop()
    {
        _navMeshAgent.enabled = false;
        _animator.SetBool("Run", false);
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
        Send(new Equip(equipment));
    }

    public void Equip(Equip rpc)
    {
        var equipment = (Equipment)rpc.equipment.body;
        switch (equipment.equipmentType)
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
        switch (equipment.equipmentType)
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
                return weapon == equipment;
        }
        return false;
    }
}