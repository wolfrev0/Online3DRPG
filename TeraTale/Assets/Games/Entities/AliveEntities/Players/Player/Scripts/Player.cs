using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TeraTaleNet;
using System;
using System.Reflection;

public class Player : AliveEntity, ISerializable
{
    static Dictionary<Type, MethodInfo> serializersCache = new Dictionary<Type, MethodInfo>();
    static Dictionary<Type, MethodInfo> serializedSizesCache = new Dictionary<Type, MethodInfo>();

    static Dictionary<string, Player> _playersByName = new Dictionary<string, Player>();
    const float kRaycastDistance = 50.0f;

    public Text nameView;
    public SpeechBubble speechBubble;

    NavMeshAgent _navMeshAgent;
    Animator _animator;
    List<ItemStack> _itemStacks = new List<ItemStack>(30);
    public Weapon _weapon;
    ItemSolid _weaponSolid;
    //Rename Attacker to AttackSubject??
    AttackSubject _attackSubject;
    //StreamingSkill (Base Attack) Management
    Projectile _pfArrow;
    
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

    public bool isArrived { get { return Vector3.Distance(_navMeshAgent.destination, _navMeshAgent.transform.position) <= _navMeshAgent.stoppingDistance; } }

    void OnWeaponInstantiate(ItemSolid itemSolid)
    {
        _weaponSolid = itemSolid;        
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
        Equip(new WeaponNull());
        transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
        _pfArrow = Resources.Load<Projectile>("Prefabs/Arrow");

        if (isServer)
            Send(new SerializedPlayer(this));
    }

    new void OnDestroy()
    {
        base.OnDestroy();
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
        Invoke("Respawn", 3.0f);
    }

    void Respawn()
    {
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

    }

    public byte[] Serialize()
    {
    }

    public void Deserialize(byte[] buffer)
    {
        throw new NotImplementedException();
    }

    public int SerializedSize()
    {
        throw new NotImplementedException();
    }
}