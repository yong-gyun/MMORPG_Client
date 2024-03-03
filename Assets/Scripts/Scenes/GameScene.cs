using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;
        Managers.Map.LoadMap(1);

        GameObject player = Managers.Resource.Instantiate("Creature/Player");
        player.name = "Player";
        Managers.Object.Add(player);

        for (int i = 0; i < 5; i++)
        {
            GameObject monster = Managers.Resource.Instantiate("Creature/Monster");
            monster.name = $"Monster_{i + 1}";
            Managers.Object.Add(monster);

            Vector3Int pos = new Vector3Int()
            {
                x = Random.Range(-20, 20),
                y = Random.Range(-10, 10)
            };

            MonsterController mc = monster.GetComponent<MonsterController>();
            mc.CellPos = pos;
            monster.transform.position = pos;
        }

    }

    public override void Clear()
    {
        
    }
}
