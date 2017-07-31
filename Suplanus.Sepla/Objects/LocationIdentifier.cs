using System;
using System.Text;
using Eplan.EplApi.DataModel;

namespace Suplanus.Sepla.Objects
{
   /// <summary>
   /// LocationIdentifier
   /// </summary>
   [Serializable]
   public class LocationIdentifier : ILocationIdentifier
   {
      public bool Equals(LocationIdentifier obj)
      {
         return Equals(obj.ToString().Equals(this.ToString()));
      }

      public LocationIdentifier()
      {

      }

      public LocationIdentifier(PagePropertyList pagePropertyList)
      {
         this.FunctionAssignment = GetPageProperty(pagePropertyList.DESIGNATION_FUNCTIONALASSIGNMENT);
         this.PlaceOfInstallation = GetPageProperty(pagePropertyList.DESIGNATION_PLACEOFINSTALLATION);
         this.Plant = GetPageProperty(pagePropertyList.DESIGNATION_PLANT);
         this.Location = GetPageProperty(pagePropertyList.DESIGNATION_LOCATION);
         this.UserDefinied = GetPageProperty(pagePropertyList.DESIGNATION_USERDEFINED);
         this.DocType= GetPageProperty(pagePropertyList.DESIGNATION_DOCTYPE);
         // todo add sub locations
      }

      private string GetPageProperty(PropertyValue propertyValue)
      {
         if (!propertyValue.IsEmpty)
         {
            return propertyValue;
         }
         return null;
      }

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
         GetFunctionalAssignment(pagePropertylistCopy, this.FunctionAssignment);
         GetPlant(pagePropertylistCopy, this.Plant);
         GetPlaceOfInstallation(pagePropertylistCopy, this.PlaceOfInstallation);
         GetLocation(pagePropertylistCopy, this.Location);
         GetUserDefinied(pagePropertylistCopy, this.UserDefinied);
         GetDocumentType(pagePropertylistCopy, this.DocType);
         GetInstallationNumber(pagePropertylistCopy, this.InstallationNumber);
         return pagePropertylistCopy;
      }

      private static void GetFunctionalAssignment(PagePropertyList pagePropertylist, string location)
      {
         if (!string.IsNullOrEmpty(location))
         {
            var split = location.Split('.');
            for (int index = 0; index < split.Length; index++)
            {
               var s = split[index];
               switch (index)
               {
                  case 0:
                     pagePropertylist.DESIGNATION_FUNCTIONALASSIGNMENT = s;
                     break;
                  case 1:
                     pagePropertylist.DESIGNATION_SUBFUNCTIONALASSIGNMENT1 = s;
                     break;
                  case 2:
                     pagePropertylist.DESIGNATION_SUBFUNCTIONALASSIGNMENT2 = s;
                     break;
                  case 3:
                     pagePropertylist.DESIGNATION_SUBFUNCTIONALASSIGNMENT3 = s;
                     break;
                  case 4:
                     pagePropertylist.DESIGNATION_SUBFUNCTIONALASSIGNMENT4 = s;
                     break;
                  case 5:
                     pagePropertylist.DESIGNATION_SUBFUNCTIONALASSIGNMENT5 = s;
                     break;
                  case 6:
                     pagePropertylist.DESIGNATION_SUBFUNCTIONALASSIGNMENT6 = s;
                     break;
                  case 7:
                     pagePropertylist.DESIGNATION_SUBFUNCTIONALASSIGNMENT7 = s;
                     break;
                  case 8:
                     pagePropertylist.DESIGNATION_SUBFUNCTIONALASSIGNMENT8 = s;
                     break;
                  case 9:
                     pagePropertylist.DESIGNATION_SUBFUNCTIONALASSIGNMENT9 = s;
                     break;
                  default:
                     throw new Exception("Only 9 sub locations allowed: + " + location);
               }
            }
         }
      }

      private static void GetPlant(PagePropertyList pagePropertylist, string location)
      {
         if (!string.IsNullOrEmpty(location))
         {
            var split = location.Split('.');
            for (int index = 0; index < split.Length; index++)
            {
               var s = split[index];
               switch (index)
               {
                  case 0:
                     pagePropertylist.DESIGNATION_PLANT = s;
                     break;
                  case 1:
                     pagePropertylist.DESIGNATION_SUBPLANT1 = s;
                     break;
                  case 2:
                     pagePropertylist.DESIGNATION_SUBPLANT2 = s;
                     break;
                  case 3:
                     pagePropertylist.DESIGNATION_SUBPLANT3 = s;
                     break;
                  case 4:
                     pagePropertylist.DESIGNATION_SUBPLANT4 = s;
                     break;
                  case 5:
                     pagePropertylist.DESIGNATION_SUBPLANT5 = s;
                     break;
                  case 6:
                     pagePropertylist.DESIGNATION_SUBPLANT6 = s;
                     break;
                  case 7:
                     pagePropertylist.DESIGNATION_SUBPLANT7 = s;
                     break;
                  case 8:
                     pagePropertylist.DESIGNATION_SUBPLANT8 = s;
                     break;
                  case 9:
                     pagePropertylist.DESIGNATION_SUBPLANT9 = s;
                     break;
                  default:
                     throw new Exception("Only 9 sub locations allowed: + " + location);
               }
            }
         }
      }

      private static void GetPlaceOfInstallation(PagePropertyList pagePropertylist, string location)
      {
         if (!string.IsNullOrEmpty(location))
         {
            var split = location.Split('.');
            for (int index = 0; index < split.Length; index++)
            {
               var s = split[index];
               switch (index)
               {
                  case 0:
                     pagePropertylist.DESIGNATION_PLACEOFINSTALLATION = s;
                     break;
                  case 1:
                     pagePropertylist.DESIGNATION_SUBPLACEOFINSTALLATION1 = s;
                     break;
                  case 2:
                     pagePropertylist.DESIGNATION_SUBPLACEOFINSTALLATION2 = s;
                     break;
                  case 3:
                     pagePropertylist.DESIGNATION_SUBPLACEOFINSTALLATION3 = s;
                     break;
                  case 4:
                     pagePropertylist.DESIGNATION_SUBPLACEOFINSTALLATION4 = s;
                     break;
                  case 5:
                     pagePropertylist.DESIGNATION_SUBPLACEOFINSTALLATION5 = s;
                     break;
                  case 6:
                     pagePropertylist.DESIGNATION_SUBPLACEOFINSTALLATION6 = s;
                     break;
                  case 7:
                     pagePropertylist.DESIGNATION_SUBPLACEOFINSTALLATION7 = s;
                     break;
                  case 8:
                     pagePropertylist.DESIGNATION_SUBPLACEOFINSTALLATION8 = s;
                     break;
                  case 9:
                     pagePropertylist.DESIGNATION_SUBPLACEOFINSTALLATION9 = s;
                     break;
                  default:
                     throw new Exception("Only 9 sub locations allowed: + " + location);
               }
            }
         }
      }

      private static void GetLocation(PagePropertyList pagePropertylist, string location)
      {
         if (!string.IsNullOrEmpty(location))
         {
            var split = location.Split('.');
            for (int index = 0; index < split.Length; index++)
            {
               var s = split[index];
               switch (index)
               {
                  case 0:
                     pagePropertylist.DESIGNATION_LOCATION = s;
                     break;
                  case 1:
                     pagePropertylist.DESIGNATION_SUBLOCATION1 = s;
                     break;
                  case 2:
                     pagePropertylist.DESIGNATION_SUBLOCATION2 = s;
                     break;
                  case 3:
                     pagePropertylist.DESIGNATION_SUBLOCATION3 = s;
                     break;
                  case 4:
                     pagePropertylist.DESIGNATION_SUBLOCATION4 = s;
                     break;
                  case 5:
                     pagePropertylist.DESIGNATION_SUBLOCATION5 = s;
                     break;
                  case 6:
                     pagePropertylist.DESIGNATION_SUBLOCATION6 = s;
                     break;
                  case 7:
                     pagePropertylist.DESIGNATION_SUBLOCATION7 = s;
                     break;
                  case 8:
                     pagePropertylist.DESIGNATION_SUBLOCATION8 = s;
                     break;
                  case 9:
                     pagePropertylist.DESIGNATION_SUBLOCATION9 = s;
                     break;
                  default:
                     throw new Exception("Only 9 sub locations allowed: + " + location);
               }
            }
         }
      }

      private static void GetUserDefinied(PagePropertyList pagePropertylist, string location)
      {
         if (!string.IsNullOrEmpty(location))
         {
            var split = location.Split('.');
            for (int index = 0; index < split.Length; index++)
            {
               var s = split[index];
               switch (index)
               {
                  case 0:
                     pagePropertylist.DESIGNATION_USERDEFINED = s;
                     break;
                  case 1:
                     pagePropertylist.DESIGNATION_USERDEFINED_SUB1 = s;
                     break;
                  case 2:
                     pagePropertylist.DESIGNATION_USERDEFINED_SUB2 = s;
                     break;
                  case 3:
                     pagePropertylist.DESIGNATION_USERDEFINED_SUB3 = s;
                     break;
                  case 4:
                     pagePropertylist.DESIGNATION_USERDEFINED_SUB4 = s;
                     break;
                  case 5:
                     pagePropertylist.DESIGNATION_USERDEFINED_SUB5 = s;
                     break;
                  case 6:
                     pagePropertylist.DESIGNATION_USERDEFINED_SUB6 = s;
                     break;
                  case 7:
                     pagePropertylist.DESIGNATION_USERDEFINED_SUB7 = s;
                     break;
                  case 8:
                     pagePropertylist.DESIGNATION_USERDEFINED_SUB8 = s;
                     break;
                  case 9:
                     pagePropertylist.DESIGNATION_USERDEFINED_SUB9 = s;
                     break;
                  default:
                     throw new Exception("Only 9 sub locations allowed: + " + location);
               }
            }
         }
      }

      private static void GetDocumentType(PagePropertyList pagePropertylist, string location)
      {
         if (!string.IsNullOrEmpty(location))
         {
            // There is no sub doctype
            pagePropertylist.DESIGNATION_DOCTYPE = location;
         }
      }

      private static void GetInstallationNumber(PagePropertyList pagePropertylist, string location)
      {
         if (!string.IsNullOrEmpty(location))
         {
            // There is no sub installation number
            pagePropertylist.DESIGNATION_INSTALLATIONNUMBER = location;
         }
      }

   }
}