using UnityEngine;
using TeraTaleNet;

public class ItemFunctions : NetworkScript
{
    public ParticleSystem _fxpotion;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        HpPotion.onUse += (Item item) =>
        {
            var player = Player.mine;
            player.Heal(new Heal("", 30));
            ParticleSystem _particle = Instantiate(_fxpotion);
            _particle.transform.SetParent(player.transform);
            _particle.transform.localPosition = Vector3.zero;
            Destroy(_particle.gameObject,_particle.duration);
            player.Send(new Heal("", 30));
        };
        Equipment.onUse += (Item item) =>
        {
            var player = Player.mine;
            if (player.IsEquiping((Equipment)item))
                player.Equip(new WeaponNull());
            else
                player.Equip((Equipment)item);

        };
    }
}
