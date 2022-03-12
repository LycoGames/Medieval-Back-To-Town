using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    private PlayerConversant playerConversant;
    [SerializeField] private TextMeshProUGUI AIText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Transform choiceRoot;
    [SerializeField] private GameObject AIResponse;
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        playerConversant.onConversationUpdated += UpdateUI;
        nextButton.onClick.AddListener(() => playerConversant.Next());
        quitButton.onClick.AddListener(() => playerConversant.Quit());

        UpdateUI();
    }
    
    void UpdateUI()
    {
        gameObject.SetActive(playerConversant.IsActive());
        if (!playerConversant.IsActive())
            return;

        AIResponse.SetActive(!playerConversant.IsChoosing());
        choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());

        if (playerConversant.IsChoosing())
        {
            BuildChoiceList();
        }
        else
        {
            AIText.text = playerConversant.GetText();
            nextButton.gameObject.SetActive(playerConversant.HasNext());
        }
    }

    private void BuildChoiceList()
    {
        foreach (Transform item in choiceRoot)
        {
            Destroy(item.gameObject);
        }

        foreach (DialogueNode choice in playerConversant.GetChoices())
        {
            GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
            var textComp = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
            textComp.text = choice.GetText();
            Button button = choiceInstance.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => { playerConversant.SelectChoice(choice); });
        }
    }
}