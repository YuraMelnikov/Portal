using System;

namespace Wiki.Models
{
    public class TableForGraphic
    {
        int orderNum;
        string orderName;
        string orderContract;
        string orderContractArr;
        string orderMTR;
        string orderOL;
        int orderPercentConplited;
        int RKDPercent;
        DateTime RKDStart;
        DateTime RKDFinish;
        int KDPercent;
        DateTime KDStart;
        DateTime KDFinish;
        int step1Percent;
        DateTime step2Start;
        DateTime step2Finish;
        int step21Persent;
        DateTime step21Start;
        DateTime step21Finish;
        int step22Percent;
        DateTime step22Start;
        DateTime step22Finish;
        int step23Percent;
        DateTime step23Start;
        DateTime step23Finish;
        int step24Percent;
        DateTime step24Start;
        DateTime step24Finish;
        int step25Percent;
        DateTime step26Stert;
        DateTime step26Finish;
        int step3Percent;
        DateTime step3Start;
        DateTime step3Finish;
        int step4Percent;
        DateTime step4Start;
        DateTime step4Finish;
        int complitedPercent;
        DateTime complitedDate;
        DateTime shipDate;
        DateTime finishDate;

        public TableForGraphic(int orderNum, string orderName, string orderContract, 
            string orderContractArr, string orderMTR, string orderOL, int orderPercentConplited, 
            int rKDPercent, DateTime rKDStart, DateTime rKDFinish, int kDPercent, DateTime kDStart, DateTime kDFinish, 
            int step1Percent, DateTime step2Start, DateTime step2Finish, int step21Persent, DateTime step21Start, DateTime step21Finish, 
            int step22Percent, DateTime step22Start, DateTime step22Finish, int step23Percent, DateTime step23Start, DateTime step23Finish,
            int step24Percent, DateTime step24Start, DateTime step24Finish, int step25Percent, DateTime step26Stert, DateTime step26Finish, 
            int step3Percent, DateTime step3Start, DateTime step3Finish, int step4Percent, DateTime step4Start, DateTime step4Finish, 
            int complitedPercent, DateTime complitedDate, DateTime shipDate, DateTime finishDate)
        {
            this.orderNum = orderNum;
            this.orderName = orderName;
            this.orderContract = orderContract;
            this.orderContractArr = orderContractArr;
            this.orderMTR = orderMTR;
            this.orderOL = orderOL;
            this.orderPercentConplited = orderPercentConplited;
            RKDPercent = rKDPercent;
            RKDStart = rKDStart;
            RKDFinish = rKDFinish;
            KDPercent = kDPercent;
            KDStart = kDStart;
            KDFinish = kDFinish;
            this.step1Percent = step1Percent;
            this.step2Start = step2Start;
            this.step2Finish = step2Finish;
            this.step21Persent = step21Persent;
            this.step21Start = step21Start;
            this.step21Finish = step21Finish;
            this.step22Percent = step22Percent;
            this.step22Start = step22Start;
            this.step22Finish = step22Finish;
            this.step23Percent = step23Percent;
            this.step23Start = step23Start;
            this.step23Finish = step23Finish;
            this.step24Percent = step24Percent;
            this.step24Start = step24Start;
            this.step24Finish = step24Finish;
            this.step25Percent = step25Percent;
            this.step26Stert = step26Stert;
            this.step26Finish = step26Finish;
            this.step3Percent = step3Percent;
            this.step3Start = step3Start;
            this.step3Finish = step3Finish;
            this.step4Percent = step4Percent;
            this.step4Start = step4Start;
            this.step4Finish = step4Finish;
            this.complitedPercent = complitedPercent;
            this.complitedDate = complitedDate;
            this.shipDate = shipDate;
            this.finishDate = finishDate;
        }
    }
}