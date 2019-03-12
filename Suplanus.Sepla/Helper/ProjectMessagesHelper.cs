using System.Collections.Generic;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.EServices;

namespace Suplanus.Sepla.Helper
{
  public class ProjectMessagesHelper
  {
    public static List<ProjectMessage> GetProjectMessages(Project project)
    {
      List<ProjectMessage> projectMessages = new List<ProjectMessage>();
      PrjMessagesCollection messagesCollection = new PrjMessagesCollection(project);
      PrjMessagesEnumerator itPrjMsg = messagesCollection.GetPrjMsgEnumerator();
      itPrjMsg.MoveNext();
      do
      {
        if (itPrjMsg.Current is ProjectMessage oPrjMsg)
        {
          projectMessages.Add(oPrjMsg);
        }
      }
      while (itPrjMsg.MoveNext());

      return projectMessages;
    }
  }
}
