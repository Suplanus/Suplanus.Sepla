using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;

namespace Suplanus.Sepla.Helper
{
  public class ExceptionHelper
  {
    public static void HandleException(Exception exception, string name, UndoStep undoStep = null)
    {
      Decider decider = new Decider();

      string message;
      if (undoStep == null)
      {
        message = "Es ist ein Fehler beim Erzeugen aufgetreten:";
      }
      else
      {
        message = "Es ist ein Fehler beim Erzeugen aufgetreten. Soll die Aktion rückgängig gemacht werden?";
      }

      string errorText;
      StringBuilder sb = new StringBuilder();
      switch (exception)
      {
        case LockingException e:
          string[] failedLockingObjects = e.GetAllFailed2LockAsString();
          foreach (var id in failedLockingObjects)
          {
            var storableObject = StorableObject.FromStringIdentifier(id);
            sb.AppendLine(storableObject.GetType().ToString());
          }
          
          errorText = $"{message}{Environment.NewLine}{exception.Message}{Environment.NewLine}{Environment.NewLine}{sb}{Environment.NewLine}";
          break;

        default:
          errorText =$"{message}{Environment.NewLine}{exception.Message}{Environment.NewLine}{Environment.NewLine}";
          break;
      }

      // Log
      BaseException bexError = new BaseException($"{name}: {errorText}{Environment.NewLine}{exception}", MessageLevel.Error);
      bexError.FixMessage();

      // Show
      EnumDecisionReturn decision;
      if (undoStep != null)
      {
        decision = decider.Decide(EnumDecisionType.eYesNoDecision, errorText,
          name, EnumDecisionReturn.eYES,
          EnumDecisionReturn.eYES, name, false, EnumDecisionIcon.eFATALERROR);
      }
      else
      {
        decision = decider.Decide(EnumDecisionType.eOkDecision, errorText,
          name, EnumDecisionReturn.eOK,
          EnumDecisionReturn.eOK, name, false, EnumDecisionIcon.eFATALERROR);
      }
      switch (decision)
      {
        case EnumDecisionReturn.eYES:
          undoStep?.DoUndo();
          break;
      }
    }
  }
}
