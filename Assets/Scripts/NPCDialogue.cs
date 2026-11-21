using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public Dialogue dialogueManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            string[] dialogue = {
                "RedHood: Oh! A visitor. I haven't seen anyone down here in ages.",
                "Bobby: Hi... where exactly am I?",
                "RedHood: Somewhere you probably shouldn't be.",
                "RedHood: If you want to get out, head east. Watch out for the enemies.",
                "Bobby: Thanks! I'll be careful."
            };

            dialogueManager.SetSentences(dialogue);
            dialogueManager.StartCoroutine(dialogueManager.TypeDialogue());

            Destroy(GetComponent<BoxCollider2D>(), 5f);
        }
    }
}