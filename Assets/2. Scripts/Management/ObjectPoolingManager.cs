using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : Singleton<GameManager>
{
    [System.Serializable]
    public class PooledObject
    {
        public GameObject obj;
        public int Count = 1;
    }
    [SerializeField] private List<PooledObject> inputObjects = new List<PooledObject>();
    private Dictionary<string, Queue<IPoolingEnable>> objectPool = new Dictionary<string, Queue<IPoolingEnable>>();

    private Dictionary<string, GameObject> objectPoolParents = new Dictionary<string, GameObject>();

    void Awake()
    {
        foreach (var inputObj in inputObjects)
        {
#if UNITY_EDITOR
            if (objectPool.ContainsKey(inputObj.obj.name))
            {

                Debug.LogError("오브젝트 풀에 다른 오브젝트의 중복된 이름이 들어있음");
                UnityEditor.EditorApplication.isPlaying = false;
                return;
            }
#endif

            GameObject parentObj = new GameObject(inputObj.obj.name);
            parentObj.transform.parent = transform;
            objectPoolParents.Add(parentObj.name, parentObj);

            Queue<IPoolingEnable> newPool = new Queue<IPoolingEnable>();
            for (int i = 0; i < inputObj.Count; ++i)
            {
                var obj = Instantiate(inputObj.obj);
                IPoolingEnable poolObject = obj.GetComponent<IPoolingEnable>();                
                obj.SetActive(false);
                obj.transform.parent = parentObj.transform;
                newPool.Enqueue(poolObject);
            }
            objectPool.Add(inputObj.obj.name, newPool);
        }
    }

    public IPoolingEnable PopObject(string tag, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        if (!objectPool.ContainsKey(tag))
        {
            Debug.LogError("오브젝트 풀에" + tag + " 없음");
            return null;
        }
        var obj = objectPool[tag].Dequeue();
        obj.GetGameObject().SetActive(true);
        obj.GetGameObject().transform.parent = null;
        obj.GetGameObject().transform.position = position;
        obj.GetGameObject().transform.rotation = rotation;
        return obj;
    }
    public void PushObject(IPoolingEnable obj)
    {
        string tag = obj.GetGameObject().name;
        int cutIndex = tag.IndexOf(' ');
        if (-1 != cutIndex) tag = tag.Substring(0, cutIndex);

        cutIndex = tag.IndexOf('(');
        if (-1 != cutIndex) tag = tag.Substring(0, cutIndex);

        if (!objectPool.ContainsKey(tag))
        {
            Debug.LogError(tag + " 풀이 비어있음");
            return;
        }

        obj.GetGameObject().SetActive(false);
        obj.GetGameObject().transform.parent = objectPoolParents[tag].transform;
        objectPool[tag].Enqueue(obj);
    }
}