using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI unassignedPointsText;
    [SerializeField] Button commitButton;

    TraitStore playerTraitStore = null;

    public event Action onConfirm;

    void Start()
    {
        playerTraitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
        commitButton.onClick.AddListener(playerTraitStore.Commit);
    }

    void Update()
    {
        unassignedPointsText.text = playerTraitStore.GetUnassignedPoints().ToString();
    }

}