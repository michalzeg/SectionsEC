using SectionsEC.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SectionsEC.Materials
{
    public class Material
    {
        public List<Concrete> Concrete { get; set; }
        public List<Steel> Steel { get; set; }
    }

    public class MaterialOperations
    {
        public static Material GetMaterials()
        {
            var location = Path.GetDirectoryName(typeof(Material).Assembly.Location);

            var filePath = Path.Combine(location, "materials.xml");
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