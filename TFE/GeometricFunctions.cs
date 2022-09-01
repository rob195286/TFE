using System;


namespace TFE
{
    public static class GeometricFunctions
    {
        /// <summary>
        ///     Permet de calculer la distance entre deux points sur une sphère, source : https://fr.wikipedia.org/wiki/Formule_de_haversine
        /// </summary>
        /// <param name="latitudeSource"> Latitude du premier vertex à partir duquel on veut en connaître la distance. </param>
        /// <param name="longitudeSource"> Longitude du premier vertex à partir duquel on veut en connaître la distance. </param>
        /// <param name="latitudeTarget"> Latitude du second vertex vers lequel on veut en connaître la distance. </param>
        /// <param name="longitudeTarget"> Longitude du second vertex vers lequel on veut en connaître la distance. </param>
        /// <returns>
        ///     Retourne une distance en km séparant les coodonnées lat/lon du vertex source et target.
        /// </returns>
        private static double Haversine(double latitudeSource,
                                        double longitudeSource,
                                        double latitudeTarget,
                                        double longitudeTarget)
        {
            return 2 * 6371 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(((Math.PI*latitudeTarget/180)-(Math.PI * latitudeSource/180))/2), 2) + 
                                (Math.Cos(Math.PI*latitudeSource/180)*Math.Cos(Math.PI*latitudeTarget/180)*Math.Pow(Math.Sin(((Math.PI*longitudeTarget/180) - 
                                                                              (Math.PI*longitudeSource/180))/2), 2))));
        }
        /// <summary>
        ///     Permet de calculer le temps à vol d'oiseau entre deux géoloacalisations à partir de noeuds passés en entrer sur
        ///         base d'une estimation heuristique.
        /// </summary>
        /// <param name="currentNode"> Noeud dont on veut connaître la distance par rapport au noeud de destination.</param>
        /// <param name="targetNode"> Noeud de destination. </param>
        /// <param name="heuristicFactor"> Nombre permettant de convertir en coût la distance renvoyé par la fonctoin de haversine.</param>
        /// <returns>
        ///     Retourne le coût équivalent par rapport à la distance à vol d'oiseau trouvée.
        /// </returns>
        public static double EuclideanDistanceCostFromTo(Vertex currentNode, Vertex targetNode, double heuristicFactor = 0.008988227930074038)
        {
            return Haversine(currentNode.latitude,
                            currentNode.longitude,
                            targetNode.latitude,
                            targetNode.longitude) * heuristicFactor;
        }
    }
}
