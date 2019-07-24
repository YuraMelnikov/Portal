using System;

namespace Wiki.Areas.PZ.Models
{
    public class CorrectPlanZakaz
    {
        PZ_PlanZakaz pZ_PlanZakaz;

        public CorrectPlanZakaz(PZ_PlanZakaz pZ_PlanZakaz)
        {
            this.pZ_PlanZakaz = pZ_PlanZakaz;
            if (pZ_PlanZakaz.MTR == null)
                pZ_PlanZakaz.MTR = "";
            if (pZ_PlanZakaz.OL == null)
                pZ_PlanZakaz.OL = "";
            if (pZ_PlanZakaz.Description == null)
                pZ_PlanZakaz.Description = "";
            if (pZ_PlanZakaz.Gruzopoluchatel == null)
                pZ_PlanZakaz.Gruzopoluchatel = "";
            if (pZ_PlanZakaz.PostAdresGruzopoluchatel == null)
                pZ_PlanZakaz.PostAdresGruzopoluchatel = "";
            if (pZ_PlanZakaz.INNGruzopoluchatel == null)
                pZ_PlanZakaz.INNGruzopoluchatel = "";
            if (pZ_PlanZakaz.OKPOGruzopoluchatelya == null)
                pZ_PlanZakaz.OKPOGruzopoluchatelya = "";
            if (pZ_PlanZakaz.KodGruzopoluchatela == null)
                pZ_PlanZakaz.KodGruzopoluchatela = "";
            if (pZ_PlanZakaz.StantionGruzopoluchatel == null)
                pZ_PlanZakaz.StantionGruzopoluchatel = "";
            if (pZ_PlanZakaz.KodStanciiGruzopoluchatelya == null)
                pZ_PlanZakaz.KodStanciiGruzopoluchatelya = "";
            if (pZ_PlanZakaz.OsobieOtmetkiGruzopoluchatelya == null)
                pZ_PlanZakaz.OsobieOtmetkiGruzopoluchatelya = "";
            if (pZ_PlanZakaz.DescriptionGruzopoluchatel == null)
                pZ_PlanZakaz.DescriptionGruzopoluchatel = "";
            if (pZ_PlanZakaz.Folder == null)
                pZ_PlanZakaz.Folder = "";
            if (pZ_PlanZakaz.Modul == null)
                pZ_PlanZakaz.Modul = "";
            if (pZ_PlanZakaz.timeContract == null)
                pZ_PlanZakaz.timeContract = "";
            if (pZ_PlanZakaz.timeArr == null)
                pZ_PlanZakaz.timeArr = "";
            if (pZ_PlanZakaz.numZakupki == null)
                pZ_PlanZakaz.numZakupki = "";
            if (pZ_PlanZakaz.numLota == null)
                pZ_PlanZakaz.numLota = "";
            if(pZ_PlanZakaz.dataOtgruzkiBP == null)
            {
                DateTime today = DateTime.Now;
                DateTime answer = today.AddDays(90);
                pZ_PlanZakaz.dataOtgruzkiBP = answer;
            }
            if(pZ_PlanZakaz.dataOtgruzkiBP.Year < 2010)
            {
                DateTime today = DateTime.Now;
                DateTime answer = today.AddDays(90);
                pZ_PlanZakaz.dataOtgruzkiBP = answer;
            }
            if (pZ_PlanZakaz.nameTU == null)
                pZ_PlanZakaz.nameTU = "";
            if (pZ_PlanZakaz.TypeShip == 0)
                pZ_PlanZakaz.TypeShip = 1;
            if (pZ_PlanZakaz.nomenklaturNumber == null)
                pZ_PlanZakaz.nomenklaturNumber = "";
            if (pZ_PlanZakaz.id_PZ_OperatorDogovora == 0)
                pZ_PlanZakaz.id_PZ_OperatorDogovora = 1;
            if (pZ_PlanZakaz.id_PZ_FIO == 0)
                pZ_PlanZakaz.id_PZ_FIO = 7;
            if (pZ_PlanZakaz.PowerST == null)
                pZ_PlanZakaz.PowerST = "";
            if (pZ_PlanZakaz.VN_NN == null)
                pZ_PlanZakaz.VN_NN = "";
            if (pZ_PlanZakaz.TypeShip == 0)
                pZ_PlanZakaz.TypeShip = 1;
        }

        public PZ_PlanZakaz PZ_PlanZakaz { get => pZ_PlanZakaz; set => pZ_PlanZakaz = value; }
    }
}