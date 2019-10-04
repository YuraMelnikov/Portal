using NLog;
using System;
using System.IO;
using System.Web;
using Wiki.Models;
using System.Data.Entity;

namespace Wiki.Areas.CMO.Models
{
    public class SandwichPanelCRUD
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private HttpPostedFileBase[] fileUploadArray;
        SandwichPanel sandwichPanel;

        public void CreateOrder(int[] id_PlanZakaz, string login, HttpPostedFileBase[] fileUploadArray)
        {

            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                this.fileUploadArray = fileUploadArray;
                GetBasicFuild(login);
                sandwichPanel.folder = "";
                db.SandwichPanel.Add(sandwichPanel);
                db.SaveChanges();
                sandwichPanel.folder = CreateFolderAndFileForPreOrder(sandwichPanel.id);
                sandwichPanel.onApprove = false;
                sandwichPanel.onCustomer = true;
                sandwichPanel.datetimeUploadNewVersion = DateTime.Now;
                db.Entry(sandwichPanel).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var dataPZ in id_PlanZakaz)
                {
                    SandwichPanel_PZ sandwichPanel_PZ = new SandwichPanel_PZ();
                    sandwichPanel_PZ.id_PZ_PlanZakaz = dataPZ;
                    sandwichPanel_PZ.id_SandwichPanel = sandwichPanel.id;
                    db.SandwichPanel_PZ.Add(sandwichPanel_PZ);
                    db.SaveChanges();
                }
                try
                {
                    new EmailSandwichPanel(sandwichPanel, login, 1);
                    logger.Debug("SandwichPanelCRUD / EmailSandwichPanel: " + sandwichPanel.id);
                }
                catch (Exception ex)
                {
                    logger.Error("SandwichPanelCRUD / EmailSandwichPanel: " + sandwichPanel.id + " | " + ex);
                }
            }
        }

        bool GetBasicFuild(string login)
        {
            sandwichPanel = new SandwichPanel();
            sandwichPanel.datetimeCreate = DateTime.Now;
            sandwichPanel.id_AspNetUsersCreate = new Login().LoginEmailToId(login);
            sandwichPanel.numberOrder = "";
            sandwichPanel.onApprove = true;
            sandwichPanel.onCorrection = false;
            sandwichPanel.onCustomer = false;
            sandwichPanel.onGetDateComplited = false;
            sandwichPanel.onComplited = false;
            sandwichPanel.remove = false;
            return true;
        }

        private string CreateFolderAndFileForPreOrder(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string directory = "\\\\192.168.1.30\\m$\\_ЗАКАЗЫ\\SP\\" + id.ToString() + "\\";
                Directory.CreateDirectory(directory);
                SaveFileToServer(directory);
                sandwichPanel.folder = directory;
                db.Entry(sandwichPanel).State = EntityState.Modified;
                db.SaveChanges();
                return directory;
            }
        }

        private void SaveFileToServer(string folderAdress)
        {
            for (int i = 0; i < fileUploadArray.Length; i++)
            {
                string fileReplace = Path.GetFileName(fileUploadArray[i].FileName);
                fileReplace = ToSafeFileName(fileReplace);
                var fileName = string.Format("{0}\\{1}", folderAdress, fileReplace);
                fileUploadArray[i].SaveAs(fileName);
            }
        }

        private string ToSafeFileName(string s)
        {
            return s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }
    }
}