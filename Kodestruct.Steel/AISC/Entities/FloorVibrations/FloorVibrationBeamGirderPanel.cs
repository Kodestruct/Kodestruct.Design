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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.Data;
using Kodestruct.Steel.AISC.Entities.Enums.FloorVibrations;
using Kodestruct.Steel.Properties;

namespace Kodestruct.Steel.AISC.Entities.FloorVibrations
{
    public partial class FloorVibrationBeamGirderPanel
    {




        /// <summary>
        /// 
        /// </summary>
        /// <param name="I_j">Joist moment of inertia</param>
        /// <param name="S_j">Joist spacing</param>
        /// <returns></returns>
        public double GetDistributedJoistMomentOfInertia(double I_j, double S_j)
        {
            double D_j = I_j / S_j;
            return D_j;
        }


        public double GetDistributedGirderMomentOfInertia(double I_g, double L_j)
        {
            double D_g = I_g / L_j;
            return D_g;
        }

        /// <summary>
        /// Effective slab thickness
        /// </summary>
        /// <param name="h_solid">Thickness of solid concrete fill over deck</param>
        /// <param name="h_rib">Deck rib height</param>
        /// <param name="w_r">Average width of rib</param>
        /// <param name="s_r">Rib spacing</param>
        /// <param name="DeckCondition"></param>
        /// <returns></returns>
        public double Get_d_e(double h_solid,double h_rib,double w_r,double s_r)
        {

            //BECAUSE DECK IS SPANNING IN ITS STRONG DIRECTION ASSUMED PERPENDICULAR
            //TO JOISTS, DECK CONDITION IS NOT ACCOUNTED FOR


            //switch (DeckCondition)
            //{
                //case DeckAtBeamCondition.NoDeck:
                //    return h_rib + h_solid;
                //    break;
                //case DeckAtBeamCondition.Parallel:
                    double t_ribEff = (w_r * h_rib) / s_r;
                    return t_ribEff + h_solid;
                //    break;
                //case DeckAtBeamCondition.Perpendicular:
                //    return h_solid;
                //    break;
                //default:
                //    return 0;
                //    break;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d_e">Slab effective thickness</param>
        /// <param name="n"> Modular ratio (include dynamic increase)</param>
        /// <returns></returns>
        public double GetDistributedSlabMomentOfInertia(double n, double d_e)
        {
            //double D_s = (((12.0) / (n))) * (((Math.Pow(d_e, 3)) / (12.0)));
            //D_s in inches / in
            double D_s = ((1.0 / (n))) * (((Math.Pow(d_e, 3)) / (12.0)));
            return D_s;
        }




        string floorVibrationOccupancyId;

        public double GetAccelerationLimit(string FloorVibrationOccupancyId)
        {
            floorVibrationOccupancyId = FloorVibrationOccupancyId; //store value so that the retrieve function works 
            return Limits.a_gRatio; 
        }

        public double GetAccelerationRatio(double f_n, double W, double beta, string FloorSeviceOccupancyId )
        {
            floorVibrationOccupancyId = FloorSeviceOccupancyId;   //store value so that the retrieve function works
            double P_o = GetP_oFromOccupancyId(FloorSeviceOccupancyId);
            double apTo_g = P_o * Math.Exp(-0.35 * f_n) / (beta * W);
            return apTo_g;
        }

        private double GetP_oFromOccupancyId(string FloorVibrationOccupancyId)
        {
            floorVibrationOccupancyId = FloorVibrationOccupancyId;   //store value so that the retrieve function works
            return Limits.P_o / 1000.0; //convert to kip units consistent with the rest of the library
        }

        public double GetCombinedModeFundamentalFrequency(double Delta_j, double Delta_g, double Delta_c=0)
        {
            double g =386.0;
            double f_n=0.18*Math.Sqrt(((g) / (Delta_j+Delta_g+Delta_c)));
            return f_n;
        }

        public double GetFloorModalDampingRatio(List<string> Components)
        {
            #region Read Damping Data

            var SampleValue = new { ComponentId = "", DampingRatio = 0.0}; // sample
            var DampingVals = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.AISC_DG11_ModalDamping))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Length == 2)
                    {
                        string _ComponentId = (string)Vals[0];
                        double _DampingRatio = double.Parse((string)Vals[1]);
                        
                        DampingVals.Add
                        (new
                        {
                            ComponentId = _ComponentId,
                            DampingRatio = _DampingRatio,
                        }

                        );
                    }
                }

            }

            #endregion

            
            List<double> damping = new List<double>();
            foreach (var CompId in Components)
            {
                if (DampingVals.Where(c => c.ComponentId ==CompId).Any())
	                {
		                        var betaEntry = DampingVals.First(o => o.ComponentId == CompId);
                                damping.Add(betaEntry.DampingRatio);
	                }
                else
                {
                    throw new Exception("At least one occupancy type was not found.");
                }

            }
            double betaTotal = damping.Sum();
            return betaTotal;
        }

        private VibrationLimits limits;

        private VibrationLimits Limits
        {
            get {

                if (limits == null)
                {
                    limits = ReadLimitData(floorVibrationOccupancyId);
                }
                return limits; }

        }

        private VibrationLimits ReadLimitData(string FloorVibrationOccupancyId)
        {
            #region Read Limit Data

            var SampleValue = new { OccId = "", ConstantForce = 0.0, Limit = 0.0}; // sample
            var VibrationLimitVals = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.AISC_DG11_VibrationLimits))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Length == 3)
                    {
                        string _OccId = (string)Vals[0];
                        double _ConstantForce =double.Parse((string)Vals[1]);
                        double _Limit = double.Parse((string)Vals[2]);
                        VibrationLimitVals.Add
                        (new
                        {
                            OccId = _OccId,
                            ConstantForce = _ConstantForce,
                            Limit = _Limit,
                        }

                        );
                    }
                }

            }

            #endregion

            var VibrationLimEntryData = VibrationLimitVals.First(l => l.OccId == FloorVibrationOccupancyId);

            if (VibrationLimEntryData!=null)
            {
                return new VibrationLimits()
                {
                    P_o = VibrationLimEntryData.ConstantForce,
                    a_gRatio = VibrationLimEntryData.Limit
                };
            }
            else
            {
                throw new Exception("Specified occupancy typenot found, please checkinput string.");
            }
        }
        

        class VibrationLimits
        {
            public double P_o { get; set; }
            public double a_gRatio { get; set; }
        }
    }
}
