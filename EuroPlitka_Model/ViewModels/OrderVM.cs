﻿namespace EuroPlitka_Model.ViewModels
{
    public class OrderVM
    {

        public OrderHeader? OrderHeader { get; set; }
        public IEnumerable<OrderDetail>? OrderDetails { get; set; }

    }
}
