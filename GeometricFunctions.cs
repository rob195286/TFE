using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFE
{
    public static class GeometricFunctions
    {
        public static double Haversine(double latitudeSource,
                                                     double longitudeSource,
                                                     double latitudeTarget,
                                                     double longitudeTarget)
        {
            return 2*6371*Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(((Math.PI*latitudeTarget/180)-(Math.PI * latitudeSource/180))/2), 2) + 
                                (Math.Cos(Math.PI*latitudeSource/180)*Math.Cos(Math.PI*latitudeTarget/180)*Math.Pow(Math.Sin(((Math.PI*longitudeTarget/180) - 
                                                                              (Math.PI*longitudeSource/180)) /2), 2))));
        }

    }
}
