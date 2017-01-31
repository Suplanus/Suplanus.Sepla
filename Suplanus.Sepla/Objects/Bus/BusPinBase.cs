using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Suplanus.Sepla.Objects.Bus
{
   [Serializable]
   public class BusPinBase
   {
      public BusPinBase()
      {

      }

      public override string ToString()
      {
         return Name;
      }

      public string Name { get; set; }
      public string Address { get; set; }

      public BusDevice BusDevice { get; set; }

      public BusType BusType { get; set; }
      public PlugSocketType PlugSocketType { get; set; }            
      public bool IsTerminated { get; set; }
      public bool IsNeedingPlugOrSocket { get; set; }
      public bool IsMaster { get; set; }

      public BusPinBase Destination { get; set; }

      

      public string DisplayText
      {
         get
         {
            var value = string.Format("{0} - {1}", Name, Address);
            return value;
         }
      }

      public bool IsPlaced { get; set; }
   }
}
