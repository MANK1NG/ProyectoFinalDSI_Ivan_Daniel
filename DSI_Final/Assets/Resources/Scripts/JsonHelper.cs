using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersonajeNS
{
    public static class JsonHelperPersonaje
    {
        public static List<Personaje> FromJson<Personaje>(string json)
        {
            ListaPersonaje<Personaje> listaPersonaje = JsonUtility.FromJson<ListaPersonaje<Personaje>>(json);
            return listaPersonaje.Personajes;
        }

        public static string ToJson<Personaje>(List<Personaje> lista)
        {
            ListaPersonaje<Personaje> listaPersonaje = new ListaPersonaje<Personaje>();
            listaPersonaje.Personajes = lista;
            return JsonUtility.ToJson(listaPersonaje);
        }

        public static string ToJson<Personaje>(List<Personaje> lista, bool prettyPrint)
        {
            ListaPersonaje<Personaje> listaPersonaje = new ListaPersonaje<Personaje>();
            listaPersonaje.Personajes = lista;
            return JsonUtility.ToJson(listaPersonaje, prettyPrint);
        }

        [Serializable]
        private class ListaPersonaje<Personaje>
        {
            public List<Personaje> Personajes;
        }
    }
}