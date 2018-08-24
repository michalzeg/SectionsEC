using NUnit.Framework;
using SectionsEC.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using SectionsEC.Helpers;

namespace SectionsEC.Drawing.Tests
{
    [TestFixture()]
    public class PerimeterPropertiesTests
    {
        [Test()]
        public void AddPerimeter_Passed()
        {
            double actualWidth = 100;
            double actualHeight = 100;

            var perimeterProperties = new PerimeterProperties(()=> actualWidth,()=> actualHeight);

            var perimeter1 = new List<PointD>() { new PointD(0, 0), new PointD(1, 1) };
            perimeterProperties.AddPerimeter(perimeter1);
            Assert.AreEqual(100, perimeterProperties.Scale);
            Assert.AreEqual(new PointD(0.5, 0.5), perimeterProperties.Centre);

            var perimeter2 = new List<PointD>() { new PointD(0.5, 0.5), new PointD(0.1, 0.1) };
            Assert.AreEqual(100, perimeterProperties.Scale);
            Assert.AreEqual(new PointD(0.5, 0.5), perimeterProperties.Centre);

            actualWidth = 200;
            actualHeight = 200;
            perimeterProperties.UpdateProperties();
            Assert.AreEqual(200, perimeterProperties.Scale);
            Assert.AreEqual(new PointD(0.5, 0.5), perimeterProperties.Centre);

            var perimeter3 = new List<PointD>() { new PointD(10, 5) };
            perimeterProperties.AddPerimeter(perimeter3);
            Assert.AreEqual(20, perimeterProperties.Scale);
            Assert.AreEqual(new PointD(5, 2.5), perimeterProperties.Centre);
            //Assert.Fail();

            perimeterProperties.RemovePerimeter(perimeter3);
            Assert.AreEqual(200, perimeterProperties.Scale);
            Assert.AreEqual(new PointD(0.5, 0.5), perimeterProperties.Centre);

        }
    }
}