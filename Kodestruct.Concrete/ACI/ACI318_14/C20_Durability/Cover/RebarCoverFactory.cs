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
 
using Kodestruct.Common.Data;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI.ACI318_14.Durability.Cover
{
    public class RebarCoverFactory
    {
        public double GetRebarCover(string CoverCaseId, RebarDesignation RebarDesignation, bool BarDiameterCheck)
        {
            int RebarNumber = (int)Double.Parse(RebarDesignation.ToString().Substring(2));
            RebarSection sec = new RebarSection(RebarDesignation);
            double d_b = sec.Diameter;

            #region Read Cover Data

            var SampleValue = new { 
                CaseId = "", 
                SmallBarNo = 1, 
                MidBarNo = 1, 
                SmallBarCover=0.0,
                MidBarCover=0.0,
                LargeBarCover=0.0,
                IsAllSameDiameterCase = true,
                DiameterCheck=true }; // sample


            var Covers = ListFactory.MakeList(SampleValue);

            using (StringReader reader = new StringReader(Resources.ACI_RebarCover))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] Vals = line.Split(',');
                    if (Vals.Length == 6)
                    {
                        string _CaseId = (string)Vals[0];

                        #region Diameters
                        string smallDiaString = (string)Vals[1];
                        string midDiaString = (string)Vals[2];

                        int _SmallBarNo = 0;
                        int _MidBarNo = 0;
                        bool _IsAllSameDiameterCase = false;


                        if (smallDiaString == "All" || midDiaString == "All")
                        {
                            _IsAllSameDiameterCase = true;
                            _SmallBarNo = 0;
                            _MidBarNo = 0;
                        }
                        else
                        {
                            _SmallBarNo = (int)Double.Parse(smallDiaString);
                            _MidBarNo = (int)Double.Parse(midDiaString);

                        } 
                        #endregion

                        string _SmallBarCoverStr = (string)Vals[3];
                        string _MidBarCoverStr = (string)Vals[4];
                        string _LargeBarCoverStr = (string)Vals[5];

                        bool _DiameterCheck = false;

                        double _SmallBarCover = 0;
                        double _MidBarCover   = 0;
                        double _LargeBarCover = 0;



                        if (_SmallBarCoverStr.Contains("db"))
                        {
                            _SmallBarCover = Double.Parse(_SmallBarCoverStr.Remove(_SmallBarCoverStr.Length-2));
                            _DiameterCheck = true;
                        }
                        else
                        {
                            _SmallBarCover = Double.Parse(_SmallBarCoverStr);
                        }

                        if (_MidBarCoverStr.Contains("db"))
                        {
                            _MidBarCover = Double.Parse(_MidBarCoverStr.Remove(_MidBarCoverStr.Length-2));
                            _DiameterCheck = true;
                        }
                        else
                        {
                            _MidBarCover = Double.Parse(_MidBarCoverStr);
                        }

                        if (_LargeBarCoverStr.Contains("db"))
                        {
                            _LargeBarCover = Double.Parse(_LargeBarCoverStr.Remove(_LargeBarCoverStr.Length-2));
                            _DiameterCheck = true;
                        }
                        else
                        {
                            _LargeBarCover = Double.Parse(_LargeBarCoverStr);
                        }

                        
                        Covers.Add
                        (new
                        {
                            CaseId =                _CaseId,
                            SmallBarNo =            _SmallBarNo , 
                            MidBarNo =              _MidBarNo , 
                            SmallBarCover=          _SmallBarCover,
                            MidBarCover=            _MidBarCover,
                            LargeBarCover=          _LargeBarCover,
                            IsAllSameDiameterCase = _IsAllSameDiameterCase , 
                            DiameterCheck=          _DiameterCheck,
                        }

                        );
                    }
                }

            }

            #endregion

            var CoverEntryData = Covers.First(l => l.CaseId == CoverCaseId);
            bool CheckDia = CoverEntryData.DiameterCheck == true ? true : BarDiameterCheck;

            double cc = 0.0;
            if (CoverEntryData.IsAllSameDiameterCase == true)
            {
                if (CheckDia == false)
                {
                    return CoverEntryData.SmallBarCover;
                }
                else
                {
                    return Math.Max(d_b, CoverEntryData.SmallBarCover);
                }
            }
            else
            {
                if (RebarNumber <=CoverEntryData.SmallBarNo)
                {
                    return CoverEntryData.SmallBarCover;
                }
                else if (RebarNumber <= CoverEntryData.MidBarCover )
	            {
                    return CoverEntryData.MidBarCover;
	            }
                else
                {
                    return CoverEntryData.LargeBarCover;
                }
            }

        }
    }
}
