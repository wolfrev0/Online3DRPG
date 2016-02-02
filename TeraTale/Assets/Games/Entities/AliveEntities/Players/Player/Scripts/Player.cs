using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TeraTaleNet;
using System;
using System.Reflection;

public class Player : AliveEntity, IAutoSerializable
{
    static Dictionary<string, Player> _playersByName = new Dictionary<string, Player>();
    const float kRaycastDistance = 50.0f;

    public Text nameView;
    public SpeechBubble speechBubble;

    NavMeshAgent _navMeshAgent;
    Animator _animator;
    public ItemStackList _itemStacks = new ItemStackList(30);
    public Weapon _weapon = new WeaponNull();
    ItemSolid _weaponSolid;
    //Rename Attacker to AttackSubject??
    AttackSubject _attackSubject;
    //StreamingSkill (Base Attack) Management
    Projectile _pfArrow;
    
    public ItemStackList itemStacks
    {
        get { return _itemStacks; }
        private set { _itemStacks = value; }
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

    public bool isArrived { get { return Vector3.Distance(_navMeshAgent.destination, _navMeshAgent.transform.position) <= _navMeshAgent.stoppingDistance; } }

    void OnWeaponInstantiate(ItemSolid itemSolid)
    {
        _weaponSolid = itemSolid;
        _weapon = (Weapon)_weaponSolid.item;
        if (_weapon.weaponType == Weapon.Type.bow)
            _weaponSolid.transform.parent = _animator.GetBoneTransform(HumanBodyBones.LeftHand);
        else
            _weaponSolid.transform.parent = _animator.GetBoneTransform(HumanBodyBones.RightHand);
        _weaponSolid.transform.localPosition = Vector3.zero;
        _weaponSolid.transform.localRotation = Quaternion.identity;
        _weaponSolid.transform.localScale = Vector3.one;
        _weaponSolid.enabled = false;
        _weaponSolid.GetComponent<Floater>().enabled = false;
        _weaponSolid.GetComponent<ItemSpawnEffector>().enabled = false;
        //Should Make AttackerNULL and AttackerImpl for ProjectileWeapon
        _attackSubject = _weaponSolid.GetComponent<AttackSubject>();
        _attackSubject.enabled = false;
        _attackSubject.owner = this;
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
        }
        transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
        _pfArrow = Resources.Load<Projectile>("Prefabs/Arrow");

        if (isServer)
        {
            GameServer.currentInstance.QuerySerializedPlayer(name);
        }
    }

    protected override void OnNetworkDestroy()
    {
        if (isServer)
            GameServer.currentInstance.SavePlayer(this);
        base.OnNetworkDestroy();
        _playersByName.Remove(name);
    }

    void AttackBegin()
    {
        _attackSubject.enabled = true;
    }

    void AttackEnd()
    {
        _attackSubject.enabled = false;
    }

    public void SetKnockdown(bool value)
    {
        _attackSubject.knockdown = value;
    }

    void Shot()
    {
        var projectile = Instantiate(_pfArrow);
        projectile.transform.position = transform.position + Vector3.up;
        projectile.direction = transform.forward;
        projectile.speed = 10;
        projectile.autoDestroyTime = 0.5f;
        projectile.GetComponent<AttackSubject>().owner = this;
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
        if (Input.GetKeyDown(KeyCode.C))
            Send(new BackTumbling());
    }

    public void FacingDirectionUpdate()
    {
        var corners = _navMeshAgent.path.corners;
        if (corners.Length >= 2)
        {
            var vec = corners[1] - corners[0];
            transform.LookAt(Vector3.Slerp(transform.forward, vec.normalized, 0.3f) + transform.position);
        }
    }

    public void Attack(Attack info)
    {
        _animator.SetBool("Attack", true);
    }

    public void StopAttack()
    {
        _animator.SetBool("Attack", false);
        _attackSubject.enabled = false;
    }

    public void BackTumbling(BackTumbling info)
    {
        _animator.SetTrigger("BackTumbling");
    }

    protected override void Die()
    {
        _animator.SetTrigger("Die");
        GetComponent<Collider>().enabled = false;
        Invoke("Respawn", 3.0f);
    }

    void Respawn()
    {
        hp = hpMax;
        SwitchWorld("Town");
    }

    protected override void Knockdown()
    {
        //_animator.SetTrigger("Knockdown");
    }

    void Navigate(Navigate info)
    {
        _navMeshAgent.destination = info.destination;
        _animator.SetBool("Run", true);
    }

    public void Speak(string chat)
    {
        speechBubble.Show(chat);
    }

    public void SwitchWorld(string world)
    {
        Send(new SwitchWorld(RPCType.Specific, Application.loadedLevelName, name, world));
    }

    public void SwitchWorld(SwitchWorld rpc)
    {
        if (isServer)
        {
            rpc.receiver = rpc.user;
            Send(rpc);
            Destroy();
        }
        else
        {
            Debug.Log("Switch World On " + rpc.world);
            SceneManager.LoadScene(rpc.world);
            Send(new BufferedRPCRequest(userName));

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
        Item item = (Item)rpc.item;
        _itemStacks.Find((ItemStack s) => { return s.IsPushable(item); }).Push(item);
    }

    public void ItemUse(ItemUse rpc)
    {
        _itemStacks[rpc.index].Use(this);
    }

    public void Equip(Equipment equipment)
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
                weapon = (Weapon)equipment;
                break;
        }
        _animator.SetInteger("WeaponType", (int)_weapon.weaponType);
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

    public void SerializedPlayer(SerializedPlayer rpc)
    {
        if (isLocal)
            return;
        if (rpc.data.Length <= 1)
            return;
        Deserialize(rpc.data);

        //Weapon Sync;
        weapon = weapon;

        //itemStack Sync
        Sync s = new Sync(RPCType.Others, "", "itemStacks");
        s.signallerID = networkID;
        s.sender = userName;
        Sync(s);
    }

    public byte[] Serialize()
    {
        return Serializer.Serialize(this as IAutoSerializable);
    }

    public void Deserialize(byte[] buffer)
    {
        Serializer.Deserialize(this as IAutoSerializable, buffer);
    }

    public int SerializedSize()
    {
        return Serializer.SerializedSize(this as IAutoSerializable);
    }

    public Header CreateHeader()
    {
        return Serializer.CreateHeader(this as IAutoSerializable);
    }
}