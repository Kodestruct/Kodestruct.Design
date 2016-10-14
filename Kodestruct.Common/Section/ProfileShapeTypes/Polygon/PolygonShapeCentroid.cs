#region Copyright
   /*Copyright (C) 2015 Konstantin Udilovich

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
   */
#endregion
 
using System;
using System.Collections.Generic;
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Common.Section.General
{
    public partial class PolygonShape : SectionBase, ISliceableSection, IMoveableSection
    {
        //Centroid is correctly located ONLY for non self intersecting polygon
        //http://en.wikipedia.org/wiki/Centroid#Centroid_of_polygon
        //http://stackoverflow.com/questions/9815699/how-to-calculate-centroid

    static Point2D Compute2DPolygonCentroid(List<Point2D> vertices)
    {
        Point2D centroid = new Point2D( 0.0, 0.0 );
        double signedArea = 0.0;
        double x0 = 0.0; // Current vertex X
        double y0 = 0.0; // Current vertex Y
        double x1 = 0.0; // Next vertex X
        double y1 = 0.0; // Next vertex Y
        double a = 0.0;  // Partial signed area

        // For all vertices except last
        int i=0;
        for (i = 0; i < vertices.Count - 1; ++i)
        {
            x0 = vertices[i].X;
            y0 = vertices[i].Y;
            x1 = vertices[i+1].X;
            y1 = vertices[i+1].Y;
            a = x0*y1 - x1*y0;
            signedArea += a;
            centroid.X += (x0 + x1)*a;
            centroid.Y += (y0 + y1)*a;
        }

        // Do last vertex
        x0 = vertices[i].X;
        y0 = vertices[i].Y;
        x1 = vertices[0].X;
        y1 = vertices[0].Y;
        a = x0*y1 - x1*y0;
        signedArea += a;
        centroid.X += (x0 + x1)*a;
        centroid.Y += (y0 + y1)*a;

        signedArea *= 0.5;
        centroid.X /= (6*signedArea);
        centroid.Y /= (6*signedArea);

        return centroid;
    }

    }
}
