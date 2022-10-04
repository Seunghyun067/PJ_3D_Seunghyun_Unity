using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ConversationController : MonoBehaviour
{
    private AudioSource audio;    
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI conversationText;
    [SerializeField] private float converSpeed = 1f;

    private Coroutine converCo;
    private bool isConverComp = false;
    private int curIndex = 0;

    [SerializeField]
    private ConversationInformation test;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void StartConversation(ConversationInformation conver, UnityAction endConverEvent)
    {
        curIndex = 0;
        gameObject.SetActive(true);
        StartCoroutine(ConversationCo(conver, endConverEvent));
    }


    IEnumerator ConversationCo(ConversationInformation conver, UnityAction endConverEvent)
    {        
        isConverComp = false;
        
        converCo = StartCoroutine(ConverTextCo(conver.Conversation[curIndex].talk, conver.Conversation[curIndex].name));

        while (true)
        {            
            if (Input.GetMouseButtonDown(0))
            {
                if (isConverComp && curIndex + 1 < conver.Conversation.Length)
                    converCo = StartCoroutine(ConverTextCo(conver.Conversation[++curIndex].talk, conver.Conversation[curIndex].name));
                else
                {
                    StopCoroutine(converCo);
                    isConverComp = true;
                    conversationText.text = conver.Conversation[curIndex].talk;
                }
            }
            
            if (curIndex + 1 == conver.Conversation.Length && isConverComp)
                break;

            yield return null;
        }
        yield return null;
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {

                endConverEvent?.Invoke();
                gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
        
    }

    IEnumerator ConverTextCo(string text, string name)
    {
        audio.Play();
        isConverComp = false;
        conversationText.text = "";
        nameText.text = name;
        foreach (var c in text)
        {
            conversationText.text += c;
            yield return new WaitForSecondsRealtime(1f / converSpeed);
        }
        isConverComp = true;
    }

}
