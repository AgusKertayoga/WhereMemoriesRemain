using UnityEngine;

public class DLChara : MonoBehaviour
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Dialogue System/New Dialogue Character", order = 1)]

    public class DialogueCharacter : ScriptableObject
    {
        public Sprite characterPhoto;
        public string characterName;
    }
}
