using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SectionsEC.Calculations.Materials
{
    public class MaterialProvider
    {
        public static Material GetMaterials()
        {
            var location = Path.GetDirectoryName(typeof(Material).Assembly.Location);

            var filePath = Path.Combine(location, "Resources", "materials.xml");
            Material material;
            XmlSerializer serializer = new XmlSerializer(typeof(Material));
            using (var reader = new StreamReader(filePath))
            {
                material = serializer.Deserialize(reader) as Material;
            }
            return material;
        }

        public static IEnumerable<Concrete> GetConcrete()
        {
            return GetMaterials().Concrete;
        }

        public static IEnumerable<Steel> GetSteel()
        {
            return GetMaterials().Steel;
        }
    }
}