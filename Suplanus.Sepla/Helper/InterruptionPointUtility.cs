using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
	/// <summary>
	/// Helper class for InterruptionPoints
	/// </summary>
	public class InterruptionPointUtility
	{
      /// <summary>
      /// Renames the InterruptionPoint
      /// </summary>
      /// <param name="interruptionPoint"></param>
      /// <param name="functionBasePropertyList"></param>
		public static void RenameAll(InterruptionPoint interruptionPoint, FunctionBasePropertyList functionBasePropertyList)
		{
			// Get all Interruptionpoints
			DMObjectsFinder objectsFinder = new DMObjectsFinder(interruptionPoint.Project);
			InterruptionPointsFilter interruptionPointsFilter = new InterruptionPointsFilter();
			interruptionPointsFilter.Name = interruptionPoint.Name;
			InterruptionPoint[] interruptionPoints = objectsFinder.GetInterruptionPoints(interruptionPointsFilter);

			// Device tag			
			foreach (InterruptionPoint ip in interruptionPoints)
			{
				NameService nameService = new NameService(ip.Page);
				ip.LockObject();				
				ip.NameParts = functionBasePropertyList;
				nameService.AdjustVisibleName(ip);
			}
		}
	}
}
