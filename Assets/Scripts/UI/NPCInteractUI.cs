using TMPro;
using UnityEngine;

public class NPCInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject interactInfoText;
    [SerializeField] private GameObject interactText;
    private AIConversant AIConversant;

    private Transform mainCamTransform;

    // Start is called before the first frame update

    private void Start()
    {
        AIConversant = GetComponentInParent<AIConversant>();
        interactInfoText.GetComponent<TextMeshProUGUI>().text = "Talk " + AIConversant.GetConversantName();
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