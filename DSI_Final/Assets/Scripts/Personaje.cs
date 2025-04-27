using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.FilterWindow;
using static UnityEngine.Rendering.DebugUI;

public struct Accesorio
{
    int indice;
    char color;

    public Accesorio(int indice = 0, char color = 'R')
    {
        this.indice = indice;
        this.color = color;
    }

    public int GetIndice()
    {
        return indice;
    }

    public void SetIndice(int newIndice)
    {
        indice = newIndice;
    }

    public char GetColor()
    {
        return color;
    }

    public void SetColor(char newColor)
    {
        color = newColor;
    }
}

public class Personaje
{
    public Accesorio casco;
    public Accesorio ojos;
    public Accesorio arma;
    public Accesorio botas;

    List<VisualElement> personajeElementos;

    public Personaje(VisualElement personajeVE, Accesorio newCasco = default, Accesorio newOjos = default, Accesorio newArma = default, Accesorio newBotas = default)
    {
        personajeElementos = new List<VisualElement>();
        foreach (VisualElement i in personajeVE.Children())
        {
            personajeElementos.Add(i);
        }
        casco = newCasco;
        ojos = newOjos;
        arma = newArma;
        botas = newBotas;
    }

    public void SetAccesorio(int tipo, int indice, char color)
    {
        if (indice == 0)
        {
            personajeElementos[4 - tipo].style.backgroundImage = new StyleBackground();
            switch (tipo)
            {
                case 0:
                    casco.SetIndice(indice);
                    break;
                case 1:
                    ojos.SetIndice(indice);
                    break;
                case 2:
                    arma.SetIndice(indice);
                    break;
                case 3:
                    botas.SetIndice(indice);
                    break;
                default:
                    break;
            }
            return;
        }

        string ruta = "";

        switch (tipo)
        {
            case 0:
                ruta += "C";
                casco.SetIndice(indice);
                casco.SetColor(color);
                break;
            case 1:
                ruta += "O";
                ojos.SetIndice(indice);
                ojos.SetColor(color);
                break;
            case 2:
                ruta += "A";
                arma.SetIndice(indice);
                arma.SetColor(color);
                break;
            case 3:
                ruta += "B";
                botas.SetIndice(indice);
                botas.SetColor(color);
                break;
            default:
                break;
        }
        ruta += indice + "_" + color;

        personajeElementos[4-tipo].style.backgroundImage = new StyleBackground(ResourceLoad.GetPersonajeAsset(ruta));
    }
}


