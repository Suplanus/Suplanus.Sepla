using System.Linq;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
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

        public static MDPart CreateOrUpdateWithFunctionTemplate(Function function)
        {
            // Need to lock project
            var project = function.Project;
            project.SmartLock();
            function.SmartLock();

            // Init
            var partsDatabase = new MDPartsManagement().OpenDatabase();
            var articleReference = function.ArticleReferences.First();
            articleReference.SmartLock();
            var partNr = articleReference.PartNr;
            var partVariant = articleReference.VariantNr;
            MDPart part = partsDatabase.GetPart(partNr, partVariant);

            // Create new part
            if (part == null)
            {
                // LockingVector is needed because of locking exception from EPLAN action (no catch possible)
                LockingVector lockingVector = new LockingVector();
                int stateId = lockingVector.PauseManualLock();
                new CommandLineInterpreter().Execute("XPameCreateType");
                lockingVector.ResumeManualLock(stateId);

                partsDatabase = new MDPartsManagement().OpenDatabase(); // Second Call needed to get new part
                part = partsDatabase.GetPart(partNr, partVariant);
            }
            else
            {
                // Rename part
                string suffix = "_temp";
                string partNrTemp = part.PartNr + suffix;
                try
                {
                    articleReference.PartNr = partNrTemp;
                    articleReference.StoreToObject();

                    // Quiet create temp part
                    var application = new EplApplication();
                    var quiteMode = application.QuietMode;
                    application.QuietMode = EplApplication.QuietModes.ShowNoDialogs;
                    new CommandLineInterpreter().Execute("XPameCreateType");
                    application.QuietMode = quiteMode;
                }
                finally
                {
                    // Rename back
                    articleReference.PartNr = partNr;
                    articleReference.StoreToObject();
                }

                // Copy FunctionTemplate
                partsDatabase = new MDPartsManagement().OpenDatabase(); // Second Call needed to get new part
                MDPart partDuplicate = partsDatabase.GetPart(partNrTemp, partVariant);
                foreach (var partFunctionTemplatePosition in part.FunctionTemplatePositions)
                {
                    part.RemoveFunctionTemplatePosition(partFunctionTemplatePosition);
                }
                foreach (var partDuplicateFunctionTemplatePosition in partDuplicate.FunctionTemplatePositions)
                {
                    part.AddFunctionTemplatePosition(partDuplicateFunctionTemplatePosition);
                }

                partsDatabase.RemovePart(partDuplicate);


                // Check if article is in project
                var existingArticle = project.Articles
                    .FirstOrDefault(obj =>
                    obj.PartNr.Equals(partNrTemp) && obj.VariantNr.Equals(partVariant)
                    );
                if (existingArticle != null)
                {
                    existingArticle.SmartLock();
                    existingArticle.Remove();
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