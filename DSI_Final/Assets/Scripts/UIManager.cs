using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.LudiqRootObjectEditor;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    VisualElement panel;
    VisualElement accesorios;
    VisualElement panelAct;
    bool accesorioActSel = false;
    VisualElement accesorioAct;

    int paginasCasco = 3;
    int paginasOjos = 3;
    int paginasArma = 2;
    int paginasBotas = 3;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        panel = root.Q<VisualElement>("Generales");
        accesorios = root.Q<VisualElement>("Accesorios");

        int indexP = 0;
        foreach (VisualElement elementP in panel.Children())
        {
            elementP.userData = indexP; // Guarda el índice
            elementP.RegisterCallback<ClickEvent>(seleccionPanel);
            indexP++;
        }

        int indexA = 0;
        foreach (VisualElement elementA in accesorios.Children())
        {
            elementA.userData = indexA; // Guarda el índice
            elementA.RegisterCallback<ClickEvent>(seleccionAccesorio);
            indexA++;
        }
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

        CargaPagina((int)panel.userData, 1);
    }

    void EstadoAccesorio(VisualElement ve, bool selec)
    {
        float alpha = selec ? 1.0f : 0.0f;

        ve.style.backgroundColor = new Color(0.7765f, 0.6196f, 0.8941f, alpha);
    }

    void seleccionAccesorio(ClickEvent e)
    {
        if (panelAct == null)
            return;
        VisualElement accesorio = e.target as VisualElement;

        if (accesorioAct != null)
        {
            if (accesorioAct != accesorio)
            {
                EstadoAccesorio(accesorioAct, false);
            }
            else
            {
                if (!accesorioActSel)
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

    void CargaPagina(int tipo, int pagina)
    {
        string[] sprite = { pagina.ToString() + "_R", pagina.ToString() + "_A", pagina.ToString() + "_V" };
        foreach (VisualElement element in accesorios.Children())
        {
            element.style.backgroundImage = new StyleBackground(ResourceLoad.GetIcono(sprite[(int)element.userData], tipo));
        }
    }
}
