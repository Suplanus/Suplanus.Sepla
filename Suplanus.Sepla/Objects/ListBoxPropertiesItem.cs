using System;
using System.Collections.Generic;
using System.Linq;
using Eplan.EplApi.DataModel.Graphics;

namespace Suplanus.Sepla.Objects
{

    public class ListBoxPropertiesItem : IEquatable<ListBoxPropertiesItem>
    {

        public string Description { get; set; }
        public string Value { get; set; }


        // distinct only works with override and IEquatable
        public bool Equals(ListBoxPropertiesItem other)
        {
            if (Description == other.Description)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hashFirstName;
            if (Description == null)
            {
                hashFirstName = 0;
            }
            else
            {
                hashFirstName = Description.GetHashCode();
            }
            return hashFirstName;
        }
    }

    public class ListBoxPlaceholderItem
    {
        public ListBoxPlaceholderItem(PlaceHolder placeHolder)
        {
            Records = placeHolder.GetRecordNames().Cast<string>().ToList();
            Description = placeHolder.Name;
            PlaceHolderObject = placeHolder;
        }

        public PlaceHolder PlaceHolderObject { get; set; }

        public string Description { get; set; }

        public List<string> Records { get; set; }
       
        public string SelectedRecord { get; set; }
    }


}
