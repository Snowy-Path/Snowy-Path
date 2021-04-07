using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery : MonoBehaviour
{
    GalleryWindow m_galleryWindow;
    GameObject m_galleryContent;
    Object[] m_galleryElements;

    void Start()
    {
        m_galleryWindow = GetComponentInChildren<GalleryWindow>();
        m_galleryWindow.gameObject.SetActive(false);

        m_galleryContent = GetComponentInChildren<GridLayoutGroup>().gameObject;

        m_galleryElements = Resources.LoadAll<GalleryElement>("Gallery");
        if (m_galleryElements.Length < 1) {
            throw new System.Exception("Gallery folder is empty, or its path is wrong.");
        }

        int i = 0;
        foreach (var elem in m_galleryElements) {
            GalleryElement galleryElement = (GalleryElement)elem; 

            GameObject galleryImage = new GameObject("GalleryImage");
            galleryImage.transform.parent = m_galleryContent.transform;

            RawImage img = galleryImage.AddComponent<RawImage>();
            img.color = Color.white;
            img.texture = galleryElement.texture;

            int elementID = i;
            Button btn = galleryImage.AddComponent<Button>();
            btn.onClick.AddListener(() => OnImageClick(elementID));

            ++i;
        }
    }

    void OnImageClick(int elementID)
    {
        if (!m_galleryWindow.gameObject.activeSelf) {
            var galleryElement = (GalleryElement)m_galleryElements[elementID];
            m_galleryWindow.title.text = galleryElement.title;
            m_galleryWindow.description.text = galleryElement.description;
            m_galleryWindow.image.texture = galleryElement.texture;

            m_galleryWindow.gameObject.SetActive(true);
        }
    }

    public void OnBackButtonClicked() {
        if (m_galleryWindow.gameObject.activeSelf)
            m_galleryWindow.gameObject.SetActive(false);
        else
            gameObject.SetActive(false);
    }
}
