//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wiki
{
    using System;
    using System.Collections.Generic;
    
    public partial class ResultGraphicStatusPlanZakaz
    {
        public Nullable<int> id_PZ_PlanZakaz { get; set; }
        public int Заказ { get; set; }
        public string Изделие { get; set; }
        public string Тр_р { get; set; }
        public string Модуль { get; set; }
        public string Заказчик { get; set; }
        public System.DateTime Договор { get; set; }
        public Nullable<int> Откл_ { get; set; }
        public Nullable<System.DateTime> Отгрузка { get; set; }
        public Nullable<System.DateTime> dataRezresheniyaProizvodstva { get; set; }
        public Nullable<System.DateTime> Дата_состояния_РКД { get; set; }
        public string Текущая_версия_РКД { get; set; }
        public string Текущая_стадия_состояния_РКД { get; set; }
        public Nullable<double> С_С_1ед_ { get; set; }
        public Nullable<double> Договорная_цена { get; set; }
        public Nullable<double> C__КБМ { get; set; }
        public Nullable<double> C__КБЭ { get; set; }
        public Nullable<double> C__ПО { get; set; }
        public Nullable<double> C__ОС { get; set; }
        public Nullable<double> Buj { get; set; }
        public Nullable<double> C__Бюджета { get; set; }
        public string Валюта { get; set; }
        public Nullable<System.Guid> ProjectUID { get; set; }
        public string graphicColumnName { get; set; }
        public Nullable<decimal> TaskWork { get; set; }
        public Nullable<decimal> TaskActualWork { get; set; }
        public Nullable<double> PercentComplited { get; set; }
        public Nullable<System.DateTime> TaskStartDate { get; set; }
        public Nullable<System.DateTime> TaskFinishDate { get; set; }
    }
}
