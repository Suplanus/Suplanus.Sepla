using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Suplanus.Sepla.Annotations;

namespace Suplanus.Sepla.Objects.Bus
{
   [Serializable]
   public class BusPinBase : INotifyPropertyChanged
   {
      private BusPinBase _destination;

      public BusPinBase()
      {

      }

      public override string ToString()
      {
         return Name;
      }

      public string Name { get; set; }
      public string PinName { get; set; }
      public string PinDescription { get; set; }
      public string Address { get; set; }

      public BusDevice BusDevice { get; set; }

      public BusType BusType { get; set; }
      public PlugSocketType PlugSocketType { get; set; }            
      public bool IsTerminated { get; set; }
      public bool IsNeedingPlugOrSocket { get; set; }
      public bool IsMaster { get; set; }


      public BusPinBase Destination
      {
         get { return _destination; }
         set
         {
            if (Equals(value, _destination)) return;
            _destination = value;
            OnPropertyChanged();
         }
      }

      public string DisplayText
      {
         get
         {
            var value = string.Format("{0} - {1}", Name, Address);
            return value;
         }
      }

      public bool IsPlaced { get; set; }


      [field: NonSerialized] // needed: http://stackoverflow.com/questions/7370687/serializing-of-data-binded-observablecollection-in-wpf-propertychangedeventmana
      public event PropertyChangedEventHandler PropertyChanged;

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
   }
}
