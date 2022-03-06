using System;
using UnityEngine;
using UnityEngine.UI;


public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;

    public bool isInRange;

    private Text interactUI;
    
    public static DialogueTrigger instance;
    
    public AudioSource audioSource;
    public AudioSource audioSource1;


    private void Awake()
    {
        interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<Text>();
        interactUI.enabled = false;
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de DialogueManager dans la scène");
            return;
        }
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (isInRange && Input.GetKeyDown((KeyCode.E)))
        {
            audioSource1.Pause();
            audioSource.Play();
            TriggerDialogue();
            
        }
        else
        {
            audioSource.Pause();
            audioSource1.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            interactUI.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            interactUI.enabled = false;
        }
    }

    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }
}