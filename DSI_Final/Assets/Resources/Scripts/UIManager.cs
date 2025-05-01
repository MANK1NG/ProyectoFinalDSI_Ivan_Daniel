using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.LudiqRootObjectEditor;
using UnityEngine.UIElements;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using System.IO;
using UnityEditor;

namespace PersonajeNS
{
    public class UIManager : MonoBehaviour
    {
        VisualElement panel;
        VisualElement accesorios;
        VisualElement panelAct;
        VisualElement accesorioAct;

        static Personaje personaje;
        static VisualElement personajeVE;

        int paginasCasco = 3;
        int paginasOjos = 3;
        int paginasArma = 2;
        int paginasBotas = 3;
        int[] paginaActual;

        Button btnFlechaAbajo;
        Button btnFlechaArriba;
        static VisualElement menuAct;

        static VisualElement panelPersonaje;
        static VisualElement panelHistorial;
        VisualElement scrollZone;
        static Button btnPersonaje;
        static Button btnHistorial;

        public static List<Personaje> listaPersonajes = new List<Personaje>();

        VisualTreeAsset plantillaT;
        VisualElement botonGuardar;

        static TextField inputNombre;

        private void OnEnable()
        {
            plantillaT = Resources.Load<VisualTreeAsset>("UXML/Plantilla");

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            panel = root.Q<VisualElement>("Generales");
            accesorios = root.Q<VisualElement>("Accesorios");
            personajeVE = root.Q<VisualElement>("Personaje");
            personaje = new Personaje(personajeVE);

            int indexP = 0;
            foreach (VisualElement elementP in panel.Children()) // Interactividad paneles
            {
                elementP.userData = indexP; // Guarda el índice
                elementP.RegisterCallback<ClickEvent>(seleccionPanel);
                indexP++;
            }

            int indexA = 0;
            foreach (VisualElement elementA in accesorios.Children()) // Interactividad accesorios
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

            botonGuardar = root.Q<Button>("Guardar");
            botonGuardar.RegisterCallback<ClickEvent>(CrearPersonaje);

            inputNombre = root.Q<TextField>("Nombre");

            btnFlechaAbajo = root.Q<Button>("btnFlechaAbajo");
            btnFlechaArriba = root.Q<Button>("btnFlechaArriba");

            btnFlechaAbajo.clicked += PasarPaginaAbajo;
            btnFlechaArriba.clicked += PasarPaginaArriba;


            panelPersonaje = root.Q<VisualElement>("Pantalla1");
            panelHistorial = root.Q<VisualElement>("Pantalla2");
            scrollZone = root.Q<VisualElement>("ScrollZone");
            root.Q<VisualElement>("Global").RegisterCallback<WheelEvent>(Scroll);

            btnHistorial = root.Q<Button>("BtnHistorial");
            btnPersonaje = root.Q<Button>("BtnPersonaje");
            menuAct = btnPersonaje;
            EstadoMenu(btnPersonaje, true);

            btnPersonaje.clicked += MostrarPersonaje;
            btnHistorial.clicked += MostrarHistorial;

            paginaActual = new int[] { 1, 1, 1, 1 };

            panelAct = panel.Children().First(); // Iniciamos con el primer panel seleccionado
            EstadoPanel(panelAct, true);
            CargaPagina(0, 1);

            InitializeCollection();
        }

        #region panel
        void EstadoPanel(VisualElement ve, bool selec) // Cambia el aspecto del panel en función de si está activo
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
            EstadoPanel(panelAct, false);
            panelAct = panel;
            EstadoPanel(panelAct, true);

            CargaPagina((int)panel.userData, 1);
        }

        void EstadoAccesorio(VisualElement ve, bool selec) // Icono destacado de ser seleccionado
        {
            float alpha = selec ? 1.0f : 0.0f;

            ve.style.backgroundColor = new Color(0.7765f, 0.6196f, 0.8941f, alpha);
        }

        void seleccionAccesorio(ClickEvent e)
        {
            if (panelAct == null)
                return;
            VisualElement accesorio = e.target as VisualElement;
            int tipo = (int)panelAct.userData;

            if (accesorioAct != null)
            {
                if (accesorioAct != accesorio)
                {
                    EstadoAccesorio(accesorioAct, false); // Deseleccionamos icono anterior
                }
                else
                {
                    if (accesorioAct.resolvedStyle.backgroundColor.a == 0.0f) // De ser el mismo, invertimos su estado
                    {
                        EstadoAccesorio(accesorioAct, true);
                        personaje.SetAccesorio(tipo, paginaActual[tipo], (char)accesorioAct.userData);
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

            accesorioAct = accesorio; // Nuevo icono seleccionado
            EstadoAccesorio(accesorioAct, true);
            personaje.SetAccesorio(tipo, paginaActual[tipo], (char)accesorioAct.userData);
        }
        #endregion

        #region paginas
        protected void Scroll(WheelEvent e)
        {
            if (panelHistorial.resolvedStyle.display == DisplayStyle.Flex)
            {
                Length currY = panelHistorial.style.translate.value.y;
                float currentPercent = (currY.unit == LengthUnit.Percent) ? currY.value : 0f;

                float newY = Mathf.Clamp(currentPercent - e.delta.y, (float)(-54.63f * ((listaPersonajes.Count() - 1) / 4)), 0);
                panelHistorial.style.translate = new StyleTranslate(new Translate(0, new Length(newY, LengthUnit.Percent), 0));
            }
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
                    historico = personaje.Casco;
                    break;
                case 1:
                    historico = personaje.Ojos;
                    break;
                case 2:
                    historico = personaje.Arma;
                    break;
                default:
                    historico = personaje.Botas;
                    break;
            }

            foreach (VisualElement element in accesorios.Children())
            {
                element.style.backgroundImage = new StyleBackground(ResourceLoad.GetIcono(sprite[aux], tipo));
                if (historico.GetIndice() != paginaActual[tipo] || historico.GetColor() != (char)sprite[aux][2])
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
        #endregion

        #region menu
        public static void MostrarHistorial()
        {
            if (menuAct != btnHistorial)
            {
                panelHistorial.style.display = DisplayStyle.Flex;
                panelPersonaje.style.display = DisplayStyle.None;

                EstadoMenu(btnPersonaje, false);
                menuAct = btnHistorial;
                EstadoMenu(btnHistorial, true);
            }
        }
        public static void MostrarPersonaje()
        {
            if (menuAct != btnPersonaje)
            {
                panelPersonaje.style.display = DisplayStyle.Flex;
                panelHistorial.style.display = DisplayStyle.None;

                EstadoMenu(btnHistorial, false);
                menuAct = btnPersonaje;
                EstadoMenu(btnPersonaje, true);
            }
        }
        static void EstadoMenu(VisualElement ve, bool selec) // Icono destacado de ser seleccionado
        {
            if(selec)
            {
                string clase = ve.GetClasses().Last().ToString() + "Active";
                ve.AddToClassList(clase);
            }
            else
            {
                ve.RemoveFromClassList(ve.GetClasses().Last());
            }
        }
        #endregion

        #region tarjeta
        public static void CargarAvatar(Personaje newPersonaje)
        {
            personaje = new Personaje(personajeVE, newPersonaje);
            personaje.CargarPersonaje(personajeVE);
            inputNombre.value = personaje.Nombre;
        }
        void CrearPersonaje(ClickEvent e)
        {
            personaje.Nombre = inputNombre.value;
            if(listaPersonajes.Any())
            {
                personaje.Id = listaPersonajes.Last().Id + 1;
            }
            else
            {
                personaje.Id = 0;
            }
            listaPersonajes.Add(personaje);
            CrearTarjeta(personaje);
            personaje = new Personaje(personajeVE);
            personaje.CargarPersonaje(personajeVE);
            panelHistorial.style.translate = new StyleTranslate(new Translate(0, new Length((float)(-54.63f * ((listaPersonajes.Count() - 1) / 4)), LengthUnit.Percent), 0));
            MostrarHistorial();
            inputNombre.value = "Nombre";
        }

        void CrearTarjeta(Personaje personaje)
        {
            VisualElement tarjetaI = plantillaT.Instantiate();
            tarjetaI.AddToClassList("plantilla");
            panelHistorial.Add(tarjetaI);
            Tarjeta tarjeta = new Tarjeta(tarjetaI, personaje);

            string filePath = Path.Combine(Application.persistentDataPath, "lista_personajes.json");
            filePath = filePath.Replace("\\", "/"); //Forzado para evitar \ antes de lista_personajes.json (problema del Combine)
            string json = JsonHelperPersonaje.ToJson(listaPersonajes, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Guardado en: " + filePath);
        }

        void InitializeCollection()
        {
            listaPersonajes = BaseDatos.getData();
            foreach (var personaje in listaPersonajes)
            {
                CrearTarjeta(personaje);
            }
        }
        #endregion
    }
}