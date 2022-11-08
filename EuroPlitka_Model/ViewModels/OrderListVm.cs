namespace EuroPlitka_Model.ViewModels
{
    public class OrderListVm
    {
        public IEnumerable<OrderHeader>? OrderHeaderList { get; set; }
        public string IsReset { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

    }
}
