using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Order
{
    public class Orders
    {
        public string Order_id { get; set; }
        public string Product_id { get; set; }
        public string User_id { get; set; }
        public OrderConfiguration Order_configuration { get; set; }
        public string Side { get; set; }
        public string Client_order_id { get; set; }
        public string Status { get; set; }
        public string Time_in_force { get; set; }
        public DateTime Created_time { get; set; }
        public string Completion_percentage { get; set; }
        public string Filled_size { get; set; }
        public string Average_filled_price { get; set; }
        public string Fee { get; set; }
        public string Number_of_fills { get; set; }
        public string Filled_value { get; set; }
        public bool Pending_cancel { get; set; }
        public bool Size_in_quote { get; set; }
        public string Total_fees { get; set; }
        public bool Size_inclusive_of_fees { get; set; }
        public string Total_value_after_fees { get; set; }
        public string Trigger_status { get; set; }
        public string Order_type { get; set; }
        public string Reject_reason { get; set; }
        public string Settled { get; set; }
        public string Product_type { get; set; }
        public string Reject_message { get; set; }
        public string Cancel_message { get; set; }
        public string Order_placement_source { get; set; }
        public string Outstanding_hold_amount { get; set; }
        public string Is_liquidation { get; set; }
    }
}
