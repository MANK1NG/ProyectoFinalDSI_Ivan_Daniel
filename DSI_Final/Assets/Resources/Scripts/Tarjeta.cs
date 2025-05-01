using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.LudiqRootObjectEditor;

namespace PersonajeNS
{
    public class Tarjeta
    {
        Personaje miPersonaje;
        VisualElement tarjetaRoot;
        Label nombreLabel;

        Button btnUsar;
        Button btnBorrar;

        public Tarjeta(VisualElement tarjetaRoot, Personaje personaje)
        {
            this.tarjetaRoot = tarjetaRoot;
            this.miPersonaje = new Personaje(tarjetaRoot.Q("Personaje"), personaje);
            this.miPersonaje.CargarPersonaje(this.tarjetaRoot);

            nombreLabel = tarjetaRoot.Q<Label>("Nombre");
            nombreLabel.text = personaje.Nombre;

            btnUsar = tarjetaRoot.Q<Button>("Usar");
            btnBorrar = tarjetaRoot.Q<Button>("Borrar");

            btnUsar.RegisterCallback<ClickEvent>(Usar);
            btnBorrar.RegisterCallback<ClickEvent>(Borrar);
        }

        void Usar(ClickEvent e)
        {
            UIManager.CargarAvatar(miPersonaje);
            UIManager.MostrarPersonaje();
        }

        void Borrar(ClickEvent e)
        {
            BaseDatos.BorrarPersonaje(miPersonaje.Id);
            tarjetaRoot.parent.Remove(tarjetaRoot);
        }
    }
}