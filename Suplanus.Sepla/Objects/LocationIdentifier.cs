using System;
using System.Text;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.E3D;
using Suplanus.Sepla.Application;

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
      if (ToString().Equals(obj.ToString()))
      {
        return true;
      }
      else
      {
        return false;
      }
      
    }

    public LocationIdentifier()
    {

    }

    public void SetFromPage(Page page)
    {
      this.FunctionAssignment = GetPageProperty(page.Properties.DESIGNATION_FULLFUNCTIONALASSIGNMENT);
      this.PlaceOfInstallation = GetPageProperty(page.Properties.DESIGNATION_FULLPLACEOFINSTALLATION);
      this.Plant = GetPageProperty(page.Properties.DESIGNATION_FULLPLANT);
      this.Location = GetPageProperty(page.Properties.DESIGNATION_FULLLOCATION);
      this.UserDefinied = GetPageProperty(page.Properties.DESIGNATION_FULLUSERDEFINED);
      this.DocType = GetPageProperty(page.Properties.DESIGNATION_FULLDOCTYPE);
    }

    public void SetFromFunction(Function function)
    {
      this.FunctionAssignment = GetPageProperty(function.Properties.DESIGNATION_FULLFUNCTIONALASSIGNMENT);
      this.PlaceOfInstallation = GetPageProperty(function.Properties.DESIGNATION_FULLPLACEOFINSTALLATION);
      this.Plant = GetPageProperty(function.Properties.DESIGNATION_FULLPLANT);
      this.Location = GetPageProperty(function.Properties.DESIGNATION_FULLLOCATION);
      this.UserDefinied = GetPageProperty(function.Properties.DESIGNATION_FULLUSERDEFINED);
      if (EplanApplicationInfo.GetActiveEplanVersion()>=290)
      {
        this.DocType = GetPageProperty(function.Properties[1520]); // DESIGNATION_FULLDOCTYPE
      }
    }

    public void SetFromInstallationSpace(InstallationSpace installationSpace)
    {
      this.FunctionAssignment = GetPageProperty(installationSpace.Properties.DESIGNATION_FULLFUNCTIONALASSIGNMENT);
      this.PlaceOfInstallation = GetPageProperty(installationSpace.Properties.DESIGNATION_FULLPLACEOFINSTALLATION);
      this.Plant = GetPageProperty(installationSpace.Properties.DESIGNATION_FULLPLANT);
      this.Location = GetPageProperty(installationSpace.Properties.DESIGNATION_FULLLOCATION);
      this.UserDefinied = GetPageProperty(installationSpace.Properties.DESIGNATION_FULLUSERDEFINED);
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
    public virtual string FunctionAssignment { get; set; }

    /// <summary>
    /// Location +
    /// </summary>
    public virtual string Location { get; set; }

    /// <summary>
    /// PlaceOfInstallation ++
    /// </summary>
    public virtual string PlaceOfInstallation { get; set; }

    /// <summary>
    /// Plant (Function) =
    /// </summary>
    public virtual string Plant { get; set; }

    /// <summary>
    /// Userdefinied #
    /// </summary>
    public virtual string UserDefinied { get; set; }

    /// <summary>
    /// DocType &amp;
    /// </summary>
    public virtual string DocType { get; set; }

    /// <summary>
    /// InstallationNumber (empty)
    /// </summary>
    public virtual string InstallationNumber { get; set; }

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

    /// <summary>
    /// All designations except DocType, because not available for installation space in 2.8
    /// </summary>
    /// <param name="pagePropertyListCopy"></param>
    /// <returns></returns>
    public FunctionBasePropertyList GetFunctionBasePropertyListFromPagePropertyList(
      PagePropertyList pagePropertyListCopy)
    {
      var functionBasPropertyList = new FunctionBasePropertyList();
      GetFunctionalAssignment(functionBasPropertyList, pagePropertyListCopy);
      GetPlant(functionBasPropertyList, pagePropertyListCopy);
      GetPlaceOfInstallation(functionBasPropertyList, pagePropertyListCopy);
      GetLocation(functionBasPropertyList, pagePropertyListCopy);
      GetInstallationNumber(functionBasPropertyList, pagePropertyListCopy);
      GetUserDefinied(functionBasPropertyList, pagePropertyListCopy);
      return functionBasPropertyList;
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

    private static void GetFunctionalAssignment(FunctionBasePropertyList functionBasePropertyList, PagePropertyList pagePropertyList)
    {
      if (pagePropertyList != null)
      {
        if (!pagePropertyList.DESIGNATION_FUNCTIONALASSIGNMENT.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_FUNCTIONALASSIGNMENT = pagePropertyList.DESIGNATION_FUNCTIONALASSIGNMENT;
        }

        if (!pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT1.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT1 =
            pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT1;
        }

        if (!pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT2.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT2 =
            pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT2;
        }

        if (!pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT3.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT3 =
            pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT3;
        }

        if (!pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT4.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT4 =
            pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT4;
        }

        if (!pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT5.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT5 =
            pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT5;
        }

        if (!pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT6.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT6 =
            pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT6;
        }

        if (!pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT7.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT7 =
            pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT7;
        }

        if (!pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT8.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT8 =
            pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT8;
        }

        if (!pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT9.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT9 =
            pagePropertyList.DESIGNATION_SUBFUNCTIONALASSIGNMENT9;
        }
      }
    }
    
    private static void GetPlant(FunctionBasePropertyList functionBasePropertyList, PagePropertyList pagePropertyList)
    {
      if (pagePropertyList != null)
      {
        if (!pagePropertyList.DESIGNATION_PLANT.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_PLANT = pagePropertyList.DESIGNATION_PLANT;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLANT1.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLANT1 = pagePropertyList.DESIGNATION_SUBPLANT1;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLANT2.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLANT2 = pagePropertyList.DESIGNATION_SUBPLANT2;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLANT3.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLANT3 = pagePropertyList.DESIGNATION_SUBPLANT3;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLANT4.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLANT4 = pagePropertyList.DESIGNATION_SUBPLANT4;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLANT5.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLANT5 = pagePropertyList.DESIGNATION_SUBPLANT5;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLANT6.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLANT6 = pagePropertyList.DESIGNATION_SUBPLANT6;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLANT7.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLANT7 = pagePropertyList.DESIGNATION_SUBPLANT7;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLANT8.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLANT8 = pagePropertyList.DESIGNATION_SUBPLANT8;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLANT9.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLANT9 = pagePropertyList.DESIGNATION_SUBPLANT9;
        }
      }
    }

    private static void GetPlaceOfInstallation(FunctionBasePropertyList functionBasePropertyList, PagePropertyList pagePropertyList)
    {
      if (pagePropertyList != null)
      {
        if (!pagePropertyList.DESIGNATION_PLACEOFINSTALLATION.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_PLACEOFINSTALLATION = pagePropertyList.DESIGNATION_PLACEOFINSTALLATION;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION1.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION1 =
            pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION1;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION2.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION2 =
            pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION2;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION3.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION3 =
            pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION3;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION4.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION4 =
            pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION4;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION5.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION5 =
            pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION5;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION6.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION6 =
            pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION6;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION7.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION7 =
            pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION7;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION8.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION8 =
            pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION8;
        }

        if (!pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION9.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION9 =
            pagePropertyList.DESIGNATION_SUBPLACEOFINSTALLATION9;
        }
      }
    }

    private static void GetLocation(FunctionBasePropertyList functionBasePropertyList, PagePropertyList pagePropertyList)
    {
      if (pagePropertyList != null)
      {
        if (!pagePropertyList.DESIGNATION_LOCATION.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_LOCATION = pagePropertyList.DESIGNATION_LOCATION;
        }

        if (!pagePropertyList.DESIGNATION_SUBLOCATION1.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBLOCATION1 = pagePropertyList.DESIGNATION_SUBLOCATION1;
        }

        if (!pagePropertyList.DESIGNATION_SUBLOCATION2.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBLOCATION2 = pagePropertyList.DESIGNATION_SUBLOCATION2;
        }

        if (!pagePropertyList.DESIGNATION_SUBLOCATION3.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBLOCATION3 = pagePropertyList.DESIGNATION_SUBLOCATION3;
        }

        if (!pagePropertyList.DESIGNATION_SUBLOCATION4.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBLOCATION4 = pagePropertyList.DESIGNATION_SUBLOCATION4;
        }

        if (!pagePropertyList.DESIGNATION_SUBLOCATION5.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBLOCATION5 = pagePropertyList.DESIGNATION_SUBLOCATION5;
        }

        if (!pagePropertyList.DESIGNATION_SUBLOCATION6.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBLOCATION6 = pagePropertyList.DESIGNATION_SUBLOCATION6;
        }

        if (!pagePropertyList.DESIGNATION_SUBLOCATION7.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBLOCATION7 = pagePropertyList.DESIGNATION_SUBLOCATION7;
        }

        if (!pagePropertyList.DESIGNATION_SUBLOCATION8.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBLOCATION8 = pagePropertyList.DESIGNATION_SUBLOCATION8;
        }

        if (!pagePropertyList.DESIGNATION_SUBLOCATION9.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_SUBLOCATION9 = pagePropertyList.DESIGNATION_SUBLOCATION9;
        }
      }
    }

    private static void GetUserDefinied(FunctionBasePropertyList functionBasePropertyList, PagePropertyList pagePropertyList)
    {
      if (pagePropertyList != null)
      {
        if (!pagePropertyList.DESIGNATION_USERDEFINED.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_USERDEFINED = pagePropertyList.DESIGNATION_USERDEFINED;
        }

        if (!pagePropertyList.DESIGNATION_USERDEFINED_SUB1.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_USERDEFINED_SUB1 = pagePropertyList.DESIGNATION_USERDEFINED_SUB1;
        }

        if (!pagePropertyList.DESIGNATION_USERDEFINED_SUB2.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_USERDEFINED_SUB2 = pagePropertyList.DESIGNATION_USERDEFINED_SUB2;
        }

        if (!pagePropertyList.DESIGNATION_USERDEFINED_SUB3.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_USERDEFINED_SUB3 = pagePropertyList.DESIGNATION_USERDEFINED_SUB3;
        }

        if (!pagePropertyList.DESIGNATION_USERDEFINED_SUB4.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_USERDEFINED_SUB4 = pagePropertyList.DESIGNATION_USERDEFINED_SUB4;
        }

        if (!pagePropertyList.DESIGNATION_USERDEFINED_SUB5.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_USERDEFINED_SUB5 = pagePropertyList.DESIGNATION_USERDEFINED_SUB5;
        }

        if (!pagePropertyList.DESIGNATION_USERDEFINED_SUB6.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_USERDEFINED_SUB6 = pagePropertyList.DESIGNATION_USERDEFINED_SUB6;
        }

        if (!pagePropertyList.DESIGNATION_USERDEFINED_SUB7.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_USERDEFINED_SUB7 = pagePropertyList.DESIGNATION_USERDEFINED_SUB7;
        }

        if (!pagePropertyList.DESIGNATION_USERDEFINED_SUB8.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_USERDEFINED_SUB8 = pagePropertyList.DESIGNATION_USERDEFINED_SUB8;
        }

        if (!pagePropertyList.DESIGNATION_USERDEFINED_SUB9.IsEmpty)
        {
          functionBasePropertyList.DESIGNATION_USERDEFINED_SUB9 = pagePropertyList.DESIGNATION_USERDEFINED_SUB9;
        }
      }
    }

    private static void GetInstallationNumber(FunctionBasePropertyList functionBasePropertyList, PagePropertyList pagePropertyList)
    {
      if (pagePropertyList != null &&
          !pagePropertyList.DESIGNATION_INSTALLATIONNUMBER.IsEmpty)
      {
        // There is no sub installation number
        functionBasePropertyList.DESIGNATION_INSTALLATIONNUMBER = pagePropertyList.DESIGNATION_INSTALLATIONNUMBER;
      }
    }
  }
}