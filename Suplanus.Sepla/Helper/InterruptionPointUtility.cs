using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
	public class InterruptionPointUtility
	{
		public static void RenameAll(InterruptionPoint interruptionPoint, FunctionBasePropertyList functionBasePropertyList)
		{
			// Get all Interruptionpoints
			DMObjectsFinder objectsFinder = new DMObjectsFinder(interruptionPoint.Project);
			InterruptionPointsFilter interruptionPointsFilter = new InterruptionPointsFilter();
			interruptionPointsFilter.Name = interruptionPoint.Name;
			InterruptionPoint[] interruptionPoints = objectsFinder.GetInterruptionPoints(interruptionPointsFilter);

			// Device tag
			NameService nameService = new NameService(interruptionPoint.Page);
			foreach (InterruptionPoint ip in interruptionPoints)
			{
				ip.LockObject();
				ip.NameParts = functionBasePropertyList;
				nameService.AdjustVisibleName(ip);
			}
		}
	}
}
