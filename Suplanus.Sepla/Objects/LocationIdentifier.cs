using System.Text;
using Eplan.EplApi.DataModel;
using Fclp.Internals.Extensions;

namespace Suplanus.Sepla.Objects
{
	/// <summary>
	/// LocationIdentifier
	/// </summary>
	public class LocationIdentifier : ILocationIdentifier
	{
      /// <summary>
      /// FunctionalAssignment ==
      /// </summary>
      public string FunctionAssignment { get; set; }

      /// <summary>
      /// Location +
      /// </summary>
      public string Location { get; set; }

      /// <summary>
      /// PlaceOfInstallation ++
      /// </summary>
		public string PlaceOfInstallation { get; set; }

      /// <summary>
      /// Plant (Function) =
      /// </summary>
		public string Plant { get; set; }

      /// <summary>
      /// Userdefinied #
      /// </summary>
		public string UserDefinied { get; set; }

      /// <summary>
      /// DocType &amp;
      /// </summary>
      public string DocType { get; set; }

      /// <summary>
      /// InstallationNumber (empty)
      /// </summary>
	   public string InstallationNumber { get; set; }

      /// <summary>
      /// Returns the full location in a string
      /// </summary>
      /// <returns></returns>
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

      /// <summary>
      /// Copies a PagePropertyList and insert the locations
      /// </summary>
      /// <param name="pagePropertyListSource">PagePropertyList to copy</param>
      /// <returns>Duplicate of PagePropertyList</returns>
      public PagePropertyList CopyPagePropertyList(PagePropertyList pagePropertyListSource)
	   {
         PagePropertyList pagePropertylistCopy = pagePropertyListSource;
         GetPagePropertyList(pagePropertylistCopy);
	      return pagePropertylistCopy;
	   }

      /// <summary>
      /// Gets a PagePropertylist with the locations
      /// </summary>
      /// <returns>Duplicate of PagePropertyList</returns>
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