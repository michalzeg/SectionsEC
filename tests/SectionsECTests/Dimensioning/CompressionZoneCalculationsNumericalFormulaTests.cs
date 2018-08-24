using NUnit.Framework;
using SectionsEC.Dimensioning;
using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.Dimensioning.Tests
{
    [TestFixture()]
    public class CompressionZoneCalculationsNumericalFormulaTests
    {
        [Test()]
        public void CompresionZoneNumericalFormulaTest_RectangularSection_Passed()
        {
            Concrete concrete = new Concrete();
            concrete.Acc = 1d;
            concrete.Ec2 = 2d / 1000d;
            concrete.Ecu2 = 3.5d / 1000d;
            concrete.GammaM = 1.5d;
            concrete.N = 2;
            concrete.Fck = 12000;

            Steel steel = new Steel();
            steel.Fyk = 500000;
            steel.GammaS = 1.15;
            steel.K = 1.05;
            steel.Euk = 2.5d / 100d;
            steel.EukToEud = 0.9;
            steel.Es = 200000000;

            Dimensioning.SectionCapacity sc = new Dimensioning.SectionCapacity(concrete, steel);

            List<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(1, 0));
            coordinates.Add(new PointD(1, 1));
            coordinates.Add(new PointD(0, 1));
            coordinates.Add(new PointD(0, 0));

            Section section = new Section(coordinates);

            var straincalcs = new StrainCalculations(concrete, steel, section);

            var compresionZoneCalcs = new CompressionZoneCalculationsNumericalFormula(concrete, straincalcs);

            var result = compresionZoneCalcs.Calculate(0.919569645356183, section);

            Assert.AreEqual(5955.3, result.NormalForce, 0.1);
            Assert.AreEqual(3677.313, result.Moment, 0.1);
        }
    }
}