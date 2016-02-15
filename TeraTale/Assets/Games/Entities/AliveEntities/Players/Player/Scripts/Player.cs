using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TeraTaleNet;
using System.Linq;

public class Player : AliveEntity
{
    static Dictionary<string, Player> _playersByName = new Dictionary<string, Player>();
    static float[] _hpMaxByLevel = new float[] { 1, 100, 110, 120, 130, 150 };
    static float[] _staminaMaxByLevel = new float[] { 1, 110, 115, 120, 126, 133 };
    static float[] _baseAttackDamageByLevel = new float[] { 1, 10, 12, 15, 18, 20 };
    static float[] _baseAttackSpeedByLevel = new float[] { 1, 1.01f, 1.02f, 1.03f, 1.04f, 1.05f };

    public Text nameView;
    public SpeechBubble speechBubble;

    NavMeshAgent _navMeshAgent;
    Animator _animator;
    public ItemStackList itemStacks = new ItemStackList(30);
    public Weapon _weapon = new WeaponNull();
    ItemSolid _weaponSolid;
    public Accessory _accessory = new AccessoryNull();
    ItemSolid _accessorySolid;
    //Rename Attacker to AttackSubject??
    AttackSubject _attackSubject;
    //StreamingSkill (Base Attack) Management
    static Projectile _pfArrow;
    static Player _pfPlayer;

    public int _money = 0;
    public int money { get { return _money; } private set { _money = value; } }

    public override float hpMax { get { return _hpMaxByLevel[level]; } }
    public override float staminaMax { get { return _staminaMaxByLevel[level]; } }
    public override float baseAttackDamage { get { return _baseAttackDamageByLevel[level]; } }
    public override float bonusAttackDamage { get { return _weapon.bonusAttackDamage; } }
    public override float baseAttackSpeed { get { return _baseAttackSpeedByLevel[level]; } }
    public override float bonusAttackSpeed { get { return _weapon.bonusAttackSpeed; } }
    public override float baseMoveSpeed { get { return 3.5f; } }
    public override float bonusMoveSpeed { get { return _accessory.bonusMoveSpeed; } }

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

    static public List<Player> players { get { return _playersByName.Values.ToList(); } }

    static public Player FindPlayerByName(string name)
    {
        if (name == null)
            return null;
        Player ret;
        _playersByName.TryGetValue(name, out ret);
        return ret;
    }

    public Weapon weapon
    {
        get { return _weapon; }

        private set
        {
            _weapon = value;
            _animator.SetInteger("WeaponType", (int)_weapon.weaponType);
            if (isServer)
            {
                NetworkDestroy(_weaponSolid);
                NetworkInstantiate(_weapon.solidPrefab.GetComponent<NetworkScript>(), new ItemSolidArgument(_weapon, transform.position + Vector3.up, 0, 0), "OnWeaponInstantiate");
            }
        }
    }

    public Accessory accessory
    {
        get { return _accessory; }

        private set
        {
            _accessory = value;
            if (isServer)
            {
                NetworkDestroy(_accessorySolid);
                NetworkInstantiate(_accessory.solidPrefab.GetComponent<NetworkScript>(), new ItemSolidArgument(_accessory, transform.position + Vector3.up, 0, 0), "OnAccessoryInstantiate");
            }
        }
    }

    public bool isArrived { get { return Vector3.Distance(_navMeshAgent.destination, _navMeshAgent.transform.position) <= _navMeshAgent.stoppingDistance; } }

    void OnWeaponInstantiate(ItemSolid itemSolid)
    {
        _weaponSolid = itemSolid;
        _weapon = (Weapon)_weaponSolid.item;
        _weaponSolid.transform.SetParent(_animator.GetBoneTransform(weapon.targetBone));
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

    void OnAccessoryInstantiate(ItemSolid itemSolid)
    {
        _accessorySolid = itemSolid;
        _accessory = (Accessory)_accessorySolid.item;
        _accessorySolid.transform.SetParent(_animator.GetBoneTransform(accessory.targetBone));
        _accessorySolid.transform.localPosition = Vector3.zero;
        _accessorySolid.transform.localRotation = Quaternion.identity;
        _accessorySolid.transform.localScale = Vector3.one;
        _accessorySolid.enabled = false;
        _accessorySolid.GetComponent<Floater>().enabled = false;
        _accessorySolid.GetComponent<ItemSpawnEffector>().enabled = false;
    }

    protected void Awake()
    {
        for (int i = 0; i < 30; i++)
            itemStacks.Add(new ItemStack());
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    protected new void Start()
    {
        base.Start();
        name = owner;
        nameView.text = name;
        _playersByName.Add(name, this);
        if (isMine)
        {
            var cam = FindObjectOfType<CameraController>();
            cam.target = transform;
            Destroy(cam.GetComponent<AudioListener>());
            gameObject.AddComponent<AudioListener>();
            GameObject.FindWithTag("PlayerStatusView").GetComponent<StatusView>().target = this;
        }
        transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
        if (_pfArrow == null)
            _pfArrow = Resources.Load<Projectile>("Prefabs/Arrow");
        if (_pfPlayer == null)
            _pfPlayer = Resources.Load<Player>("Prefabs/Player");

        if (isServer)
            GameServer.currentInstance.QuerySerializedPlayer(name);
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
        GetComponent<CapsuleCollider>().center = new Vector3(float.MaxValue / 2, float.MaxValue / 2, float.MaxValue / 2);
        Invoke("Respawn", 3.0f);
    }

    void Respawn()
    {
        hp = hpMax;
        SwitchWorld("Town");
    }

    protected override void Knockdown()
    {
        _animator.SetTrigger("Knockdown");
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
            SceneManager.LoadScene(rpc.world);
            Send(new BufferedRPCRequest(userName));
            NetworkProgramUnity.currentInstance.NetworkInstantiate(_pfPlayer);
        }
    }

    public void AddItem(Item item)
    {
        Send(new AddItem(item));
    }

    public void AddItem(AddItem rpc)
    {
        Item item = (Item)rpc.item;
        itemStacks.Find((ItemStack s) => { return s.IsPushable(item); }).Push(item);
    }

    public bool CanAddItem(Item item, int amount)
    {
        return itemStacks.Find((ItemStack s) => { return s.IsPushable(item, amount); }) != null;
    }

    public void ItemUse(ItemUse rpc)
    {
        itemStacks[rpc.index].Use(this);
    }

    public void Use(int itemStackIndex)
    {
        Send(new ItemUse(itemStackIndex));
    }

    public void Equip(Equipment equipment)
    {
        switch (equipment.equipmentType)
        {
            case Equipment.Type.Accessory:
                accessory = (Accessory)equipment;
                break;
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
            case Equipment.Type.Accessory:
                return accessory == equipment;
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

        

        //Deserialize only set the field _weapon, not the property weapon. so, we should call this property manually.
        weapon = weapon;
        //Weapon Sync;
        Sync s = new Sync(RPCType.Others, "", "weapon");
        s.signallerID = networkID;
        s.sender = userName;
        Sync(s);

        //same as weapon
        accessory = accessory;
        s = new Sync(RPCType.Others, "", "accessory");
        s.signallerID = networkID;
        s.sender = userName;
        Sync(s);

        //itemStack Sync
        s = new Sync(RPCType.Others, "", "itemStacks");
        s.signallerID = networkID;
        s.sender = userName;
        Sync(s);

        //money Sync
        s = new Sync(RPCType.Others, "", "money");
        s.signallerID = networkID;
        s.sender = userName;
        Sync(s);
    }

    public void DropItemStack(DropItemStack rpc)
    {
        var itemStack = itemStacks[rpc.index];
        var itemCount = itemStack.count;
        for (int i = 0; i < itemCount; i++)
        {
            var item = itemStack.Pop();
            if (isServer)
                NetworkInstantiate(item.solidPrefab.GetComponent<ItemSolid>(), new ItemSolidArgument(item, transform.position + Vector3.up, 0, 0));
        }
    }

    public void DropItemStack(int index)
    {
        Send(new DropItemStack(index));
    }

    public void SwapItemStack(SwapItemStack rpc)
    {
        var tmp = itemStacks[rpc.indexA];
        itemStacks[rpc.indexA] = itemStacks[rpc.indexB];
        itemStacks[rpc.indexB] = tmp;
    }

    public void SwapItemStack(int indexA, int indexB)
    {
        Send(new SwapItemStack(indexA, indexB));
    }

    public void SellItem(SellItem rpc)
    {
        for (int i = 0; i < rpc.amount; i++)
            money += itemStacks[rpc.index].Pop().price;
    }

    public void SellItem(int index, int amount)
    {
        Send(new SellItem(index, amount));
    }

    public void BuyItem(BuyItem rpc)
    {
        money -= rpc.item.price * rpc.amount;
        for (int i = 0; i < rpc.amount; i++)
            AddItem(rpc.item);

        var s = new Sync(RPCType.Others, "", "money");
        s.signallerID = networkID;
        s.sender = userName;
        Sync(s);
    }

    public void BuyItem(Item item, int amount)
    {
        Send(new BuyItem(Application.loadedLevelName, item, amount));
    }

    public int ItemCount(Item item)
    {
        int count = 0;

        for (int i = 0; i < itemStacks.count; i++)
            if (itemStacks[i].item.IsSameType(item))
                count += itemStacks[i].count;

        return count;
    }

    public void RemoveItem(RemoveItem rpc)
    {
        for (int i = 0; i < itemStacks.count; i++)
        {
            var itemStack = itemStacks[i];
            if (itemStack.item.IsSameType(rpc.item))
            {
                while (itemStack.count != 0)
                {
                    if (rpc.amount == 0)
                        return;
                    itemStack.Pop();
                    --rpc.amount;
                }
            }
        }
    }

    public void RemoveItem(Item item, int amount)
    {
        Send(new RemoveItem(item, amount));
    }
}