using NUnit.Framework;
using SectionsEC.Dimensioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using SectionsEC.Common.Geometry;
using SectionsEC.Common.Interfaces;
using SectionsEC.Dimensioning.Integration;

namespace SectionsECTests.Dimensioning
{
    [TestFixture]
    public class NumericalIntegrationTests
    {
        [Test]
        public void Integrate_RectangulerSection_Passed()
        {
            IList<PointD> coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(1, 0),
                new PointD(1, 1),
                new PointD(0, 1),
                new PointD(0, 0)
            };

            var section = Substitute.For<IIntegrable>();
            section.Coordinates.Returns(coordinates);
            section.IntegrationPointY.Returns(0);
            section.MaxY.Returns(1);
            section.MinY.Returns(0);

            var integration = new IntegrationCalculator();
            var result = integration.Integrate(section, (e) => 1);

            Assert.AreEqual(1d, result.NormalForce, 0.001);
            Assert.AreEqual(0.5, result.Moment, 0.001);
        }

        [Test()]
        public void Integrate_RectangulerSectionParabilicFunction_Passed()
        {
            IList<PointD> coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(1, 0),
                new PointD(1, 1),
                new PointD(0, 1),
                new PointD(0, 0)
            };

            var section = Substitute.For<IIntegrable>();
            section.Coordinates.Returns(coordinates);
            section.IntegrationPointY.Returns(0);
            section.MaxY.Returns(1);
            section.MinY.Returns(0);

            var integration = new IntegrationCalculator();
            var result = integration.Integrate(section, y => (1 - (1 - y) * (1 - y)));

            Assert.AreEqual(0.6666667, result.NormalForce, 0.001);
            Assert.AreEqual(0.416667, result.Moment, 0.001);
        }
    }
}