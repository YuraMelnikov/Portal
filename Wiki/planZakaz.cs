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
    
    public partial class planZakaz
    {
        public int id { get; set; }
        public Nullable<int> Zakaz { get; set; }
        public Nullable<System.DateTime> Otgruzka { get; set; }
        public Nullable<System.DateTime> Otkr { get; set; }
        public Nullable<int> SSmat { get; set; }
        public Nullable<int> bujetIspoln { get; set; }
        public Nullable<double> bujetProizv { get; set; }
        public Nullable<double> bujetRezerv { get; set; }
        public Nullable<double> obespechenost { get; set; }
        public Nullable<double> SSfact1 { get; set; }
        public Nullable<bool> bitVipusk { get; set; }
        public Nullable<bool> bitRKD { get; set; }
        public Nullable<System.DateTime> startUSS { get; set; }
        public Nullable<System.DateTime> startEU { get; set; }
        public string OboznacIzdelia { get; set; }
        public string NaimenovanieIzdelia { get; set; }
        public Nullable<bool> mir { get; set; }
        public Nullable<double> procentIzmeneniyStoimDeficita { get; set; }
        public Nullable<System.DateTime> dataRezresheniyaProizvodstva { get; set; }
        public Nullable<bool> PVProvedena { get; set; }
        public Nullable<bool> OVProvedena { get; set; }
        public Nullable<double> SSSDI { get; set; }
        public Nullable<double> SSfact { get; set; }
        public string SposobDostavki { get; set; }
        public string Gruzopoluchatel { get; set; }
        public string PostAdresGruzopoluchatel { get; set; }
        public string INNGruzopoluchatel { get; set; }
        public string OKPOGruzopoluchatelya { get; set; }
        public string KodGruzopoluchatela { get; set; }
        public string StantionGruzopoluchatel { get; set; }
        public string KodStanciiGruzopoluchatelya { get; set; }
        public string OsobieOtmetkiGruzopoluchatelya { get; set; }
        public string DescriptionGruzopoluchatel { get; set; }
        public string naimenovaniePoTU { get; set; }
        public Nullable<System.DateTime> dataDogovora { get; set; }
        public string nomerDogovora { get; set; }
        public Nullable<System.DateTime> dataPrilDogovora { get; set; }
        public string nomerPrilDogovora { get; set; }
        public string naimenovaniePoDogovoru { get; set; }
        public string MTR { get; set; }
        public string oprostiList { get; set; }
        public string obekt { get; set; }
        public string kolvo { get; set; }
        public Nullable<double> weight { get; set; }
        public Nullable<bool> updateData { get; set; }
    }
}
