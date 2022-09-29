using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterCreatePositionType { SWORD_MONSTER }

public class MonsterManager : Singleton<MonsterManager>
{

    [SerializeField] GameObject[] swordMonsterPos;
    [SerializeField] GameObject[] rifleMonsterPos;
    [SerializeField] GameObject[] angPos;

    public List<GameObject> activeMonsters = new List<GameObject>();

    private void Awake()
    {
        StartCoroutine(MonsterCheck());
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
            Debug.Log(activeMonsters.Count);
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

   

    public void CreateSwordMonster()
    {
        foreach (var pos in swordMonsterPos)
        {
            GameObject obj = ObjectPooling.Instance.PopObject("SwordRobot", pos.transform.position);
            activeMonsters.Add(obj);
        }
        StartCoroutine(SwordRobotDeadEvent());
    }




    public void CreateRifleMonster()
    {
        foreach (var pos in rifleMonsterPos)
        {

            GameObject obj = ObjectPooling.Instance.PopObject("RifleRobot", pos.transform.position);
            activeMonsters.Add(obj);
        }

    }

    public void CreateAngPos()
    {
        foreach (var pos in angPos)
            ObjectPooling.Instance.PopObject("RifleRobot", pos.transform.position);
    }
}
