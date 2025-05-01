using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.FilterWindow;
using static UnityEngine.Rendering.DebugUI;

namespace PersonajeNS
{
    [Serializable]
    public struct Accesorio // Cada elemento que defina al personaje se compone de un identificador y un color
    {
        [SerializeField] int indice;
        [SerializeField] char color;

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

    [Serializable]
    public class Personaje
    {
        [SerializeField] int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [SerializeField] Accesorio casco; // Un identificador por elemento
        public Accesorio Casco
        {
            get { return casco; }
            set { casco = value; }
        }

        [SerializeField] Accesorio ojos;
        public Accesorio Ojos
        {
            get { return ojos; }
            set { ojos = value; }
        }

        [SerializeField] Accesorio arma;
        public Accesorio Arma
        {
            get { return arma; }
            set { arma = value; }
        }

        [SerializeField] Accesorio botas;
        public Accesorio Botas
        {
            get { return botas; }
            set { botas = value; }
        }

        [SerializeField] string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

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

        public Personaje(VisualElement personajeVE, Personaje personaje)
        {
            personajeElementos = new List<VisualElement>();
            foreach (VisualElement i in personajeVE.Children())
            {
                personajeElementos.Add(i);
            }
            id = personaje.Id;
            nombre = personaje.Nombre;
            casco = personaje.Casco;
            ojos = personaje.Ojos;
            arma = personaje.Arma;
            botas = personaje.Botas;
        }

        public void SetAccesorio(int tipo, int indice, char color)
        {
            if (indice == 0) // Índice 0 significa que el accesorio en este tipo (casco, ojos, arma o botas) es nulo
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

            switch (tipo) // Sobreescribimos la información
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

            personajeElementos[4 - tipo].style.backgroundImage = new StyleBackground(ResourceLoad.GetPersonajeAsset(ruta)); // Cargamos la imagen
        }

        public void CargarPersonaje(VisualElement root)
        {
            for (int tipo = 0; tipo < 4; tipo++)
            {
                string ruta = "";
                Accesorio accesorioT;

                switch (tipo) // Sobreescribimos la información
                {
                    case 0:
                        ruta += "C";
                        accesorioT = casco;
                        break;
                    case 1:
                        ruta += "O";
                        accesorioT = ojos;
                        break;
                    case 2:
                        ruta += "A";
                        accesorioT = arma;
                        break;
                    default:
                        ruta += "B";
                        accesorioT = botas;
                        break;
                }
                ruta += accesorioT.GetIndice() + "_" + accesorioT.GetColor();

                personajeElementos[4 - tipo].style.backgroundImage = new StyleBackground(ResourceLoad.GetPersonajeAsset(ruta)); // Cargamos la imagen
            }
        }
    }
}