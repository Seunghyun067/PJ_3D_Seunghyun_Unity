using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : Singleton<ObjectPooling>
{    
    [System.Serializable]
    public class PooledObject
    {
        public GameObject obj;
        public int Count = 1;
    }
    [SerializeField] private List<PooledObject> inputObjects = new List<PooledObject>();
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> objectPoolParents = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        foreach(var inputObj in inputObjects)
        {
            if (objectPool.ContainsKey(inputObj.obj.name))
            {
                Debug.LogError("오브젝트 풀에 다른 오브젝트의 중복된 이름이 들어있음");
                UnityEditor.EditorApplication.isPlaying = false;
                return;
            }


            GameObject parentObj = new GameObject(inputObj.obj.name);
            parentObj.transform.parent = transform;
            objectPoolParents.Add(parentObj.name, parentObj);
                
            Queue<GameObject> newPool = new Queue<GameObject>();
            for(int i = 0; i < inputObj.Count; ++i)
            {               
                var obj = Instantiate(inputObj.obj);
                obj.SetActive(false);
                obj.transform.parent = parentObj.transform;
                newPool.Enqueue(obj);
            }
            objectPool.Add(inputObj.obj.name, newPool);
        }
    }

    public GameObject PopObject(string tag, Vector3 position, Quaternion rotation)
    {
        if(!objectPool.ContainsKey(tag))
        {
            Debug.LogError("오브젝트 풀에" + tag + " 없음");
            return null;
        }
        var obj = objectPool[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.parent = null;
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public GameObject PopObject(string tag, Vector3 position)
    {
        if (!objectPool.ContainsKey(tag))
        {
            Debug.LogError("오브젝트 풀에" + tag + " 없음");
            return null;
        }

        if (objectPool[tag].Count == 0)
        {
            Debug.LogError(tag + "풀이 비었음");
            return null;
        }

        var obj = objectPool[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.parent = null;
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;
        return obj;
    }

    public GameObject PopObject(string tag)
    {
        if (!objectPool.ContainsKey(tag))
        {
            Debug.LogError("오브젝트 풀에" + tag + " 없음");
            return null;
        }
        var obj = objectPool[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }

    public void PushObject(GameObject obj)
    {
        string tag = obj.name;
        int cutIndex = tag.IndexOf(' ');
        if (-1 != cutIndex) tag = tag.Substring(0, cutIndex);

        cutIndex = tag.IndexOf('(');
        if (-1 != cutIndex) tag = tag.Substring(0, cutIndex);

        if (!objectPool.ContainsKey(tag))
        {
            Debug.LogError(tag + " 풀이 비어있음");
            return;
        }

        obj.SetActive(false);
        obj.transform.parent = objectPoolParents[tag].transform;
        objectPool[tag].Enqueue(obj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
