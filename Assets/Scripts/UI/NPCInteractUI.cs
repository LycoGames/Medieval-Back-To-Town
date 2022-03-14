using TMPro;
using UnityEngine;

public class NPCInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject interactInfoText;
    [SerializeField] private GameObject interactText;

    private Transform mainCamTransform;

    // Start is called before the first frame update

    private void Start()
    {
        interactInfoText.GetComponent<TextMeshProUGUI>().text = "Talk " + gameObject.name;
        mainCamTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (interactInfoText.activeInHierarchy || interactText.activeInHierarchy)
        {
            transform.LookAt(transform.position + mainCamTransform.forward);
        }
    }

    public void SetActiveInteractInfo(bool state)
    {
        interactInfoText.SetActive(state);
    }

    public void SetActiveInteract(bool state)
    {
        interactText.SetActive(state);
    }
}