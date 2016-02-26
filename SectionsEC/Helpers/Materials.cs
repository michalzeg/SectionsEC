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
            Material material;
            XmlSerializer serializer = new XmlSerializer(typeof(Material));

            using (var reader = new StringReader(Properties.Resources.materials)) 
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
