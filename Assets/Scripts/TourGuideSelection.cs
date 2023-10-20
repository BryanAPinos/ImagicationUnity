using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TourGuideSelection : MonoBehaviour
{
    public Color gray;
    public Color red;
    public Color blue;
    public Button StudentBtn;
    public Button GuideBtn;

    public Image characterModel;
    public Sprite Craig;
    public Sprite Dave;

    public GameObject passwordField;
    public GameObject tourNameField;

    public GameObject NameField;
    public GameObject NameTitle;

    public static string model = "student";

    public void TourGuidePressed()
    {
        characterModel.GetComponent<Image>().sprite = Dave;

        ColorBlock cb_Guide = GuideBtn.colors;
        cb_Guide.normalColor = blue;
        GuideBtn.colors = cb_Guide;

        ColorBlock cb_Student = StudentBtn.colors;
        cb_Student.normalColor = gray;
        StudentBtn.colors = cb_Student;
        passwordField.SetActive(true);

        // Shorten the width of the name input field to 150px
        RectTransform nameInputField = tourNameField.GetComponent<RectTransform>();
        nameInputField.sizeDelta = new Vector2(200, 35);
        nameInputField.localPosition = new Vector2(-68.6f, 1);

        RectTransform nameField = NameField.GetComponent<RectTransform>();
        nameField.localPosition = new Vector2(63.1f, -61);

        RectTransform nameTitle = NameTitle.GetComponent<RectTransform>();
        nameTitle.localPosition = new Vector2(-85.7f, 35);

        model = "tourguide";
    }

    public void StudentPressed()
    {
        characterModel.GetComponent<Image>().sprite = Craig;

        ColorBlock cb_Guide = GuideBtn.colors;
        cb_Guide.normalColor = gray;
        GuideBtn.colors = cb_Guide;

        ColorBlock cb_Student = StudentBtn.colors;
        cb_Student.normalColor = red;
        StudentBtn.colors = cb_Student;
        passwordField.SetActive(false);

        RectTransform nameInputField = tourNameField.GetComponent<RectTransform>();
        nameInputField.sizeDelta = new Vector2(300, 35);
        nameInputField.localPosition = new Vector2(4, 1);

        RectTransform nameField = NameField.GetComponent<RectTransform>();
        nameField.localPosition = new Vector2(126, -61);

        RectTransform nameTitle = NameTitle.GetComponent<RectTransform>();
        nameTitle.localPosition = new Vector2(-66, 35);

        model = "student";
    }
}
