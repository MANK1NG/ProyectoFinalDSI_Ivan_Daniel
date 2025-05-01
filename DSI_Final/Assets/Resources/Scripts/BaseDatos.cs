using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Unity.VisualScripting.LudiqRootObjectEditor;

namespace PersonajeNS
{
    public class BaseDatos : MonoBehaviour
    {
        private static readonly string filePath = Path.Combine(Application.persistentDataPath, "lista_personajes.json");

        public static List<Personaje> getData()
        {
            List<Personaje> datos = new List<Personaje>();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                datos = JsonHelperPersonaje.FromJson<Personaje>(json);
                Debug.Log("JSON cargado.");
            }
            else
            {
                Debug.Log("No se pudo encontrar JSON.");
            }

            return datos;
        }

        public static void BorrarPersonaje(int id)
        {
            if (!File.Exists(filePath)) return;

            string json = File.ReadAllText(filePath);
            List<Personaje> lista = JsonHelperPersonaje.FromJson<Personaje>(json);

            // Buscar y eliminar
            lista.RemoveAll(p => p.Id == id);
            UIManager.listaPersonajes = lista;

            // Guardar de nuevo
            string nuevoJson = JsonHelperPersonaje.ToJson(lista, true);
            File.WriteAllText(filePath, nuevoJson);

            Debug.Log("Personaje eliminado: " + id);
        }
    }

}
