using X.PagedList;

namespace HomematicApp.ViewModels
{
	public class ActionListModel
	{
		public IPagedList<ActionModel> Actions { get; set; }
		public IPagedList<EspDataModel> EspData { get; set; }
	}
}
