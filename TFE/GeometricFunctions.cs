using System;


namespace TFE
{
    public static class GeometricFunctions
    {
        /// <summary>
        ///     1) 120 / 3600 = 1 / 30 -> km / s 
        ///     2) 100 / 3 -> m / s
        ///     3) 100 / 3 = 33,33 -> m / s
        /// </summary>
        public static double CarSpeed = 33.33; 
        /// <summary>
        ///     Permet de calculer la distance entre deux points sur une sphère, prit d'ici : https://fr.wikipedia.org/wiki/Formule_de_haversine
        /// </summary>
        /// <param name="latitudeSource"></param>
        /// <param name="longitudeSource"></param>
        /// <param name="latitudeTarget"></param>
        /// <param name="longitudeTarget"></param>
        /// <returns></returns>
        private static double Haversine(double latitudeSource,
                                        double longitudeSource,
                                        double latitudeTarget,
                                        double longitudeTarget)
        {
            return 2*6371*Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(((Math.PI*latitudeTarget/180)-(Math.PI * latitudeSource/180))/2), 2) + 
                                (Math.Cos(Math.PI*latitudeSource/180)*Math.Cos(Math.PI*latitudeTarget/180)*Math.Pow(Math.Sin(((Math.PI*longitudeTarget/180) - 
                                                                              (Math.PI*longitudeSource/180)) /2), 2))));
        }

        public static double TimeAsCrowFlies(GraphNode currentNode, GraphNode targetNode)
        {
            return GeometricFunctions.Haversine(currentNode.latitude,
                                                currentNode.longitude,
                                                targetNode.latitude,
                                                targetNode.longitude) / CarSpeed;
        }

    }
}
