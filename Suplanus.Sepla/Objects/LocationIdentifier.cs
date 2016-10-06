using System.Text;
using Eplan.EplApi.DataModel;
using Fclp.Internals.Extensions;

namespace Suplanus.Sepla.Objects
{
	public class LocationIdentifier : ILocationIdentifier
	{
		public string FunctionAssignment { get; set; }
		public string Location { get; set; }
		public string PlaceOfInstallation { get; set;}
		public string Plant { get; set; }
		public string UserDefinied { get; set; }
	   public string DocType { get; set; }
	   public string InstallationNumber { get; set; }

      public override string ToString()
      {
         StringBuilder sb = new StringBuilder();
         AddProperty(FunctionAssignment, "==", sb);
         AddProperty(Plant, "=", sb);
         AddProperty(PlaceOfInstallation, "++", sb);
         AddProperty(Location, "+", sb);
         AddProperty(UserDefinied, "#", sb);
         AddProperty(DocType, "&", sb);
         AddProperty(InstallationNumber, "$", sb);
         return sb.ToString();
      }

      private static void AddProperty(string value, string prefix, StringBuilder sb)
      {
         if (!string.IsNullOrEmpty(value))
         {
            sb.Append(prefix + value);
         }
      }

      public PagePropertyList CopyPagePropertyList(PagePropertyList pagePropertyListSource)
	   {
         PagePropertyList pagePropertylistCopy = pagePropertyListSource;
         GetPagePropertyList(pagePropertylistCopy);
	      return pagePropertylistCopy;
	   }

	   public PagePropertyList GetPagePropertyList()
	   {
	      PagePropertyList pagePropertyList = new PagePropertyList();
	      return GetPagePropertyList(pagePropertyList);
	   }

	   private PagePropertyList GetPagePropertyList(PagePropertyList pagePropertylistCopy)
	   {
	      if (!this.FunctionAssignment.IsNullOrEmpty())
	      {
	         pagePropertylistCopy.DESIGNATION_FUNCTIONALASSIGNMENT = this.FunctionAssignment;
	      }
	      if (!this.PlaceOfInstallation.IsNullOrEmpty())
	      {
	         pagePropertylistCopy.DESIGNATION_PLACEOFINSTALLATION = this.PlaceOfInstallation;
	      }
	      if (!this.Plant.IsNullOrEmpty())
	      {
	         pagePropertylistCopy.DESIGNATION_PLANT = this.Plant;
	      }
	      if (!this.Location.IsNullOrEmpty())
	      {
	         pagePropertylistCopy.DESIGNATION_LOCATION = this.Location;
	      }
	      if (!this.UserDefinied.IsNullOrEmpty())
	      {
	         pagePropertylistCopy.DESIGNATION_USERDEFINED = this.UserDefinied;
	      }
	      if (!this.DocType.IsNullOrEmpty())
	      {
	         pagePropertylistCopy.DESIGNATION_DOCTYPE = this.DocType;
	      }
	      if (!this.InstallationNumber.IsNullOrEmpty())
	      {
	         pagePropertylistCopy.DESIGNATION_INSTALLATIONNUMBER = this.InstallationNumber;
	      }

	      return pagePropertylistCopy;

	   }
	}
}