using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public Terrain world;
    public Image playerIcon;
    public Image pfSpawnerIcon;
    public Image pfNPCIcon;
    public Image pfPortalIcon;
    ScrollRectEx _scroll;
    RectTransform _playerIconRT;

    void Awake()
    {
        _scroll = GetComponent<ScrollRectEx>();
        _playerIconRT = playerIcon.GetComponent<RectTransform>();
    }

    void Start()
    {
        var spawners = FindObjectsOfType<MonsterSpawner>();
        foreach (var spawner in spawners)
        {
            var spawnerIcon = Instantiate(pfSpawnerIcon);
            spawnerIcon.rectTransform.SetParent(_scroll.content.transform);
            spawnerIcon.rectTransform.anchoredPosition = new Vector2(spawner.transform.position.x / world.terrainData.size.x * _scroll.content.sizeDelta.x, spawner.transform.position.z / world.terrainData.size.z * _scroll.content.sizeDelta.y);
            var sx = spawner.spawnRange * 2 / world.terrainData.size.x * _scroll.content.sizeDelta.x / 128;
            var sy = spawner.spawnRange * 2 / world.terrainData.size.z * _scroll.content.sizeDelta.y / 128;
            spawnerIcon.rectTransform.localScale = new Vector3(sx, sy);
        }
        var npcs = FindObjectsOfType<NPC>();
        foreach (var npc in npcs)
        {
            var npcIcon = Instantiate(pfNPCIcon);
            npcIcon.rectTransform.SetParent(_scroll.content.transform);
            npcIcon.rectTransform.anchoredPosition = new Vector2(npc.transform.position.x / world.terrainData.size.x * _scroll.content.sizeDelta.x, npc.transform.position.z / world.terrainData.size.z * _scroll.content.sizeDelta.y);
            npcIcon.rectTransform.localScale = new Vector3(1, 1, 1);
        }
        var portals = FindObjectsOfType<Portal>();
        foreach (var portal in portals)
        {
            var portalIcon = Instantiate(pfPortalIcon);
            portalIcon.rectTransform.SetParent(_scroll.content.transform);
            portalIcon.rectTransform.anchoredPosition = new Vector2(portal.transform.position.x / world.terrainData.size.x * _scroll.content.sizeDelta.x, portal.transform.position.z / world.terrainData.size.z * _scroll.content.sizeDelta.y);
            portalIcon.rectTransform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Update()
    {
        if (Player.mine)
        {
            var ppos = new Vector2(Player.mine.transform.position.x / world.terrainData.size.x * _scroll.content.sizeDelta.x, Player.mine.transform.position.z / world.terrainData.size.z * _scroll.content.sizeDelta.y);
            _playerIconRT.anchoredPosition = ppos;
            _scroll.SetContentAnchoredPosition(-ppos);
            _scroll.SetDirty();
        }
    }
}