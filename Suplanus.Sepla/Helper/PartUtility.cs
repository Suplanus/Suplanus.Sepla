using System.Linq;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.E3D;
using Eplan.EplApi.HEServices;
using Eplan.EplApi.MasterData;
using Eplan.EplApi.System;

namespace Suplanus.Sepla.Helper
{
  public class PartUtility
  {
    public static MDPart SelectPartWithGui()
    {
      EplApplication eplanApplication = new EplApplication();
      MDPartsManagement partsManagement = new MDPartsManagement();
      string partnNumber = string.Empty;
      string partVariant = string.Empty;
      eplanApplication.ShowPartSelectionDialog(ref partnNumber, ref partVariant);
      MDPartsDatabase partsDatabase = partsManagement.OpenDatabase();
      MDPart part = partsDatabase.GetPart(partnNumber, partVariant);
      return part;
    }

    public static MDPart CreateOrUpdatePart(ArticleReference articleReference, bool updateFunctionTemplate)
    {
      // Need to lock project
      var project = articleReference.Project;
      project.SmartLock();
      //if (articleReference.ParentObject != null) articleReference.ParentObject.SmartLock();
      articleReference.SmartLock();

      // Init
      var partsDatabase = new MDPartsManagement().OpenDatabase();
      //var articleReference = function.ArticleReferences.First();
      articleReference.SmartLock();
      var partNr = articleReference.PartNr;
      var partVariant = articleReference.VariantNr;
      MDPart part = partsDatabase.GetPart(partNr, partVariant);

      // Check if article is in project and remove, because the eplan action to create is not possible
      var existingArticle = project.Articles
          .FirstOrDefault(obj =>
              obj.PartNr.Equals(partNr) && obj.VariantNr.Equals(partVariant)
          );
      if (existingArticle != null)
      {
        existingArticle.SmartLock();
        existingArticle.Remove();
      }

      // Need to focus again if its lost
      if (articleReference.ParentObject is Placement placementToBringInFront)
      {
        new Edit().BringToFront(placementToBringInFront);
      }

      // Create new part
      if (part == null)
      {
        // LockingVector is needed because of locking exception from EPLAN action (no catch possible)
        using (new LockingUtility.SeplaLockingVector())
        {
          new CommandLineInterpreter().Execute("XPameCreateType"); 
        }
        
        partsDatabase = new MDPartsManagement().OpenDatabase(); // Second Call needed to get new part
        part = partsDatabase.GetPart(partNr, partVariant);
      }
      // Existing part
      else
      {
        // Check if pro panel, because there is no update possible
        bool isProPanel = articleReference.ParentObject is Function3D;

        string partNrTemp = partNr;
        if (!isProPanel)
        {
          // Rename part
          string suffix = "_temp";
          partNrTemp = part.PartNr + suffix;
          try
          {
            articleReference.PartNr = partNrTemp;
            articleReference.ParentObject.SmartLock();
            articleReference.StoreToObject();

            // Quiet create temp part
            var application = new EplApplication();
            var quiteMode = application.QuietMode;
            application.QuietMode = EplApplication.QuietModes.ShowNoDialogs;
            using (new LockingUtility.SeplaLockingVector())
            {
              new CommandLineInterpreter().Execute("XPameCreateType");
            }
            application.QuietMode = quiteMode;
          }
          finally
          {
            // Rename back
            articleReference.PartNr = partNr;
            articleReference.StoreToObject();
          }

          //Get temp part for copy functionTemplate and remove
          partsDatabase = new MDPartsManagement().OpenDatabase(); // Second Call needed to get new part
          MDPart partDuplicate = partsDatabase.GetPart(partNrTemp, partVariant);

          // Copy FunctionTemplate
          if (updateFunctionTemplate)
          {
            foreach (var partFunctionTemplatePosition in part.FunctionTemplatePositions)
            {
              part.RemoveFunctionTemplatePosition(partFunctionTemplatePosition);
            }
            foreach (var partDuplicateFunctionTemplatePosition in partDuplicate.FunctionTemplatePositions)
            {
              part.AddFunctionTemplatePosition(partDuplicateFunctionTemplatePosition);
            }
          }
          partsDatabase.RemovePart(partDuplicate);
        }
        
        // Check if article is in project
        var existingTempArticle = project.Articles
            .FirstOrDefault(obj =>
            obj.PartNr.Equals(partNrTemp) && obj.VariantNr.Equals(partVariant)
            );
        if (existingTempArticle != null)
        {
          existingTempArticle.SmartLock();
          existingTempArticle.Remove();
        }
      }

      // Load data
      var article = project.Articles
          .FirstOrDefault(obj =>
              obj.PartNr.Equals(partNr) && obj.VariantNr.Equals(partVariant)
          );
      if (article != null)
      {
        article.SmartLock();
        article.LoadFromMasterdata();
      }

      return part;
    }


 
  }
}