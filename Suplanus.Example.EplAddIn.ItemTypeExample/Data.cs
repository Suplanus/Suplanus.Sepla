using System.Collections.Generic;
using System.IO;
using Eplan.EplApi.Base;

namespace Suplanus.Example.EplAddIn.ItemTypeExample
{
    internal static class Data
    {
        public static List<ItemTypeObject> ItemTypeObjects = new List<ItemTypeObject>();

        private static readonly string Filename = PathMap.SubstitutePath(@"$(MD_PARTS)\ItemTypeObjects.xml");

        public static void Load()
        {
            if (File.Exists(Filename)) ItemTypeObjects = XmlHelper.Read<List<ItemTypeObject>>(Filename);
        }

        public static void Save()
        {
            XmlHelper.Write(ItemTypeObjects, Filename);
        }
    }
}