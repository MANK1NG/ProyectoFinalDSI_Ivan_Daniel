using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.LudiqRootObjectEditor;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    List<VisualElement> panel;
    List<VisualElement> accesorios;
    VisualElement panelAct;
    bool accesorioActSel = false;
    VisualElement accesorioAct;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        var panel = root.Q<VisualElement>("Generales");
        var accesorios = root.Q<VisualElement>("Accesorios");

        foreach (VisualElement elementP in panel.Children())
            elementP.RegisterCallback<ClickEvent>(seleccionPanel);

        foreach (VisualElement elementA in accesorios.Children())
            elementA.RegisterCallback<ClickEvent>(seleccionAccesorio);
    }

    void EstadoPanel(VisualElement ve, bool selec)
    {
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

    void EstadoAccesorio(VisualElement ve, bool selec)
    {
        float alpha = selec ? 1.0f : 0.0f;

        ve.style.backgroundColor = new Color(0.7765f, 0.6196f, 0.8941f, alpha);
    }

    void seleccionAccesorio(ClickEvent e)
    {
        VisualElement accesorio = e.target as VisualElement;

        if (accesorioAct != null)
        {
            if (accesorioAct != accesorio)
            {
                EstadoAccesorio(accesorioAct, false);
            }
            else
            {
                if(!accesorioActSel)
                {
                    EstadoAccesorio(accesorioAct, true);
                    accesorioActSel = true;
                    return;
                }
                else
                {
                    EstadoAccesorio(accesorioAct, false);
                    accesorioActSel = false;
                    return;
                }
            }
        }

        accesorioAct = accesorio;
        accesorioActSel = true;
        EstadoAccesorio(accesorioAct, true);
    }
}
