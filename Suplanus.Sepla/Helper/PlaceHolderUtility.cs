using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.Graphics;

namespace Suplanus.Sepla.Helper
{
  /// <summary>
  /// Placeholder Helper Class
  /// </summary>
  public class PlaceHolderUtility
  {
    /// <summary>
    /// Apply a record to placeholders by name
    /// </summary>
    /// <param name="placeHolders">List of placeholder which should be applied</param>
    /// <param name="placeHolderName">Name of placeholder</param>
    /// <param name="recordName">Name of record</param>
    /// <returns>Return true if one or more was applied</returns>
    public static bool ApplyRecord(IEnumerable<PlaceHolder> placeHolders, string placeHolderName, string recordName)
    {
      List<PlaceHolder> foundPlaceHolder = placeHolders
          .Where(placeHolder => placeHolder.Name.Equals(placeHolderName)) // name
          .Where(placeHolder => placeHolder.FindRecord(recordName) != -1) // record
          .ToList();

      ApplyRecord(foundPlaceHolder, recordName);

      return foundPlaceHolder.Any(); // true == found | false == not found
    }

    /// <summary>
    /// Apply a record to placeholders
    /// </summary>
    /// <param name="placeHolders">List of placeholder which should be applied</param>
    /// <param name="recordName">Name of record</param>
    /// <returns>Return true if one or more was applied</returns>
    public static void ApplyRecord(IEnumerable<PlaceHolder> placeHolders, string recordName)
    {
      try
      {
        foreach (PlaceHolder placeHolder in placeHolders)
        {
          placeHolder.ApplyRecord(recordName, placeHolder.ArePagePropertiesDisplayed); // apply (with page data)
        }
      }
      catch (System.Exception exception)
      {
        MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
      }
    }



    /// <summary>
    /// Create a record to a placeHolder by name and apply a record
    /// </summary>
    /// <param name="placeHolders">List of placeholder which should be applied</param>
    /// <param name="placeHolderName">Name of placeholder</param>
    /// <param name="recordName">Name of record</param>
    /// <param name="variableName">Variable to set</param>
    /// <param name="value">New value of the variable</param>
    /// <returns>Return true if one or more was applied</returns>
    public bool CreateRecordWithValueAndApply(PlaceHolder[] placeHolders, string placeHolderName, string recordName, string variableName, string value)
    {
      List<PlaceHolder> foundPlaceHolder = placeHolders
          .Where(placeHolder => placeHolder.Name.Equals(placeHolderName)) // name
          .ToList();

      foreach (PlaceHolder placeHolder in foundPlaceHolder)
      {
        placeHolder.AddRecord(recordName);
        placeHolder.set_Value(recordName, variableName, value);
        using (Transaction transaction = new TransactionManager().CreateTransaction())
        {
          placeHolder.ApplyRecord(recordName, true); // apply (with page data)
          transaction.Commit(); // needed if not placed in project
        }
      }

      return foundPlaceHolder.Any(); // true == found | false == not found
    }


  }
}
