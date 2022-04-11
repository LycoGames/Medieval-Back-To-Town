using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitRowUI : MonoBehaviour
{
    [SerializeField] Trait trait;
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] Button minusButton;
    [SerializeField] Button plusButton;

    TraitStore playerTraitStore = null;

    public void Allocate(int points)
    {
        playerTraitStore.AssignPoints(trait, points);
    }

    public void AddValue()
    {
        Allocate(+1);
    }

    public void MinusValue()
    {
        Allocate(-1);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
        //  minusButton.onClick.AddListener(() => Allocate(-1));
        //  plusButton.onClick.AddListener(() => Allocate(+1));
    }

    // Update is called once per frame
    void Update()
    {
        minusButton.interactable = playerTraitStore.CanAssignPoints(trait, -1);
        plusButton.interactable = playerTraitStore.CanAssignPoints(trait, +1);
        valueText.text = playerTraitStore.GetProposedPoints(trait).ToString();
    }
}
