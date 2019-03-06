namespace Wiki.Models
{
    public class ViewPostShipment
    {
        int order;
        bool notificationBeforeShipment;
        bool tn;
        bool sf;
        bool n;
        bool delivery;
        bool deliveryClient;
        bool activeReclamation;
        bool controlReport;
        bool controlReportClient;
        string linkView;
        string linkEdit;

        public ViewPostShipment(int order, bool notificationBeforeShipment, bool tn, bool sf, bool n, bool delivery, bool deliveryClient, 
            bool activeReclamation, bool controlReport, bool controlReportClient, string linkView, string linkEdit)
        {
            this.order = order;
            this.notificationBeforeShipment = notificationBeforeShipment;
            this.tn = tn;
            this.sf = sf;
            this.n = n;
            this.delivery = delivery;
            this.deliveryClient = deliveryClient;
            this.activeReclamation = activeReclamation;
            this.controlReport = controlReport;
            this.controlReportClient = controlReportClient;
            this.linkView = linkView;
            this.linkEdit = linkEdit;
        }
    }
}