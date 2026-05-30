using System.Collections.Generic;
using RegistroEstudiantes.Modelos;

namespace RegistroEstudiantes
{
    // Nodo del árbol
    public class NodoEstudiante
    {
        public Estudiante Dato { get; set; }
        public NodoEstudiante? Izquierdo { get; set; }
        public NodoEstudiante? Derecho { get; set; }

        public NodoEstudiante(Estudiante estudiante)
        {
            Dato = estudiante;
            Izquierdo = null;
            Derecho = null;
        }
    }

    // Árbol Binario de Búsqueda ordenado por Carnet
    public class ArbolBST
    {
        private NodoEstudiante? _raiz;

        public ArbolBST()
        {
            _raiz = null;
        }

        // Insertar estudiante ordenado por Carnet
        public void Insertar(Estudiante estudiante)
        {
            _raiz = InsertarRec(_raiz, estudiante);
        }

        private NodoEstudiante InsertarRec(NodoEstudiante? nodo, Estudiante estudiante)
        {
            if (nodo == null)
                return new NodoEstudiante(estudiante);

            int cmp = string.Compare(estudiante.Carnet, nodo.Dato.Carnet);
            if (cmp < 0)
                nodo.Izquierdo = InsertarRec(nodo.Izquierdo, estudiante);
            else if (cmp > 0)
                nodo.Derecho = InsertarRec(nodo.Derecho, estudiante);

            return nodo;
        }

        // Recorrido inorden: devuelve lista ordenada por Carnet
        public List<Estudiante> ObtenerInorden()
        {
            var lista = new List<Estudiante>();
            InordenRec(_raiz, lista);
            return lista;
        }

        private void InordenRec(NodoEstudiante? nodo, List<Estudiante> lista)
        {
            if (nodo == null) return;
            InordenRec(nodo.Izquierdo, lista);
            lista.Add(nodo.Dato);
            InordenRec(nodo.Derecho, lista);
        }

        // Buscar por Carnet
        public Estudiante? BuscarPorCarnet(string carnet)
        {
            return BuscarCarnetRec(_raiz, carnet);
        }

        private Estudiante? BuscarCarnetRec(NodoEstudiante? nodo, string carnet)
        {
            if (nodo == null) return null;

            int cmp = string.Compare(carnet, nodo.Dato.Carnet);
            if (cmp == 0) return nodo.Dato;
            if (cmp < 0) return BuscarCarnetRec(nodo.Izquierdo, carnet);
            return BuscarCarnetRec(nodo.Derecho, carnet);
        }

        // Buscar por ID
        public Estudiante? BuscarPorId(int id)
        {
            return BuscarIdRec(_raiz, id);
        }

        private Estudiante? BuscarIdRec(NodoEstudiante? nodo, int id)
        {
            if (nodo == null) return null;
            if (nodo.Dato.IdEstudiante == id) return nodo.Dato;

            var izq = BuscarIdRec(nodo.Izquierdo, id);
            if (izq != null) return izq;
            return BuscarIdRec(nodo.Derecho, id);
        }

        // Cargar lista completa al árbol
        public void CargarDesdelista(List<Estudiante> estudiantes)
        {
            _raiz = null;
            foreach (var e in estudiantes)
                Insertar(e);
        }

        // Limpiar árbol
        public void Limpiar()
        {
            _raiz = null;
        }
    }
}