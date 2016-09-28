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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Kodestruct.Common.Mathematics;


namespace Kodestruct.Common.Data
{
    public partial class MapDataFileReader
    {
        //Two recursive algorithms are used:
        //------1st goes through all the latitudes (1st column) - binary search method is utilized
        //------2nd establishes an initial step and them goes through the data, if search direction
        //is flipped then the step is reduced

        public IMultipleValueDataPoint2D FindPointInDataFileRecursive(decimal Latitude, decimal Longitude, int startIndex, int EndIndex)
        {
            int chunkSize = 1 + (EndIndex - startIndex);

            if (chunkSize <= 0)
                return null;

            int currentIndex = startIndex + (chunkSize / 2);
            IMultipleValueDataPoint2D FirstPointWithGivenLatitude = readPointValue(currentIndex);

            if (FirstPointWithGivenLatitude.Latitude == Latitude)
            {
                IMultipleValueDataPoint2D foundPoint = FindPointInDataFileByLongitudeRecursive(FirstPointWithGivenLatitude, currentIndex, Latitude, Longitude, CurrentIncrementDirection.Unknown, 100);
                return foundPoint;
            }
                else //if latitude did not match
                {
                    if (FirstPointWithGivenLatitude.Latitude > Latitude)
                        return FindPointInDataFileRecursive(Latitude, Longitude, currentIndex + 1, EndIndex);
                    else
                        return FindPointInDataFileRecursive(Latitude, Longitude, startIndex, currentIndex - 1);
                }
            
        }

        private IMultipleValueDataPoint2D FindPointInDataFileByLongitudeRecursive(IMultipleValueDataPoint2D CurrentPoint, int currentIndex, decimal Latitude, decimal Longitude, 
            CurrentIncrementDirection incrementDirection, int Step)
        {

                decimal thisLongitude = CurrentPoint.Longitude;
                if (thisLongitude== Longitude) 
                {
                    return CurrentPoint;
                }
                else // if did not hot the target
                {

                    #region Current longitude less than target
                    if (thisLongitude < Longitude)
                    {
                        if (incrementDirection == CurrentIncrementDirection.Increase)
                        {
                            Step = (int)Step / 2;
                        }
                        incrementDirection = CurrentIncrementDirection.Decrease;
                        currentIndex = (currentIndex + Step) <= DataEndIndex ? currentIndex + Step : DataEndIndex;
                        CurrentPoint = readPointValue(currentIndex);

                        //case when this is a boundary condition
                        if (currentIndex >= DataEndIndex - NumberOfColumns) // if this is the last row in the table
                        {
                            int count1 = 0;
                            while (CurrentPoint.Longitude!= Longitude) //increment by one
                            {
                                currentIndex = currentIndex + 1;
                                CurrentPoint = readPointValue(currentIndex);
                                count1++;
                                if (count1 > NumberOfColumns)
                                {
                                    throw new Exception("Data point not found in table.");
                                }
                            }
                            return CurrentPoint;
                        }

                        if (CurrentPoint.Latitude != Latitude) //this means we overshot
                        {
                            //step one by one to go find value
                            while (CurrentPoint.Latitude != Latitude && CurrentPoint.Longitude != Longitude)
                            {
                                currentIndex = currentIndex - 1;
                                CurrentPoint = readPointValue(currentIndex);
                            }
                            return CurrentPoint;
                        }
                        else
                        {
                            //continue recursive iteration
                            return FindPointInDataFileByLongitudeRecursive(CurrentPoint, currentIndex,
                                Latitude, Longitude, incrementDirection, Step);
                        }
                    } 
                    #endregion

                    #region Current longitude greater than target
                    else //(thisLongitude>Longitude)
                    {
                        if (incrementDirection == CurrentIncrementDirection.Decrease)
                        {
                            Step = (int)Step / 2;
                        }
                        incrementDirection = CurrentIncrementDirection.Increase;
                        currentIndex = Step <= currentIndex ? currentIndex - Step : currentIndex - 1;
                        CurrentPoint = readPointValue(currentIndex);
                        //case when this is a boundary condition
                        if (currentIndex < NumberOfColumns) // if this is the first row in the table
                        {
                            int count2 = 0;
                            while (CurrentPoint.Longitude != Longitude) //increment by one
                            {
                                currentIndex = currentIndex - 1;
                                CurrentPoint = readPointValue(currentIndex);
                                count2++;
                                if (count2>NumberOfColumns)
                                {
                                    throw new Exception("Data point not found in table.");
                                }
                            }
                            return CurrentPoint;
                        }
                        if (CurrentPoint.Latitude != Latitude) //this means we overshot 
                        {
                            //step one by one to go find value
                            while (CurrentPoint.Latitude != Latitude && CurrentPoint.Longitude != Longitude)
                            {
                                currentIndex = currentIndex + 1;
                                CurrentPoint = readPointValue(currentIndex);
                            }
                            return CurrentPoint;
                        }
                        else
                        {
                            //continue recursive iteration
                            return FindPointInDataFileByLongitudeRecursive(CurrentPoint, currentIndex,
                                Latitude, Longitude, incrementDirection, Step);
                        }
                    } 
                    #endregion
                }
        }



    }
}
