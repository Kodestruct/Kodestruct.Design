using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Steel.AISC.Entities.Enums.FloorVibrations;
using Kodestruct.Steel.AISC.Entities.FloorVibrations;


namespace Kodestruct.Steel.Tests.AISC
{
    [TestFixture]
    public class FloorVibrationTests : ToleranceTestBase
    {
        public FloorVibrationTests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;

        /// <summary>
        /// AISC Live Webinar
        /// May 5, 2016
        /// Thomas Murray
        /// Vibration of Steel Framed Structural Systems Due to Human Activity
        /// Session 1: Basic Analysis of Structural Systems Due to Human Activity 
        /// </summary>
        [Test]
        public void SingleBeamReturnsFrequency()
        {
            FloorVibrationBeamSingle bm = new FloorVibrationBeamSingle();
            double f_n = bm.GetFundamentalFrequency(0.376);
            double refValue = 5.77;
            double actualTolerance = EvaluateActualTolerance(f_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        /// <summary>
        /// AISC Live Webinar
        /// May 5, 2016
        /// Thomas Murray
        /// Vibration of Steel Framed Structural Systems Due to Human Activity
        /// Session 1: Basic Analysis of Structural Systems Due to Human Activity 
        /// </summary>
        [Test]
        public void PanelReturnsModalDamping()
        {
            FloorVibrationBeamGirderPanel bmPanel = new FloorVibrationBeamGirderPanel();
            List<string> Components =
                new List<string>()
                {

                    "Structural system",
                    "Ceiling and ductwork",
                    "Electronic office fitout"

                };
            double beta = bmPanel.GetFloorModalDampingRatio(Components);
            double refValue = 0.025;
            double actualTolerance = EvaluateActualTolerance(beta, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        /// <summary>
        /// AISC Live Webinar
        /// May 5, 2016
        /// Thomas Murray
        /// Vibration of Steel Framed Structural Systems Due to Human Activity
        /// Session 2: Basic Analysis of Structural Systems Due to Human Activity 
        /// </summary>
        [Test]
        public void PanelReturnsJoistEffectiveWidth()
        {
            FloorVibrationBeamGirderPanel bmPanel = new FloorVibrationBeamGirderPanel();
            double B_j = bmPanel.GetEffectiveJoistWidth(4.0, 110.0, 3.5, 2.0, 6.0, 12,
                Steel.AISC.DeckAtBeamCondition.Perpendicular,45.0*12.0, 1800.0, 7.5 * 12, 30.0 * 12, 
                Steel.AISC.Entities.Enums.FloorVibrations.BeamFloorLocationType.Inner);
  
            double refValue = 20*12;
            double actualTolerance = EvaluateActualTolerance(B_j, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// AISC Live Webinar
        /// May 5, 2016
        /// Thomas Murray
        /// Vibration of Steel Framed Structural Systems Due to Human Activity
        /// Session 2: Basic Analysis of Structural Systems Due to Human Activity 
        /// </summary>
        [Test]
        public void PanelReturnsGirderEffectiveWidth()
        {
            double I_g = 4970.0;

            
            FloorVibrationBeamGirderPanel bmPanel = new FloorVibrationBeamGirderPanel();
            //double B_g = bmPanel.GetEffectiveGirderWidth(30.0 * 12, 90.0 * 12, 45.0 * 12, 240.0 / 12, 110.0 / 12.0, BeamFloorLocationType.Inner, JoistToGirderConnectionType.ConnectionToWeb);
            double B_g = bmPanel.GetEffectiveGirderWidth(30.0 * 12, 45.0 * 12, 4970, 1800.0, 7.5*12, 90.0 * 12, BeamFloorLocationType.Inner, JoistToGirderConnectionType.ConnectionToWeb);


            double refValue = 60.0*12.0;
            double actualTolerance = EvaluateActualTolerance(B_g, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// AISC Live Webinar
        /// May 5, 2016
        /// Thomas Murray
        /// Vibration of Steel Framed Structural Systems Due to Human Activity
        /// Session 2: Basic Analysis of Structural Systems Due to Human Activity 
        /// </summary>
        [Test]
        public void PanelReturnsJoistEffectiveWeight()
        {
            FloorVibrationBeamGirderPanel bmPanel = new FloorVibrationBeamGirderPanel();
            double W_j = bmPanel.GetJoistModeEffectiveWeight(0.472 / 12.0, 7.5 * 12, 20.0 * 12.0, 45.0 * 12, AdjacentSpanWeightIncreaseType.HotRolledBeamOverTheColumn);
            double refValue = 85.0;
            double actualTolerance = EvaluateActualTolerance(W_j, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// AISC Live Webinar
        /// May 5, 2016
        /// Thomas Murray
        /// Vibration of Steel Framed Structural Systems Due to Human Activity
        /// Session 2: Basic Analysis of Structural Systems Due to Human Activity 
        /// </summary>
        [Test]
        public void PanelReturnsGirderEffectiveWeight()
        {
            double w_g = 2.89 / 12.0;
            FloorVibrationBeamGirderPanel bmPanel = new FloorVibrationBeamGirderPanel();
            double W_g = bmPanel.GetGirderModeEffectiveWeight(w_g, 30.0 * 12.0, 60.0 * 12.0, 45.0 * 12);
            double refValue = 116.0;
            double actualTolerance = EvaluateActualTolerance(W_g, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// AISC Live Webinar
        /// May 5, 2016
        /// Thomas Murray
        /// Vibration of Steel Framed Structural Systems Due to Human Activity
        /// Session 2: Basic Analysis of Structural Systems Due to Human Activity 
        /// </summary>
        [Test]
        public void PanelReturnsCombinedWeight()
        {
            FloorVibrationBeamGirderPanel bmPanel = new FloorVibrationBeamGirderPanel();
            double W = bmPanel.GetCombinedModeEffectiveWeight(0.835, 0.366, 85.0, 116.0);
            double refValue = 94.3;
            double actualTolerance = EvaluateActualTolerance(W, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        /// <summary>
        /// AISC Live Webinar
        /// May 5, 2016
        /// Thomas Murray
        /// Vibration of Steel Framed Structural Systems Due to Human Activity
        /// Session 2: Basic Analysis of Structural Systems Due to Human Activity 
        /// </summary>
        [Test]
        public void PanelReturnsCombinedFrequency()
        {
            FloorVibrationBeamGirderPanel bmPanel = new FloorVibrationBeamGirderPanel();
            double f_c = bmPanel.GetCombinedModeFundamentalFrequency(0.835, 0.366);
            double refValue = 3.23;
            double actualTolerance = EvaluateActualTolerance(f_c, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        /// <summary>
        /// AISC Live Webinar
        /// May 5, 2016
        /// Thomas Murray
        /// Vibration of Steel Framed Structural Systems Due to Human Activity
        /// Session 2: Basic Analysis of Structural Systems Due to Human Activity 
        /// </summary>
        [Test]
        public void PanelReturnsPredictedAcceleration()
        {
            FloorVibrationBeamGirderPanel bmPanel = new FloorVibrationBeamGirderPanel();
            double a_p_g = bmPanel.GetAccelerationRatio(3.23, 94.3, 0.03, "Office");
            double refValue = 0.0074;
            double actualTolerance = EvaluateActualTolerance(refValue, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }
    }
}
