using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterCreatePositionType { SWORD_MONSTER }

public class MonsterManager : Singleton<MonsterManager>
{

    [SerializeField] GameObject[] swordMonsterPos;
    [SerializeField] GameObject[] rifleMonsterPos;

    List<SwordRobot> swordMonsters = new List<SwordRobot>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SwordRobotDeadEvent()
    {
        bool isAllDead = false;
        while(true)
        {
            foreach (var monster in swordMonsters)
            {
                if (!(isAllDead = monster.isDead))
                    break;
            }

            if(isAllDead)
            {
                TimelineManager.Instance.PlayTimeline("AircraftOn");
                yield break;
            }

            yield return new WaitForSeconds(1f);

        }
        

        
    }

    public void CreateSwordMonster()
    {
        foreach (var pos in swordMonsterPos)
        {
            GameObject obj = ObjectPooling.Instance.PopObject("SwordRobot", pos.transform.position);
            swordMonsters.Add(obj.GetComponent<SwordRobot>());
        }
        StartCoroutine(SwordRobotDeadEvent());
    }



    public void CreateRifleMonster()
    {
        foreach (var pos in rifleMonsterPos)
            ObjectPooling.Instance.PopObject("RifleRobot", pos.transform.position);
    }
}
