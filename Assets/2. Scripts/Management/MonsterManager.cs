using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterCreatePositionType { SWORD_MONSTER }

public class MonsterManager : Singleton<MonsterManager>
{
    [SerializeField]
    GameObject[] swordMonsterPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSwordMonster()
    {
        foreach (var pos in swordMonsterPos)
            ObjectPooling.Instance.PopObject("SwordRobot", pos.transform.position);
    }
}
