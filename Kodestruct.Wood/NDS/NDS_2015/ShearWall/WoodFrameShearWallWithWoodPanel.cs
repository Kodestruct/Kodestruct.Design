using Kodestruct.Common.Data;
using Kodestruct.Wood.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kodestruct.Wood.NDS.NDS2015.ShearWall
{
    public class WoodFrameShearWallWithWoodPanel
    {
        public (double v, string Fastener, string FastenerPenetration, double G) GetShearUnitStrength(string PanelMaterial , string Grade, string PanelThickness, double NailSpacing,
            bool IsSeismic, WoodPanelType WoodPanelType)
        {

   
            #region Read Occupancy Data

            var SampleValue = new { PanelMaterial = "", Grade = "", Thickness = "", FastenerPenetration = "", Fastener = "", 
                v_Seism6 = 0.0, G_Seism6Osb = 0.0, G_Seism6Ply = 0.0, 
                v_Seism4 = 0.0, G_Seism4Osb = 0.0, G_Seism4Ply = 0.0, 
                v_Seism3 = 0.0, G_Seism3Osb = 0.0, G_Seism3Ply = 0.0, 
                v_Seism2 = 0.0, G_Seism2Osb = 0.0, G_Seism2Ply = 0.0, 
                v_Wind6 =  0.0, v_Wind4 = 0.0, v_Wind3 = 0.0, v_Wind2 = 0.0 }; // sample
            var shearWallVals = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.SDPWS2015Table4_3A))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Length == 21)
                    {
                       string _PanelMaterial = Vals[0];
                       string _Grade = Vals[1];
                       string _Thickness = Vals[2];
                       string _FastenerPenetration = Vals[3];
                       string _Fastener =  Vals[4];
                       double _v_Seism6 = Double.Parse(Vals[5]);
                       double _G_Seism6Osb = Double.Parse(Vals[6]);
                       double _G_Seism6Ply = Double.Parse(Vals[7]);
                       double _v_Seism4 = Double.Parse(Vals[8]);
                       double _G_Seism4Osb = Double.Parse(Vals[9]);
                       double _G_Seism4Ply = Double.Parse(Vals[10]);
                       double _v_Seism3 = Double.Parse(Vals[11]);
                       double _G_Seism3Osb = Double.Parse(Vals[12]);
                       double _G_Seism3Ply = Double.Parse(Vals[13]);
                       double _v_Seism2 = Double.Parse(Vals[14]);
                       double _G_Seism2Osb = Double.Parse(Vals[15]);
                       double _G_Seism2Ply = Double.Parse(Vals[16]);
                       double _v_Wind6 = Double.Parse(Vals[17]);
                       double _v_Wind4 = Double.Parse(Vals[18]);
                       double _v_Wind3 = Double.Parse(Vals[19]);
                       double _v_Wind2 = Double.Parse(Vals[20]);

                        shearWallVals.Add
                        (new
                        {
                            PanelMaterial = _PanelMaterial,
                            Grade = _Grade,
                            Thickness = _Thickness,
                            FastenerPenetration = _FastenerPenetration,
                            Fastener = _Fastener,
                            v_Seism6 = _v_Seism6,
                            G_Seism6Osb = _G_Seism6Osb,
                            G_Seism6Ply = _G_Seism6Ply,
                            v_Seism4 = _v_Seism4,
                            G_Seism4Osb = _G_Seism4Osb,
                            G_Seism4Ply = _G_Seism4Ply,
                            v_Seism3 = _v_Seism3,
                            G_Seism3Osb = _G_Seism3Osb,
                            G_Seism3Ply = _G_Seism3Ply,
                            v_Seism2 = _v_Seism2,
                            G_Seism2Osb = _G_Seism2Osb,
                            G_Seism2Ply = _G_Seism2Ply,
                            v_Wind6 = _v_Wind6,
                            v_Wind4 = _v_Wind4,
                            v_Wind3 = _v_Wind3,
                            v_Wind2 = _v_Wind2
                        }

                        );
                    }
                }

            }
            #endregion


            var FoundValues = shearWallVals.Where(s => s.PanelMaterial == PanelMaterial && s.Grade == Grade && s.Thickness == PanelThickness).FirstOrDefault();
            if (FoundValues!=null)
            {
                double v=0;
                double G_a = 0;
                string Fastener = FoundValues.Fastener;
                string FastenerPenetration = FoundValues.FastenerPenetration;

                if (IsSeismic)
                {
                    if (NailSpacing == 6)
                    {
                        v = FoundValues.v_Seism6;
                        G_a = WoodPanelType == WoodPanelType.OSB ? FoundValues.G_Seism6Osb : FoundValues.G_Seism6Ply;

                    }
                    if (NailSpacing == 4)
                    {
                        v = FoundValues.v_Seism4;
                        G_a = WoodPanelType == WoodPanelType.OSB ? FoundValues.G_Seism4Osb : FoundValues.G_Seism4Ply;
                    }
                    if (NailSpacing == 3)
                    {
                        v = FoundValues.v_Seism3;
                        G_a = WoodPanelType == WoodPanelType.OSB ? FoundValues.G_Seism3Osb : FoundValues.G_Seism3Ply;
                    }
                    if (NailSpacing == 2)
                    {
                        v = FoundValues.v_Seism2;
                        G_a = WoodPanelType == WoodPanelType.OSB ? FoundValues.G_Seism2Osb : FoundValues.G_Seism2Ply;
                    }
                }
                else
                {
                    if (NailSpacing == 6)
                    {
                        v = FoundValues.v_Wind6;
                    }
                    if (NailSpacing == 4)
                    {
                        v = FoundValues.v_Wind4;
                    }
                    if (NailSpacing == 3)
                    {
                        v = FoundValues.v_Wind3;
                    }
                    if (NailSpacing == 2)
                    {
                        v = FoundValues.v_Wind2;
                    }
                }
                return (v, Fastener, FastenerPenetration, G_a);
            }
            return (0, null,null,0);
           
        }

    }
}
