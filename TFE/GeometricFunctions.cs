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

        public static double TimeAsCrowFlies(GraphNode currentNode, GraphNode targetNode, double carSpeed = 120)
        {
            /// <summary>
            ///     1) 120 / 3600 = 1 / 30 -> km / s   | speed * 1000 / 3600 -> m / s
            ///     2) 1 * 1000 / 30       -> m / s    | speed * 10 / 36     -> m / s
            ///     3) 100 / 3 = 33,33     -> m / s    | speed * 5 / 18      -> m / s
            /// </summary>
            carSpeed = carSpeed == 120 ? 33.33 : carSpeed * 5 / 18; 
            return GeometricFunctions.Haversine(currentNode.latitude,
                                                currentNode.longitude,
                                                targetNode.latitude,
                                                targetNode.longitude) / carSpeed;
        }

    }
}
