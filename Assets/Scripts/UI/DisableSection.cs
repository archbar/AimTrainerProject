using UnityEngine;
using UnityEngine.UI;

public class DisableSection : MonoBehaviour
{
    public Button toggleButton;
    public GameObject imagePanel1;
    public GameObject imagePanel2;

    void Start()
    {
        // Add a listener to the button click event
        toggleButton.onClick.AddListener(ToggleSections);
    }

    // Function to toggle visibility and enable/disable of image panels
    void ToggleSections()
    {
        // Toggle visibility and enable/disable of image panel 1 and its children
        ToggleVisibilityAndEnable(imagePanel1, false);

        // Toggle visibility and enable/disable of image panel 2 and its children
        ToggleVisibilityAndEnable(imagePanel2, true);
    }

    // Function to toggle visibility and enable/disable of a GameObject and its children
    void ToggleVisibilityAndEnable(GameObject go, bool enable)
    {
        if (go != null)
        {
            // Set the visibility
            go.SetActive(enable);

            // Set the visibility for all children recursively
            foreach (Transform child in go.transform)
            {
                ToggleVisibilityAndEnable(child.gameObject, enable);
            }
        }
    }
}
