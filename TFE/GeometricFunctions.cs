using System;


namespace TFE
{
    public static class GeometricFunctions
    {
        /// <summary>
        ///     Permet de calculer la distance entre deux points sur une sphère, prit d'ici : https://fr.wikipedia.org/wiki/Formule_de_haversine
        /// </summary>
        /// <param name="latitudeSource"></param>
        /// <param name="longitudeSource"></param>
        /// <param name="latitudeTarget"></param>
        /// <param name="longitudeTarget"></param>
        /// <returns>
        ///     Retourne une distance en km.
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
        ///     Permet de calculer le temps à vol d'oiseau entre deux géoloacalisations à partir de noeuds passés en entrer.
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="targetNode"></param>
        /// <param name="heuristicFactor"></param>
        /// <returns>
        ///     Retourne le coût équivalent au .
        /// </returns>
        public static double EuclideanDistanceCostFromTo(Vertex currentNode, Vertex targetNode, double heuristicFactor = 0.008988227930074038)
        {
            //heuristicFactor = 111257;
            return Haversine(currentNode.latitude,
                            currentNode.longitude,
                            targetNode.latitude,
                            targetNode.longitude) * heuristicFactor;
        }
    }
}
