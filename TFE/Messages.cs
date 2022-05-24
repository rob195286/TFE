using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFE
{
    public static class Messages
    {
        public static string VertexDontExist = "Un des vertices passé en paramètre n'existe pas dans le graphe : ";
        public static string NoPathFound = "Aucun chemin trouvé entre le noeud de départ et le noeud de la destination finale : ";
        public static string MaxPQcapacity = "Capacité maximum de la file atteinte, la file a été redimensionnée d'un facteur 2.";
    }
}
