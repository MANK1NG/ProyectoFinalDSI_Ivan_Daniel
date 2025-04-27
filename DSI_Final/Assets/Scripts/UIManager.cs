using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.LudiqRootObjectEditor;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    List<VisualElement> panel;
    VisualElement panelAct;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        panel = root.Query<VisualElement>("Generales").ToList();

        foreach (VisualElement element in panel)
            element.RegisterCallback<ClickEvent>(seleccionPanel);
    }

    void EstadoPanel(VisualElement ve, bool selec)
    {
        VisualElement tarjeta = ve.Q("tarjeta");

        string oldClass = "btnPanel";
        string newClass = oldClass;

        if (selec)
            newClass += "Active";
        else
            oldClass += "Active";

        ve.RemoveFromClassList(oldClass);
        ve.AddToClassList(newClass);
    }

    void seleccionPanel(ClickEvent e)
    {
        VisualElement panel = e.target as VisualElement;

        if (panelAct != null)
            EstadoPanel(panelAct, false);

        panelAct = panel;
        EstadoPanel(panelAct, true);
    }
}
