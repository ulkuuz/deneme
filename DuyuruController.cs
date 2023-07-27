using ADF.Contracts.DataContracts;
using JGNK.Asayis.Servis.Contracts.DataContracts.SilahIslemleri.CompositeEntities;
using JGNK.Asayis.Servis.Contracts.DataContracts.SilahIslemleri.DTOs;
using JGNK.Asayis.Servis.Contracts.ServiceContracts.SilahIslemleri;
using System.Web.Mvc;
using ADF.MVC.Enums;
using SilahIslemleriYonetici.Base;
using System;

namespace SilahIslemleriYonetici.Controllers
{
    public class DuyuruController : SilahIslemleriYoneticiBase
    {
        [HttpPost]
        public JsonResult DuyuruGetir(int id)
        {
            var duyuru = ProxyHelper.ExecuteCall<ILoginService, ServiceResult<DuyuruModelDto>>(s => s.GetirDuruyu(id)).Result;
            return Json(duyuru, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Duyurular()
        {
            var dyrList = ProxyHelper.ExecuteCall<ILoginService, ServiceResult<DuyuruModelComposite>>(s => s.ListeleDuyurular(2, true)).Result;
         
            ShowToastrMessage("Lütfen duyuruları dikkatlice okuyunuz.", MessageType.Info);
            return View(dyrList);
        }
        [Authorize(Roles = "1")]
        public ActionResult DuyuruEkle()
        {
            var model = ProxyHelper.ExecuteCall<ILoginService, ServiceResult<DuyuruModelComposite>>(s => s.ListeleDuyurular(3)).Result;
            model.Duyuru.YayinTarihi = DateTime.Today;
            model.Duyuru.YayindanKalkmaTarihi = DateTime.Today.AddDays(5);
            return View(model);
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult DuyuruEkle(DuyuruModelComposite duyuruModel)
        {
            var res = ProxyHelper.ExecuteCall<ILoginService, ServiceResult>(s => s.DuyuruEkle(duyuruModel.Duyuru));
            ShowToastrMessage(res.Message, MessageType.Info);
            return RedirectToAction("DuyuruEkle");
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        public JsonResult DuyuruSilme(int id)
        {
            ProxyHelper.ExecuteCall<ILoginService, ServiceResult>(s => s.SilDuyuru(id));
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}