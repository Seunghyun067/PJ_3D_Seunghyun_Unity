using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conver", menuName = "ScriptableObject/Conversation")]

public class ConversationInformation : ScriptableObject
{
    [System.Serializable]
    public class Conver
    {
        public string name = "";
        [TextArea(5, 7)]
        public string talk = "";
    }

    [SerializeField] private Conver[] conversation;
    public Conver[] Conversation { get { return conversation; } }    
}
