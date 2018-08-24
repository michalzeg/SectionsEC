using NUnit.Framework;
using SectionsEC.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using SectionsEC.Calculations.Geometry;

namespace SectionsECTests.Windows.CommonWindows
{
    [TestFixture]
    public class PerimeterPropertiesTests
    {
        [Test]
        public void AddPerimeter_Passed()
        {
            double actualWidth = 100;
            double actualHeight = 100;

            var perimeterProperties = new PerimeterProperties(() => actualWidth, () => actualHeight);

            var perimeter1 = new List<PointD>() { new PointD(0, 0), new PointD(1, 1) };
            perimeterProperties.AddPerimeter(perimeter1);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(100, perimeterProperties.Scale);
                Assert.AreEqual(new PointD(0.5, 0.5), perimeterProperties.Centre);
            });
        }
    }
}