#region Copyright
   /*Copyright (C) 2015 Kodestruct Inc

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

namespace Kodestruct.Common.Section.Interfaces
{
    public interface ISectionAngle : ISection, IWeakAxisCloneable
    {
        double d { get;  }
        double t { get;  }
        double b { get;  }

        double I_w { get; }
        double I_z { get; }

        double S_w { get; }
        double S_z { get; }

        double r_w { get; }
        double r_z { get; }

        double Angle_alpha { get; }

        double beta_w { get; }

        AngleOrientation AngleOrientation { get;}
        AngleRotation AngleRotation { get; }
    }
}
