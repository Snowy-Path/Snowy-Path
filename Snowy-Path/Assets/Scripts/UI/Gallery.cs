using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery : MonoBehaviour
{
    public List<Color> imageList;
    public GameObject galleryWindow;

    GameObject m_galleryContent;

    void Start()
    {
        m_galleryContent = GetComponentInChildren<GridLayoutGroup>().gameObject;

        int i = 0;
        foreach (var image in imageList) {
            GameObject galleryImage = new GameObject("GalleryImage");
            galleryImage.transform.parent = m_galleryContent.transform;

            Image img = galleryImage.AddComponent<Image>();
            img.color = Random.ColorHSV(); // FIXME: image unused

            int imageID = i;
            Button btn = galleryImage.AddComponent<Button>();
            btn.onClick.AddListener(() => OnImageClick(imageID));

            ++i;
        }
    }

    void OnImageClick(int imageID)
    {
        if (!galleryWindow.activeSelf) {
            galleryWindow.SetActive(true);
        }
    }

    public void OnBackButtonClicked() {
        if (galleryWindow.activeSelf)
            galleryWindow.SetActive(false);
        else
            gameObject.SetActive(false);
    }
}
