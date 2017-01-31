using System;
using System.Collections.Generic;

namespace Suplanus.Sepla.Objects.Bus
{
   [Serializable]
   public class BusDevice
   {
      public string Name { get; set; }
      public List<BusPinBase> BusPins { get; set; }

      public BusDevice()
      {
         
      }

      public override string ToString()
      {
         return string.IsNullOrEmpty(Name) ? base.ToString() : Name;
      }

   }

}