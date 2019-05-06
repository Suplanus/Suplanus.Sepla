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
      PrjMessagesEnumerator projEnumerator = messagesCollection.GetPrjMsgEnumerator();
      projEnumerator.MoveNext();
      do
      {
        if (projEnumerator.Current is ProjectMessage projectMessage)
        {
          projectMessages.Add(projectMessage);
        }
      }
      while (projEnumerator.MoveNext());

      return projectMessages;
    }
  }
}
