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
    VisualElement accesorioAct;

    Personaje personaje;

    int paginasCasco = 3;
    int paginasOjos = 3;
    int paginasArma = 2;
    int paginasBotas = 3;
    int[] paginaActual;

    Button btnFlechaAbajo;
    Button btnFlechaArriba;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        panel = root.Q<VisualElement>("Generales");
        accesorios = root.Q<VisualElement>("Accesorios");
        personaje = new Personaje(root.Q<VisualElement>("Personaje"));

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
            switch (indexA) // Guarda el color
            {
                case 0:
                    elementA.userData = 'R';
                    break;
                case 1:
                    elementA.userData = 'A';
                    break;
                case 2:
                    elementA.userData = 'V';
                    break;
            }
            elementA.RegisterCallback<ClickEvent>(seleccionAccesorio);
            indexA++;
        }

         btnFlechaAbajo = root.Q<Button>("btnFlechaAbajo");
         btnFlechaArriba = root.Q<Button>("btnFlechaArriba");

        btnFlechaAbajo.clicked += PasarPaginaAbajo;
        btnFlechaArriba.clicked += PasarPaginaArriba;

        paginaActual = new int[] { 1, 1, 1, 1 };
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
                if (accesorioAct.resolvedStyle.backgroundColor.a == 0.0f)
                {
                    EstadoAccesorio(accesorioAct, true);
                    personaje.SetAccesorio((int)panelAct.userData, 1, (char)accesorioAct.userData);
                    return;
                }
                else
                {
                    EstadoAccesorio(accesorioAct, false);
                    personaje.SetAccesorio((int)panelAct.userData, 0, (char)accesorioAct.userData);
                    return;
                }
            }
        }

        accesorioAct = accesorio;
        EstadoAccesorio(accesorioAct, true);
        personaje.SetAccesorio((int)panelAct.userData, 1, (char)accesorioAct.userData);
    }

    void CargaPagina(int tipo, int pagina)
    {
        int paginasTotales = GetPaginasTotales(tipo);
        if (pagina < 1) pagina = 1;
        if (pagina > paginasTotales) pagina = paginasTotales;

        paginaActual[tipo] = pagina;

        string[] sprite = { pagina.ToString() + "_R", pagina.ToString() + "_A", pagina.ToString() + "_V" };
        int aux = 0;
        Accesorio historico;
        switch (tipo)
        {
            case 0:
                historico = personaje.casco;
                break;
            case 1:
                historico = personaje.ojos;
                break;
            case 2:
                historico = personaje.arma;
                break;
            default:
                historico = personaje.botas;
                break;
        }

        foreach (VisualElement element in accesorios.Children())
        {
            element.style.backgroundImage = new StyleBackground(ResourceLoad.GetIcono(sprite[aux], tipo));
            if (historico.GetIndice() != 1 || historico.GetColor() != (char)sprite[aux][2])
                EstadoAccesorio(element, false);
            else
            {
                EstadoAccesorio(element, true);
                accesorioAct = element;
            }
            aux++;
        }

        ActualizarFlechas();
    }


    void ActualizarFlechas()
    {
        int tipo = (int)panelAct.userData;
        int paginasTotales = GetPaginasTotales(tipo);

        if (paginaActual[tipo] == 1)
        {
            btnFlechaArriba.style.display = DisplayStyle.None;
        }
        else
        {
            btnFlechaArriba.style.display = DisplayStyle.Flex;
        }

        if (paginaActual[tipo] == paginasTotales)
        {
            btnFlechaAbajo.style.display = DisplayStyle.None;
        }
        else
        {
            btnFlechaAbajo.style.display = DisplayStyle.Flex;
        }

    }

    void PasarPaginaAbajo()
    {
        int tipo = (int)panelAct.userData;
        int paginasTotales = GetPaginasTotales(tipo);

        if (paginaActual[tipo] < paginasTotales)
        {
            paginaActual[tipo]++;
            CargaPagina(tipo, paginaActual[tipo]);
            ActualizarFlechas();
        }
    }

    void PasarPaginaArriba()
    {
        int tipo = (int)panelAct.userData;

        if (paginaActual[tipo] > 1)
        {
            paginaActual[tipo]--;
            CargaPagina(tipo, paginaActual[tipo]);
            ActualizarFlechas();
        }
    }

    int GetPaginasTotales(int tipo)
    {
        int paginas = 1;

        switch (tipo)
        {
            case 0:
                paginas = paginasCasco;
                break;
            case 1:
                paginas = paginasOjos;
                break;
            case 2:
                paginas = paginasArma;
                break;
            case 3:
                paginas = paginasBotas;
                break;
        }

        return paginas;
    }
}