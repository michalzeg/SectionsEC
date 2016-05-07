using NUnit.Framework;
using SectionsEC.Dimensioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SectionsEC.Helpers;
namespace SectionsEC.Dimensioning.Tests
{
    [TestFixture()]
    public class SectionCapacity
    {
        [TestCase(0,1085)]
        [TestCase(1000, 1338)]
        [TestCase(-1000, 716)]
        [TestCase(5000, 951)]
        [TestCase(6000, 681)]
        public void SectionCapacity_REctangularSectionWithOneBarNormalConcrete_Passed(double normalForce,double expectedCapacity)
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
            steel.Euk = 2.5d/100d;
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
            List<Reinforcement> reinforcement = new List<Reinforcement>();
            List<Bar> reinf = new List<Bar>();
            Bar bar = new Bar();
            bar.As = 30d / 10000d;
            bar.X = 0.5;
            bar.Y = 0.1;
            reinf.Add(bar);
            var results = sc.CalculateCapacity(normalForce, section, reinf);
            Assert.AreEqual(expectedCapacity, results.Mrd, 1);
        }
        [TestCase(0, 1278)]
        [TestCase(1000, 1688)]
        [TestCase(-1000, 828.7)]
        [TestCase(10000, 4889)]
        [TestCase(25000, 6345)]
        public void SectionCapacity_REctangularSectionWithOneBarHighStrengthConcrete_Passed(double normalForce, double expectedCapacity)
        {
            Concrete concrete = new Concrete();
            concrete.Acc = 1d;
            concrete.Ec2 = 2.6d / 1000d;
            concrete.Ecu2 = 2.6d / 1000d;
            concrete.GammaM = 1.5d;
            concrete.N = 1.4;
            concrete.Fck = 90000;
            Steel steel = new Steel();
            steel.Fyk = 500000;
            steel.GammaS = 1.15;
            steel.K = 1.15;
            steel.Euk = 7.5d / 100d;
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
            List<Reinforcement> reinforcement = new List<Reinforcement>();
            List<Bar> reinf = new List<Bar>();
            Bar bar = new Bar();
            bar.As = 30d / 10000d;
            bar.X = 0.5;
            bar.Y = 0.1;
            reinf.Add(bar);
            var results = sc.CalculateCapacity(normalForce, section, reinf);
            Assert.AreEqual(expectedCapacity, results.Mrd, 1);
        }
        [TestCase(0, 315.8)]
        [TestCase(1000, 28.97)]
        [TestCase(-1000, 482.4)]
        [TestCase(500, 174.1)]
        [TestCase(-600, 484.7)]
        public void SectionCapacity_CustomSectionWithOneBarNormalConcrete_Passed(double normalForce, double expectedCapacity)
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
            coordinates.Add(new PointD(0,0));
            coordinates.Add(new PointD(0.1,0));
            coordinates.Add(new PointD(0.2,0.5));
            coordinates.Add(new PointD(0.3,0.6));
            coordinates.Add(new PointD(-0.2,0.6));
            coordinates.Add(new PointD(-0.1,0.5));
            coordinates.Add(new PointD(0, 0));
            Section section = new Section(coordinates);
            List<Reinforcement> reinforcement = new List<Reinforcement>();
            List<Bar> reinf = new List<Bar>();
            Bar bar = new Bar();
            bar.As = 30d / 10000d;
            bar.X = 0.05;
            bar.Y = 0.05;
            reinf.Add(bar);
            /*var results = sc.CalculateCapacity(normalForce, section, reinf);
            List<Reinforcement> reinforcement = new List<Reinforcement>();
            Reinforcement reinf = new Reinforcement();
            Bar bar = new Bar();
            bar.As = 30d / 10000d;
            bar.X = 0.05;
            bar.Y = 0.05;
            reinf.Bar = bar;
            reinforcement.Add(reinf);*/
            var results = sc.CalculateCapacity(normalForce, section, reinf);
            Assert.AreEqual(expectedCapacity, results.Mrd, 1);
        }
        [TestCase(0, 696)]
        [TestCase(1000, 833)]
        [TestCase(-1000, 554.1)]
        [TestCase(500, 770.2)]
        [TestCase(700, 796.9)]
        public void SectionCapacity_CustomSectionWithOneBarHighStrengthConcrete_Passed(double normalForce, double expectedCapacity)
        {
            Concrete concrete = new Concrete();
            concrete.Acc = 1d;
            concrete.Ec2 = 2.6d / 1000d;
            concrete.Ecu2 = 2.6d / 1000d;
            concrete.GammaM = 1.5d;
            concrete.N = 1.4;
            concrete.Fck = 90000;
            Steel steel = new Steel();
            steel.Fyk = 500000;
            steel.GammaS = 1.15;
            steel.K = 1.15;
            steel.Euk = 7.5d / 100d;
            steel.EukToEud = 0.9;
            steel.Es = 200000000;
            Dimensioning.SectionCapacity sc = new Dimensioning.SectionCapacity(concrete, steel);
            List<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(0.1, 0));
            coordinates.Add(new PointD(0.2, 0.5));
            coordinates.Add(new PointD(0.3, 0.6));
            coordinates.Add(new PointD(-0.2, 0.6));
            coordinates.Add(new PointD(-0.1, 0.5));
            coordinates.Add(new PointD(0, 0));
            Section section = new Section(coordinates);
            List<Reinforcement> reinforcement = new List<Reinforcement>();
            List<Bar> reinf = new List<Bar>();
            Bar bar = new Bar();
            bar.As = 30d / 10000d;
            bar.X = 0.05;
            bar.Y = 0.05;
            reinf.Add(bar);
            /*var results = sc.CalculateCapacity(normalForce, section, reinf);
            List<Reinforcement> reinforcement = new List<Reinforcement>();
            Reinforcement reinf = new Reinforcement();
            Bar bar = new Bar();
            bar.As = 30d / 10000d;
            bar.X = 0.05;
            bar.Y = 0.05;
            reinf.Bar = bar;
            reinforcement.Add(reinf);*/
            var results = sc.CalculateCapacity(normalForce, section, reinf);
            Assert.AreEqual(expectedCapacity, results.Mrd, 1);
        }
    }
}