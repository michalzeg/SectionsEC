using SectionsEC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMethods;
using SectionsEC.Helpers;
using SectionsEC.StressCalculations;

namespace SectionsEC.Dimensioning
{

	interface ICompressionZoneCalculations
	{

		CompressionZoneResult Calculate(double x,Section section);
	}


	public class CompressionZoneCalculationsGreenFormula : ICompressionZoneCalculations
	{


		private Concrete concrete;
		private IStrainCalculations strainCalculations;

		private IList<PointD> compressionZone;
		private IList<PointD> parabolicZone;
		private IList<PointD> linearZone;
		private double yNeutralAxis;
		private double y2Promiles;


		public CompressionZoneCalculationsGreenFormula(Concrete concrete,IStrainCalculations strainCalculations)
		{
			this.concrete = concrete;
			this.strainCalculations = strainCalculations;
		}
		
		public CompressionZoneResult Calculate(double x,Section section)
		{
			yNeutralAxis = section.MaxY - x;  
			double ec2Y = this.strainCalculations.Ec2Y(x);
			y2Promiles = yNeutralAxis + ec2Y;

			compressionZone = CompressionZoneCoordinates.CoordinatesOfCompressionZone(section.Coordinates, yNeutralAxis);
			parabolicZone = CompressionZoneCoordinates.CoordinatesOfParabolicSection(compressionZone, y2Promiles);
			linearZone = CompressionZoneCoordinates.CoordinatesOfLinearSection(compressionZone, y2Promiles);

			var result = new CompressionZoneResult();
			result.NormalForce = this.calculateResultantForce();
			result.Moment = this.calculateResultantMoment(x,section);
			return result;
		}
		private double calculateResultantForce()
		{
			return this.resultantOfLinearSection(linearZone) + this.resultantOfParabolicSection(parabolicZone, y2Promiles, yNeutralAxis);
		}
		private double calculateResultantMoment(double x, Section section)
		{
			double rLinear = 0; 
			double rParabolic = 0; 
			if (y2Promiles < section.MaxY) 
			{
				rLinear = (this.firstMomentOfAreaOfLinearSection(linearZone) / this.resultantOfLinearSection(linearZone)) - section.MinY;
			}

			double volumeOfLinearZone = this.resultantOfLinearSection(linearZone);
			double volumeOfParabolicZone = this.resultantOfParabolicSection(parabolicZone, y2Promiles, yNeutralAxis);

			var momentParabolic = firstMomentOfAreaOfParabolicSection(parabolicZone, y2Promiles, yNeutralAxis);

			rParabolic = section.MaxY - x + (this.firstMomentOfAreaOfParabolicSection(parabolicZone, y2Promiles, yNeutralAxis) / this.resultantOfParabolicSection(parabolicZone, y2Promiles, yNeutralAxis)) - section.MinY;
			return this.resultantOfLinearSection(linearZone) * rLinear + this.resultantOfParabolicSection(parabolicZone, y2Promiles, yNeutralAxis) * rParabolic;
			
		}
		private double resultantOfParabolicSection(IList<PointD> parabolicSection, double ec2Y, double neutralAxisY)
		{
	
			double ymax = ec2Y - neutralAxisY; 
			double a = this.concrete.Fcd * ymax / 3;
			double c = 1 / ymax;
			double V = 0.0;

			for (int i = 0; i <= parabolicSection.Count - 2; i++)
			{
				double dx = parabolicSection[i + 1].X - parabolicSection[i].X;
				double dy = parabolicSection[i + 1].Y - parabolicSection[i].Y;
				double xi = parabolicSection[i].X;
				double yi = parabolicSection[i].Y - neutralAxisY; 
				V = V + a * dx * (c * c * c * dy * dy * dy / 4 + Math.Pow(c * yi - 1, 3) + c * c * dy * dy * (c * yi - 1) + 0.5 * 3 * c * dy * (c * yi - 1) * (c * yi - 1)) + this.concrete.Fcd * dy * (dx / 2 + xi);
			}
			return V;
		}
		private double firstMomentOfAreaOfParabolicSection(IList<PointD> parabolicSection, double ec2Y, double neutralAxisY)
		{ 
			double ymax = ec2Y - neutralAxisY;
			double c = 1 / ymax;
			double S = 0.0;
			for (int i = 0; i <= parabolicSection.Count - 2; i++)
			{
				double dx = parabolicSection[i + 1].X - parabolicSection[i].X;
				double dy = parabolicSection[i + 1].Y - parabolicSection[i].Y;
				double xi = parabolicSection[i].X;
				double yi = parabolicSection[i].Y - neutralAxisY;
				//S = S + this.fcd * dx * (Math.Pow(c, 2) * (Math.Pow(dy, 4) / 20 + Math.Pow(dy, 3) * yi / 4 + Math.Pow(dy * yi, 2) / 2 + dy * Math.Pow(yi, 3) / 2 + Math.Pow(yi, 4) / 4) - c * (Math.Pow(dy, 3) / 6 + 2 * Math.Pow(dy, 2) * yi / 3 + dy * yi * yi + 2 * Math.Pow(yi, 3) / 3) + (dy / 6 + dy * yi / 2 + yi * yi / 2) - 1 / (12 * c * c)) + this.fcd * dy * (dx * dy / 3 + dy * xi / 2 + dx * yi / 2 + xi * yi);
				//S = S + this.concrete.Fcd / (c * c) * dx * (c * c * c * c * dy * dy * dy * dy / 20 + c * c * c * c * dy * dy * dy * yi / 4 + c * c * c * c * dy * dy * yi * yi / 2 + c * c * c * c * dy * yi * yi * yi / 4 - c * c * c * dy * dy * dy / 6 - 2 * c * c * c * dy * dy * yi / 3 - c * c * c * dy * yi * yi - 2 * c * c * c * yi * yi * yi / 3 + c * c * yi * yi / 6 + c * c * dy * yi / 2 + c * c * yi * yi / 2 - 1 / 12) + this.concrete.Fcd * dy * (dx * dy / 3 + dy * xi / 2 + dx * yi / 2 + xi * yi);
				//S = S + this.concrete.Fcd / (c * c) * dx * (c * c * c * c * dy * dy * dy * dy / 20 + c * c * c * c * dy * dy * dy * yi / 4 + c * c * c * c * dy * dy * yi * yi / 2 + c * c * c * c * yi * yi * yi * yi / 4 - c * c * c * dy * dy * dy / 6 - 2 * c * c * c * dy * dy * yi / 3 - c * c * c * dy * yi * yi - 2 * c * c * c * yi * yi * yi / 3 + c * c * dy * dy / 6 + c * c * dy * yi / 2 + c * c * yi * yi / 2 - (1 / 12)) + this.concrete.Fcd * dy * (dx * dy / 3 + dy * xi / 2 + dx * yi / 2 + xi * yi);
				S = S + this.concrete.Fcd / (c * c) * dx * (c * c * c * c * dy * dy * dy * dy / 20 + c * c * c * c * dy * dy * dy * yi / 4 + c * c * c * c * dy * dy * yi * yi / 2 + c*c*c*c*dy*yi*yi*yi/2+c * c * c * c * yi * yi * yi * yi / 4 - c * c * c * dy * dy * dy / 6 - 2 * c * c * c * dy * dy * yi / 3 - c * c * c * dy * yi * yi - 2 * c * c * c * yi * yi * yi / 3 + c * c * dy * dy / 6 + c * c * dy * yi / 2 + c * c * yi * yi / 2 - (1 / 12)) + this.concrete.Fcd * dy * (dx * dy / 3 + dy * xi / 2 + dx * yi / 2 + xi * yi);
			}
			return S;
		}
		private double resultantOfLinearSection(IList<PointD> linearSection)
		{
			double x1, x2, y1, y2;
			double V = 0;
			for (int i = 0; i <= linearSection.Count - 2; i++)
			{
				x1 = linearSection[i].X;
				x2 = linearSection[i + 1].X;
				y1 = linearSection[i].Y;
				y2 = linearSection[i + 1].Y;
				V = V + (x1 - x2) * (y2 + y1);
			}
			V = 0.5 * V * this.concrete.Fcd;
			return V;
		}
		private double firstMomentOfAreaOfLinearSection(IList<PointD> linearSection)
		{
			double x1, x2, y1, y2;
			double S = 0;
			for (int i = 0; i <= linearSection.Count - 2; i++)
			{
				x1 = linearSection[i].X;
				x2 = linearSection[i + 1].X;
				y1 = linearSection[i].Y;
				y2 = linearSection[i + 1].Y;
				S = S + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
			}
			S = S * this.concrete.Fcd / 6;
			return S;
		}

	}
	public class CompressionZoneCalculationsNumericalFormula :ICompressionZoneCalculations
	{
		private IStrainCalculations strainCalculations;
		private Concrete concrete;

		public CompressionZoneCalculationsNumericalFormula(Concrete concrete, IStrainCalculations strainCalculations)
		{
			this.strainCalculations = strainCalculations;
			this.concrete = concrete;
		}

		public CompressionZoneResult Calculate(double x,Section section)
		{

			var compressionZoneCoordinates = CompressionZoneCoordinates.CoordinatesOfCompressionZone(section.Coordinates, section.MaxY - x);
			var compressionZone = new Section(compressionZoneCoordinates);
			compressionZone.IntegrationPointY = section.IntegrationPointY;

			Func<double, double> distance = y => section.MaxY - y;
			Func<double, double> strain = di => this.strainCalculations.StrainInConcrete(x, distance(di));
			Func<double, double> stress = e => StressFunctions.ConcreteStressDesign(strain(e), this.concrete);

			var integration = new Integration();
			var result = integration.Integrate(compressionZone, stress);
			return result;
		}

	}


	class CompressionZoneCoordinates
	{
		public static IList<PointD> CoordinatesOfCompressionZone(IList<PointD> section, double neutralAxisY)
		{

			IList<PointD> compressedSection = new List<PointD>();

			PointD point1; 
			PointD point2; 
			PointD intersectionPoint; 
			for (int i = 0; i <= section.Count - 2; i++) 
			{
				point1 = section[i];
				point2 = section[i + 1];
				if ((point1.Y - point2.Y).IsApproximatelyEqualTo(0))
				{
					if ((point1.Y >= neutralAxisY) && (point2.Y >= neutralAxisY))
					{
						compressedSection.Add(point1);
						compressedSection.Add(point2);
					}
				}
				else
				{
					intersectionPoint = CompressionZoneCoordinates.intersectionPoint(point1, point2, neutralAxisY);
					if (isPointInsideSection(point1, point2, intersectionPoint))
					{
						if (point1.Y > intersectionPoint.Y)
						{
							compressedSection.Add(point1);
							compressedSection.Add(intersectionPoint);
						}
						else
						{
							compressedSection.Add(intersectionPoint);
							compressedSection.Add(point2);
						}
					}
					else
					{
						if ((point1.Y >= neutralAxisY) && (point2.Y >= neutralAxisY))
						{
							compressedSection.Add(point1);
							compressedSection.Add(point2);
						}
					}
				}
			}
			if (!((compressedSection[0].X.IsApproximatelyEqualTo(compressedSection[compressedSection.Count - 1].X)) && (compressedSection[0].Y.IsApproximatelyEqualTo(compressedSection[compressedSection.Count - 1].Y))))
			{
				PointD P = new PointD();
				P.X = compressedSection[0].X;
				P.Y = compressedSection[0].Y;
				compressedSection.Add(P);
			}
			return compressedSection;
		} 
		public static IList<PointD> CoordinatesOfLinearSection(IList<PointD> compressedSection, double ec2Y)
		{
			IList<PointD> linearSection = new List<PointD>();
			PointD point1; 
			PointD point2; 
			PointD intersectionPoint;
			for (int i = 0; i <= compressedSection.Count - 2; i++) 
			{
				point1 = compressedSection[i];
				point2 = compressedSection[i + 1];
				if ((point1.Y - point2.Y).IsApproximatelyEqualTo(0))
				{
					if ((point1.Y >= ec2Y) && (point2.Y >= ec2Y))
					{
						linearSection.Add(point1);
						linearSection.Add(point2);
					}
				}
				else
				{
					intersectionPoint = CompressionZoneCoordinates.intersectionPoint(point1, point2, ec2Y);
					if (isPointInsideSection(point1, point2, intersectionPoint))
					{
						if (point1.Y > intersectionPoint.Y)
						{
							linearSection.Add(point1);
							linearSection.Add(intersectionPoint);
						}
						else
						{
							linearSection.Add(intersectionPoint);
							linearSection.Add(point2);
						}
					}
					else
					{
						if ((point1.Y >= ec2Y) && (point2.Y >= ec2Y))
						{
							linearSection.Add(point1);
							linearSection.Add(point2);
						}
					}
				}
			}
			if (linearSection.Count > 0)
			{
				if (!((linearSection[0].X.IsApproximatelyEqualTo(linearSection[linearSection.Count - 1].X)) && (linearSection[0].Y.IsApproximatelyEqualTo(linearSection[linearSection.Count - 1].Y))))
				{
					PointD P = new PointD();
					P.X = linearSection[0].X;
					P.Y = linearSection[0].Y;
					linearSection.Add(P);
				}
			}
			return linearSection;
		}
		public static IList<PointD> CoordinatesOfParabolicSection(IList<PointD> compressedSection, double ec2Y)//przekroj o parabolicznym naprezeniu
		{
			IList<PointD> parabolicSection = new List<PointD>();
			PointD point1; 
			PointD point2; 
			PointD intersectionPoint; 
			for (int i = 0; i <= compressedSection.Count - 2; i++) 
			{
				point1 = compressedSection[i];
				point2 = compressedSection[i + 1];
				if ((point1.Y - point2.Y).IsApproximatelyEqualTo(0))
				{
					if ((point1.Y <= ec2Y) && (point2.Y <= ec2Y))
					{
						parabolicSection.Add(point1);
						parabolicSection.Add(point2);
					}
				}
				else
				{
					intersectionPoint = CompressionZoneCoordinates.intersectionPoint(point1, point2, ec2Y);
					if (isPointInsideSection(point1, point2, intersectionPoint))
					{
						if (point1.Y > intersectionPoint.Y)
						{
							parabolicSection.Add(intersectionPoint);
							parabolicSection.Add(point2);
						}
						else
						{
							parabolicSection.Add(point1);
							parabolicSection.Add(intersectionPoint);
						}
					}
					else
					{
						if ((point1.Y <= ec2Y) && (point2.Y <= ec2Y))
						{
							parabolicSection.Add(point1);
							parabolicSection.Add(point2);
						}
					}
				}

			}
			if (parabolicSection.Count > 0)
			{
				if (!((parabolicSection[0].X.IsApproximatelyEqualTo(parabolicSection[parabolicSection.Count - 1].X)) && (parabolicSection[0].Y.IsApproximatelyEqualTo(parabolicSection[parabolicSection.Count - 1].Y))))
				{
					PointD P = new PointD();
					P.X = parabolicSection[0].X;
					P.Y = parabolicSection[0].Y;
					parabolicSection.Add(P);
				}
			}

			return parabolicSection;
		}
		private static PointD intersectionPoint(PointD a1, PointD a2, double a)
		{
			double xa = a1.X;
			double xb = a2.X;
			double ya = a1.Y;
			double yb = a2.Y;

			PointD P = new PointD(); 
			P.X = ((a - ya) * (xb - xa)) / (yb - ya) + xa;
			return P;
		}
		private static bool isPointInsideSection(PointD A, PointD B, PointD P)
		{
			
			if (((Math.Min(A.X, B.X) <= P.X) && (P.X <= Math.Max(A.X, B.X)) && (Math.Min(A.Y, B.Y) <= P.Y)) && (P.Y <= Math.Max(A.Y, B.Y)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

	}

	

	public class SectionSlice
	{
		public double CentreOfGravityY { get; set; }
		public double Area { get; set; }
	}
	public class CompressionZoneResult
	{
		public double Moment { get; set; }
		public double NormalForce { get; set; }
	}

	public interface IIntegration
	{
		CompressionZoneResult Integrate(IIntegrable section, Func<double, double> distributionFunction);
	}
	public interface IIntegrable
	{
		IList<PointD> Coordinates { get; }
		double IntegrationPointY { get; }
		double MaxY { get; }
		double MinY { get; }
	}


	public class Integration : IIntegration
	{
		private readonly int numberOfSlices = 1000; //CHECK ACCURACY LATER
													//private readonly PointD pointOfMomentIntegration = new PointD(0, 0);

		//private IIntegrable section;
		//private Func<double, double> distributionFunction;
		public Integration()
		{
		}

		public CompressionZoneResult Integrate(IIntegrable section, Func<double, double> distributionFunction)
		{
			double resultantMoment = 0;
			double resultantNormalForce = 0;
			Slicing slicing = new Slicing();

			double currentY = section.MinY;
			double deltaY = (section.MaxY - section.MinY) / this.numberOfSlices;
			while (currentY <= section.MaxY) 
			{

				SectionSlice slice = slicing.GetSlice(section.Coordinates, currentY + deltaY, currentY);
				currentY = currentY + deltaY;
				double value = distributionFunction(slice.CentreOfGravityY);
				double normalForce = value * slice.Area;
				double leverArm = Math.Abs(section.IntegrationPointY - slice.CentreOfGravityY);
				//double moment = ((slice.CentreOfGravityY < section.IntegrationPointY) ? -leverArm * value * slice.Area : leverArm * value * slice.Area);
				double moment = leverArm * value * slice.Area;

				resultantMoment = resultantMoment + moment;
				resultantNormalForce = resultantNormalForce + normalForce;
			}
			

			var result = new CompressionZoneResult();
			result.NormalForce = resultantNormalForce;
			result.Moment = resultantMoment;
			return result;
		}
	}

	class Slicing 
	{

		public SectionSlice GetSlice(IList<PointD> section, double upperY, double lowerY)
		{

			IList<PointD> lowerCoordinates = this.lowerSection(section, lowerY);
			IList<PointD> upperCoordinates = this.upperSection(lowerCoordinates, upperY);
			var sectionSlice = this.calculateProperties(upperCoordinates);
			return sectionSlice;
		}


		private SectionSlice calculateProperties(IList<PointD> coordinates)
		{
			//calculate section properties
			//SectionPropertiesCalculations calculations = new SectionPropertiesCalculations(coordinates);
			SectionSlice slice = new SectionSlice();
			slice.Area = SectionProperties.A(coordinates);//calculations.A;
			slice.CentreOfGravityY = SectionProperties.Cz(coordinates);
			return slice;
		}
		private IList<PointD> lowerSection(IList<PointD> section, double a) //lower boundary of the slice
		{

			IList<PointD> compressedSection = new List<PointD>();
			PointD point1; 
			PointD point2; 
			PointD intersectionPoint; 
			for (int i = 0; i <= section.Count - 2; i++) 
			{
				point1 = section[i];
				point2 = section[i + 1];
				if ((point1.Y - point2.Y).IsApproximatelyEqualTo(0))
				{
					if ((point1.Y >= a) && (point2.Y >= a))
					{
						compressedSection.Add(point1);
						compressedSection.Add(point2);
					}
				}
				else
				{
					intersectionPoint = this.intersectionPoint(point1, point2, a);
					if (this.isPointInsideSection(point1, point2, intersectionPoint))
					{
						if (point1.Y > intersectionPoint.Y)
						{
							compressedSection.Add(point1);
							compressedSection.Add(intersectionPoint);
						}
						else
						{
							compressedSection.Add(intersectionPoint);
							compressedSection.Add(point2);
						}
					}
					else
					{
						if ((point1.Y >= a) && (point2.Y >= a))
						{
							compressedSection.Add(point1);
							compressedSection.Add(point2);
						}
					}
				}

			}
			if (!(((compressedSection[0].X).IsApproximatelyEqualTo(compressedSection[compressedSection.Count - 1].X)) && (compressedSection[0].Y.IsApproximatelyEqualTo(compressedSection[compressedSection.Count - 1].Y))))
			{
				PointD P = new PointD();
				P.X = compressedSection[0].X;
				P.Y = compressedSection[0].Y;
				compressedSection.Add(P);
			}
			return compressedSection;
		}
		private List<PointD> upperSection(IList<PointD> compressedSection, double a)
		{
			List<PointD> parabolicSection = new List<PointD>();
			PointD point1; 
			PointD point2; 
			PointD intersectionPoint; 
			for (int i = 0; i <= compressedSection.Count - 2; i++) 
			{
				point1 = compressedSection[i];
				point2 = compressedSection[i + 1];
				if ((point1.Y - point2.Y).IsApproximatelyEqualTo(0))
				{
					if ((point1.Y <= a) && (point2.Y <= a))
					{
						parabolicSection.Add(point1);
						parabolicSection.Add(point2);
					}
				}
				else
				{
					intersectionPoint = this.intersectionPoint(point1, point2, a);
					if (this.isPointInsideSection(point1, point2, intersectionPoint))
					{
						if (point1.Y > intersectionPoint.Y)
						{
							parabolicSection.Add(intersectionPoint);
							parabolicSection.Add(point2);
						}
						else
						{
							parabolicSection.Add(point1);
							parabolicSection.Add(intersectionPoint);
						}
					}
					else
					{
						if ((point1.Y <= a) && (point2.Y <= a))
						{ 
							parabolicSection.Add(point1);
							parabolicSection.Add(point2);
						}
					}
				}

			}
			if (parabolicSection.Count > 0)
			{
				if (!((parabolicSection[0].X.IsApproximatelyEqualTo(parabolicSection[parabolicSection.Count - 1].X)) && (parabolicSection[0].Y.IsApproximatelyEqualTo(parabolicSection[parabolicSection.Count - 1].Y))))
				{
					PointD P = new PointD();
					P.X = parabolicSection[0].X;
					P.Y = parabolicSection[0].Y;
					parabolicSection.Add(P);
				}
			}

			return parabolicSection;
		}
		private PointD intersectionPoint(PointD a1, PointD a2, double a)
		{
			double xa = a1.X;
			double xb = a2.X;
			double ya = a1.Y;
			double yb = a2.Y;

			PointD P = new PointD(); 
			P.Y = a; 
			P.X = ((a - ya) * (xb - xa)) / (yb - ya) + xa;
			return P;
		}
		private bool isPointInsideSection(PointD A, PointD B, PointD P)
		{
			
			if (((Math.Min(A.X, B.X) <= P.X) && (P.X <= Math.Max(A.X, B.X)) && (Math.Min(A.Y, B.Y) <= P.Y)) && (P.Y <= Math.Max(A.Y, B.Y)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
