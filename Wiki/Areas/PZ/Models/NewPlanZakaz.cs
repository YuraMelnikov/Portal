using System;
using System.Collections.Generic;
using System.Linq;

namespace Wiki.Areas.PZ.Models
{
    public class NewPlanZakaz
    {
        PortalKATEKEntities db = new PortalKATEKEntities();

        public NewPlanZakaz(PZ_PlanZakaz pZ_PlanZakaz, bool addDebitWork)
        { 
            int numberPZ = db.PZ_PlanZakaz.Where(d => d.PlanZakaz < 9000).Max(d => d.PlanZakaz) + 1;
            PZ_PlanZakaz newPZ_PlanZakaz = new PZ_PlanZakaz() {
                PlanZakaz = numberPZ,
                DateCreate = DateTime.Now,
                MTR = pZ_PlanZakaz.MTR,
                Name = pZ_PlanZakaz.Name,
                OL = pZ_PlanZakaz.OL,
                Zapros = pZ_PlanZakaz.Zapros,
                DateSupply = pZ_PlanZakaz.DateSupply,
                DateShipping = pZ_PlanZakaz.DateShipping,
                Cost = pZ_PlanZakaz.Cost,
                Description = pZ_PlanZakaz.Description,
                ProductType = pZ_PlanZakaz.ProductType,
                Dostavka = pZ_PlanZakaz.Dostavka,
                Manager = pZ_PlanZakaz.Manager,
                Client = pZ_PlanZakaz.Client,
                Gruzopoluchatel = pZ_PlanZakaz.Gruzopoluchatel,
                PostAdresGruzopoluchatel = pZ_PlanZakaz.PostAdresGruzopoluchatel,
                INNGruzopoluchatel = pZ_PlanZakaz.INNGruzopoluchatel,
                OKPOGruzopoluchatelya = pZ_PlanZakaz.OKPOGruzopoluchatelya,
                KodGruzopoluchatela = pZ_PlanZakaz.KodGruzopoluchatela,
                StantionGruzopoluchatel = pZ_PlanZakaz.StantionGruzopoluchatel,
                KodStanciiGruzopoluchatelya = pZ_PlanZakaz.KodStanciiGruzopoluchatelya,
                OsobieOtmetkiGruzopoluchatelya = pZ_PlanZakaz.OsobieOtmetkiGruzopoluchatelya,
                DescriptionGruzopoluchatel = pZ_PlanZakaz.DescriptionGruzopoluchatel,
                Folder = pZ_PlanZakaz.Folder,
                Modul = pZ_PlanZakaz.Modul,
                massa = 0,
                timeContract = pZ_PlanZakaz.timeContract,
                timeContractDate = pZ_PlanZakaz.timeContractDate,
                timeArr = pZ_PlanZakaz.timeArr,
                timeArrDate = pZ_PlanZakaz.timeArrDate,
                id_PZ_FIO = pZ_PlanZakaz.id_PZ_FIO,
                numZakupki = pZ_PlanZakaz.numZakupki,
                numLota = pZ_PlanZakaz.numLota,
                dataOtgruzkiBP = pZ_PlanZakaz.dataOtgruzkiBP,
                ProjectUID = pZ_PlanZakaz.ProjectUID,
                nameTU = pZ_PlanZakaz.nameTU,
                TypeShip = pZ_PlanZakaz.TypeShip,
                criticalDateShip = pZ_PlanZakaz.criticalDateShip,
                nomenklaturNumber = pZ_PlanZakaz.nomenklaturNumber,
                costSMR = pZ_PlanZakaz.costSMR,
                costPNR = pZ_PlanZakaz.costPNR,
                id_PZ_OperatorDogovora = pZ_PlanZakaz.id_PZ_OperatorDogovora,
                PowerST = pZ_PlanZakaz.PowerST,
                VN_NN = pZ_PlanZakaz.VN_NN,
                objectOfExploitation = pZ_PlanZakaz.objectOfExploitation,
                counterText = pZ_PlanZakaz.counterText
            };
            db.PZ_PlanZakaz.Add(newPZ_PlanZakaz);
            db.SaveChanges();
            Reclamation_CloseOrder reclamation_CloseOrder = new Reclamation_CloseOrder
            {
                close = false,
                dateTimeClose = DateTime.Now,
                description = "",
                id_PZ_PlanZakaz = newPZ_PlanZakaz.Id,
                userClose = newPZ_PlanZakaz.Manager
            };
            db.Reclamation_CloseOrder.Add(reclamation_CloseOrder);
            db.SaveChanges();
            PZ_TEO pZ_TEO = new PZ_TEO
            {
                Currency = 1,
                NDS = 0,
                Rate = 0,
                SSM = 0,
                SSR = 0,
                IzdKom = 0,
                IzdPPKredit = 0,
                PI = 0,
                NOP = 0,
                KI_S = 0,
                KI_prochee = 0,
                Id_PlanZakaz = newPZ_PlanZakaz.Id,
                OtpuskChena = 0,
                KursValuti = 0,
                SSRFact = 0,
                percentYear = 0,
                percentPI = 0,
                durationBeforePay = 0,
                SSMToBYN = 0,
                SSMProduct = 0
            };
            db.PZ_TEO.Add(pZ_TEO);
            db.SaveChanges();
            PZ_Setup pZ_Setup = new PZ_Setup
            {
                KolVoDneyNaPrijemku = 0,
                PunktDogovoraOSrokahPriemki = "",
                RassmotrenieRKD = 0,
                SrokZamechanieRKD = 0,
                TimeNaRKD = 0,
                UslovieOplatyInt = 0,
                UslovieOplatyText = "",
                id_PZ_PlanZakaz = newPZ_PlanZakaz.Id
            };
            db.PZ_Setup.Add(pZ_Setup);
            db.SaveChanges();
            Debit_Platform debit_Platform = new Debit_Platform
            {
                id_PlanZakaz = newPZ_PlanZakaz.Id,
                countPlatform = 0,
                gabar = "",
                massa = 0,
                numPlatform = "",
                numPlomb = ""
            };
            db.Debit_Platform.Add(debit_Platform);
            db.SaveChanges();
            PlanVerificationItems planVerificationItems = new PlanVerificationItems
            {
                id_PZ_PlanZakaz = newPZ_PlanZakaz.Id,
                @fixed = false,
                appDescription = "",
                factDescription = "",
                fixetFirstDate = false,
                planDescription = "",
                verificationDateInPrj = newPZ_PlanZakaz.dataOtgruzkiBP
            };
            db.PlanVerificationItems.Add(planVerificationItems);
            db.SaveChanges();
            if (addDebitWork == true)
            {
                List<TaskForPZ> dateTaskWork = db.TaskForPZ.Where(w => w.step == 1).Where(z => z.id_TypeTaskForPZ == 1).ToList();
                foreach (var data in dateTaskWork)
                {
                    Debit_WorkBit newDebit_WorkBit = new Debit_WorkBit();
                    newDebit_WorkBit.dateCreate = DateTime.Now;
                    newDebit_WorkBit.close = false;
                    newDebit_WorkBit.id_PlanZakaz = newPZ_PlanZakaz.Id;
                    newDebit_WorkBit.id_TaskForPZ = (int)data.id;
                    newDebit_WorkBit.datePlanFirst = DateTime.Now.AddDays((double)data.time);
                    newDebit_WorkBit.datePlan = DateTime.Now.AddDays((double)data.time);
                    if (newDebit_WorkBit.id_TaskForPZ == 1)
                        newDebit_WorkBit.dateClose = DateTime.Now;
                    db.Debit_WorkBit.Add(new Debit_WorkBit()
                    {
                        close = false,
                        dateCreate = DateTime.Now,
                        datePlan = newDebit_WorkBit.datePlan,
                        datePlanFirst = newDebit_WorkBit.datePlanFirst,
                        id_PlanZakaz = newDebit_WorkBit.id_PlanZakaz,
                        id_TaskForPZ = newDebit_WorkBit.id_TaskForPZ,
                        dateClose = newDebit_WorkBit.dateClose
                    });
                    db.SaveChanges();
                }
            }
        }
    }
}