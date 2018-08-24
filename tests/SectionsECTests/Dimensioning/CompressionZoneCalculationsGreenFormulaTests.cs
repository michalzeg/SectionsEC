using NUnit.Framework;
using SectionsEC.Calculations.Geometry;
using SectionsEC.Calculations.Materials;
using SectionsEC.Calculations.Sections;
using SectionsEC.Calculations.StressFunctions;
using SectionsEC.Dimensioning;
using SectionsEC.Dimensioning.CompressionZone;
using SectionsEC.Dimensioning.Dimensioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsECTests.Dimensioning
{
    [TestFixture]
    public class CompressionZoneCalculationsGreenFormulaTests
    {
        [Test]
        public void CompresionZoneGreenFormulaTest_RectangularSection_Passed()
        {
            var concrete = new Concrete
            {
                Acc = 1d,
                Ec2 = 2d / 1000d,
                Ecu2 = 3.5d / 1000d,
                GammaM = 1.5d,
                N = 2,
                Fck = 12000
            };

            var steel = new Steel
            {
                Fyk = 500000,
                GammaS = 1.15,
                K = 1.05,
                Euk = 2.5d / 100d,
                EukToEud = 0.9,
                Es = 200000000
            };

            var sectionCapacity = new SectionCapacity(concrete, steel);

            List<PointD> coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(1, 0),
                new PointD(1, 1),
                new PointD(0, 1),
                new PointD(0, 0)
            };

            var section = new Section(coordinates);

            var straincalcs = new StrainCalculations(concrete, steel, section);

            var compresionZoneCalcs = new CompressionZoneCalculationsGreenFormula(concrete, straincalcs);

            var result = compresionZoneCalcs.Calculate(0.919569645356183, section);

            Assert.AreEqual(5955.3, result.NormalForce, 0.1);
            Assert.AreEqual(3677.313, result.Moment, 0.1);
        }
    }
}