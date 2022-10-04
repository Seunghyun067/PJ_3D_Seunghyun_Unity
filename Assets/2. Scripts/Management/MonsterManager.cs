using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterCreatePositionType { SWORD_MONSTER }

public class MonsterManager : Singleton<MonsterManager>
{

    private Vector3[] swordMonsterPos = new Vector3[4];
    private Vector3[] rifleMonsterPos = new Vector3[3];
    [SerializeField] GameObject[] angPos;

    public List<GameObject> activeMonsters = new List<GameObject>();

    private void Awake()
    {
        StartCoroutine(MonsterCheck());
        swordMonsterPos[0] = new Vector3(-32.082325f, 0.228000045f, 3.00129986f);
        swordMonsterPos[1] = new Vector3(-32.144268f, 0.228000045f, 12.9178267f);
        swordMonsterPos[2] = new Vector3(-25.2915802f, 0.228000164f, 2.2557323f);
        swordMonsterPos[3] = new Vector3(-25.3995762f, 0.228000045f, 13.8301229f);

        rifleMonsterPos[0] = new Vector3(-35.3289604f, 0.120000482f, -0.858152926f);
        rifleMonsterPos[1] = new Vector3(-35.355732f, 0.0946056843f, 13.6669798f);
        rifleMonsterPos[2] = new Vector3(-35.3361626f, 0.120000362f, 7.53509521f);
    }

    public void ResetActiveMonsters()
    {
        foreach (var monster in activeMonsters)
            ObjectPooling.Instance.PushObject(monster.gameObject);
        activeMonsters.Clear();
    }

    IEnumerator MonsterCheck()
    {
        
        while (true)
        {            
            foreach (var monster in activeMonsters)
            {                
                if (!monster.activeSelf)
                    activeMonsters.Remove(monster);
            }
            yield return new WaitForSeconds(1f);
        }
        
    }

    IEnumerator SwordRobotDeadEvent()
    {
        while(true)
        {
            if (activeMonsters.Count == 0)
            {
                TimelineManager.Instance.PlayTimeline("AircraftOn");
                yield break;
            }

            yield return new WaitForSeconds(1f);

        }       
    }

    public void StopCoroutine()
    {
        StopAllCoroutines();
    }

    Coroutine swordCo;
    public void CreateSwordMonster()
    {
        foreach (var pos in swordMonsterPos)
        {
            GameObject obj = ObjectPooling.Instance.PopObject("SwordRobot", pos);
            activeMonsters.Add(obj);
        }
        swordCo = StartCoroutine(SwordRobotDeadEvent());
    }




    public void CreateRifleMonster()
    {
        foreach (var pos in rifleMonsterPos)
        {

            GameObject obj = ObjectPooling.Instance.PopObject("RifleRobot", pos);
            activeMonsters.Add(obj);
        }

    }

    public void CreateAngPos()
    {
        foreach (var pos in angPos)
            ObjectPooling.Instance.PopObject("RifleRobot", pos.transform.position);
    }
}
