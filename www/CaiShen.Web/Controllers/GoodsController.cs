using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Models;
using Pannet.DAL;
using Pannet.Utility;
using PagedList;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using Pannet.Web.Attribute;
using Pannet.DAL.Repository;
using System.Text;

//using System.ComponentModel.DataAnnotations;

namespace Pannet.Web.Controllers
{
    public class GoodsController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };
        #region 商品列表

        [CheckPermission]
        public ActionResult Index(string field = "name", string keyword = "", int GoodsCategoryID = 0, int GoodsTypeID = 0, int page = 1, string action = "", int isexist = -1, int istuijian = -1, int ismobile = -1, int ismobile_tj = -1, int status = -1)
        {
            ViewBag.Field = field;
            ViewBag.Keyword = keyword;
            ViewBag.GoodsTypes = work.GoodsTypeRepository.Get();
            ViewBag.GoodsCategoryID = GoodsCategoryID;
            ViewBag.GoodsTypeID = GoodsTypeID;
            ViewBag.isexist = isexist;
            ViewBag.istuijian = istuijian;
            ViewBag.ismobile = ismobile;
            ViewBag.ismobile_tj = ismobile_tj;
            ViewBag.status = status;

            #region 批量操作

            if (!string.IsNullOrEmpty(action))
            {
                string ids = Request.Form["ids"];
                if (!string.IsNullOrEmpty(ids))
                {
                    string[] arrIds = ids.Trim(',').Split(',');
                    if (action == "delete")//批量删除
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));
                                if (model != null)
                                {
                                    work.GoodsRepository.Delete(model);
                                    work.Save();
                                }
                            }
                        }
                    }
                    else if (action == "update")//批量更新
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_Sort = Convert.ToInt32(Request.Form["sort_" + a_id]);
                                model.G_Name = Request.Form["name_" + a_id];
                                model.G_MarketPrice = Convert.ToDecimal(Request.Form["marketprice_" + a_id]);
                                model.G_Price = Convert.ToDecimal(Request.Form["price_" + a_id]);
                                //model.G_Unit = Request.Form["unit_" + a_id];
                                //model.G_UnitInfo = Request.Form["unitinfo_" + a_id];
                                //model.G_SortMobile = Convert.ToInt32(Request.Form["mobilesort_" + a_id]);

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "tj")//批量推荐
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_IsRecommend = 1;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "tj_cancel")//批量取消推荐
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_IsRecommend = 0;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "new")//批量新品
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_IsNew = 1;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "new_cancel")//批量取消新品
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_IsNew = 0;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "hot")//批量热卖
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_IsHot = 1;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "hot_cancel")//批量取消热卖
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_IsHot = 0;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "onsale")//上架
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_Status = 1;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "offsale")//下架
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_Status = 0;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "weight")//重货
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_IsWeight = 1;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "weight_not")//泡货
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_IsWeight = 0;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "m_tj")//手机推荐
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_IsRecommendMobile = 1;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "m_tj_cancel")//取消手机推荐
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_IsRecommendMobile = 0;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "m_show")//手机显示
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_IsMobile = 1;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "m_show_cancel")//取消手机显示
                    {
                        Goods model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                                model.G_IsMobile = 0;

                                work.GoodsRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                }

            }


            #endregion

            var rst = work.Context.Goods.Join(work.Context.GoodsCategorys, g => g.GoodsCategoryID, gc => gc.ID, (g, gc) => new { g, gc });
            if (GoodsCategoryID != 0)
            {
                rst = rst.Where(m => m.gc.GC_ParentID == GoodsCategoryID || m.gc.ID == GoodsCategoryID);
            }
            if (GoodsTypeID != 0)
            {
                rst = rst.Where(m => m.g.GoodsTypeID == GoodsTypeID);
                ViewBag.GoodsCategorys = work.GoodsCategoryRepository.Get(m => m.GC_Type == GoodsTypeID);
            }
            else
            {
                ViewBag.GoodsCategorys = work.GoodsCategoryRepository.Get();
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                switch (field)
                {
                    case "name": rst = rst.Where(m => m.g.G_Name.Contains(keyword)); break;
                    case "number": rst = rst.Where(m => m.g.G_Number.Contains(keyword)); break;
                    default: break;
                };
            }
            //现货
            if (isexist != -1)
            {
                rst = rst.Where(m => m.g.G_IsExist == isexist);
            }
            //推荐
            if (istuijian != -1)
            {
                rst = rst.Where(m => m.g.G_IsRecommend == istuijian);
            }
            //手机
            if (ismobile != -1)
            {
                rst = rst.Where(m => m.g.G_IsMobile == ismobile);
            }
            //手机推荐
            if (ismobile_tj != -1)
            {
                rst = rst.Where(m => m.g.G_IsRecommendMobile == ismobile_tj);
            }
            //上下架状态
            if (status != -1)
            {
                rst = rst.Where(m => m.g.G_Status == status);
            }
            List<Goods> listGoods = rst.Select(m => m.g).ToList().OrderByDescending(m => m.ID).ToList();

            int pageSize = 20;
            int pageNumber = page;
            return View(listGoods.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region 添加商品

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        [ValidateInput(false)]
        public ActionResult Add()
        {
            //ViewBag.GoodsCategorysParent = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == 0);
            //ViewBag.GoodsCategorysSub = work.GoodsCategoryRepository.Get();
            ViewBag.GoodsTypes = work.GoodsTypeRepository.Get().OrderBy(m => m.GT_Name).ToList();
            ViewBag.Suppliers = work.SupplierRepository.Get();
            ViewBag.Warehouses = work.WarehouseRepository.Get();
            ViewBag.Brands = work.BrandRepository.Get();
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

            return View(new Goods());
        }

        /// <summary>
        /// 添加商品-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        [ValidateInput(false)]
        public ActionResult Add(Goods newModel, int GoodsCategoryIDSub = 0)
        {
            ViewBag.GoodsCategorysParent = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == 0 && m.GC_Type == newModel.GoodsTypeID);
            ViewBag.GoodsTypes = work.GoodsTypeRepository.Get().OrderBy(m => m.GT_Name).ToList();
            ViewBag.Suppliers = work.SupplierRepository.Get();
            ViewBag.Warehouses = work.WarehouseRepository.Get();
            ViewBag.Brands = work.BrandRepository.Get();
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

            JsonResult json = new JsonResult
            {
                Data = new
                {
                    status = "0",
                    msg = "添加成功！"
                }
            };


            if (ModelState.IsValid)
            {

                var role = work.GoodsRepository.Get(m => m.G_Number == newModel.G_Number);
                if (role.Count() > 0)
                {
                    //ModelState.AddModelError("G_Number", "商品编号已存在");
                    json.Data = new { status = "-1", msg = "商品编号已存在！" };
                    return Json(json);
                }
                else
                {
                    //选择二级分类时，保存二级分类
                    if (GoodsCategoryIDSub != 0)
                    {
                        newModel.GoodsCategoryID = GoodsCategoryIDSub;
                    }
                    else if (newModel.GoodsCategoryID == 0)
                    {
                        json.Data = new { status = "-1", msg = "请选择商品分类！" };
                        return Json(json);
                    }

                    if (newModel.G_Tags != null)
                    {
                        newModel.G_Tags = newModel.G_Tags.Replace("，", ",");
                    }

                    work.GoodsRepository.Insert(newModel);
                    work.Save();

                    //图片不为空，插入至goodsPhoto表，并默认为主图
                    if (!string.IsNullOrEmpty(newModel.G_Image) && newModel.ID != 0)
                    {
                        GoodsPhoto modelPhoto = new GoodsPhoto();
                        modelPhoto.GP_Image = newModel.G_Image;
                        modelPhoto.GoodsID = newModel.ID;
                        modelPhoto.GP_IsFirst = 1;
                        modelPhoto.GP_Sort = 0;

                        work.GoodsPhotoRepository.Insert(modelPhoto);
                        work.Save();
                    }

                    work.Dispose();

                    LogService.Add(ManagerService.GetLoginModel(), "新增商品信息:" + newModel.G_Name, newModel.ID.ToString());

                    //ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    json.Data = new { status = "0", msg = "保存成功！", ID = newModel.ID };
                    return Json(json);
                }
            }
            else
            {
                //return View(newModel);
                StringBuilder errinfo = new StringBuilder();
                foreach (var s in ModelState.Values)
                {
                    foreach (var p in s.Errors)
                    {
                        errinfo.AppendFormat("{0}", p.ErrorMessage);
                    }
                }
                json.Data = new { status = "-1", msg = errinfo.ToString() };

                return Json(json);
            }
        }

        #endregion

        #region 编辑商品

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        [ValidateInput(false)]
        public ActionResult Edit(int ID = 0, string info = "")
        {
            ViewBag.ID = ID;

            if (info == "success")
            {
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }

            if (ID != 0)
            {
                ViewBag.GoodsTypes = work.GoodsTypeRepository.Get();
                ViewBag.Suppliers = work.SupplierRepository.Get();
                ViewBag.Warehouses = work.WarehouseRepository.Get();
                ViewBag.Brands = work.BrandRepository.Get();
                ViewBag.CategoryParentID = 0;
                ViewBag.CategorySubID = 0;
                ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

                var model = work.GoodsRepository.Get(m => m.ID == ID).FirstOrDefault<Goods>();

                if (model != null)
                {
                    //当前商品分类
                    ViewBag.GoodsCategorysParent = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == 0 && m.GC_Type == model.GoodsTypeID);
                    GoodsCategory categoryModel = work.GoodsCategoryRepository.Get(m => m.ID == model.GoodsCategoryID).FirstOrDefault<GoodsCategory>();
                    if (categoryModel != null && categoryModel.GC_ParentID == 0)//所属分类为一级
                    {
                        ViewBag.GoodsCategorysSub = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == model.GoodsCategoryID);
                        ViewBag.CategoryParentID = model.GoodsCategoryID;
                    }
                    else if (categoryModel != null)
                    {
                        ViewBag.GoodsCategorysSub = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == categoryModel.GC_ParentID);
                        ViewBag.CategoryParentID = categoryModel.GC_ParentID;
                        ViewBag.CategorySubID = model.GoodsCategoryID;
                    }
                    else
                    {
                        ViewBag.CategoryParentID = 0;
                        ViewBag.CategorySubID = 0;
                    }
                }

                return View(model);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 编辑商品-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        [ValidateInput(false)]
        public ActionResult Edit(Goods newModel, int ID = 0, int GoodsCategoryIDSub = 0)
        {
            ViewBag.GoodsCategorysParent = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == 0 && m.GC_Type == newModel.GoodsTypeID);
            ViewBag.GoodsTypes = work.GoodsTypeRepository.Get();
            ViewBag.Suppliers = work.SupplierRepository.Get();
            ViewBag.Warehouses = work.WarehouseRepository.Get();
            ViewBag.Brands = work.BrandRepository.Get();
            ViewBag.CategoryParentID = 0;
            ViewBag.CategorySubID = 0;
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

            if (ModelState.IsValid)
            {
                if (ID != 0)
                {
                    //选择二级分类时，保存二级分类
                    if (GoodsCategoryIDSub != 0)
                    {
                        newModel.GoodsCategoryID = GoodsCategoryIDSub;
                    }
                    else if (newModel.GoodsCategoryID == 0)
                    {
                        ModelState.AddModelError("GoodsCategoryID", "请选择商品分类");
                        return View(newModel);
                    }

                    //保存后， 继续显示已选项目
                    //var oldModel = work.GoodsRepository.Get(m => m.ID == ID).FirstOrDefault<Goods>();

                    //if (oldModel != null)
                    //{
                    //当前商品分类
                    GoodsCategory categoryModel = work.GoodsCategoryRepository.Get(m => m.ID == newModel.GoodsCategoryID).FirstOrDefault<GoodsCategory>();
                    if (categoryModel.GC_ParentID == 0)//所属分类为一级
                    {
                        ViewBag.GoodsCategorysSub = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == newModel.GoodsCategoryID);
                        ViewBag.CategoryParentID = newModel.GoodsCategoryID;
                    }
                    else
                    {
                        ViewBag.GoodsCategorysSub = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == categoryModel.GC_ParentID);
                        ViewBag.CategoryParentID = categoryModel.GC_ParentID;
                        ViewBag.CategorySubID = newModel.GoodsCategoryID;
                    }
                    //}


                    var existModel = work.GoodsRepository.Get(m => m.G_Number == newModel.G_Number & m.ID != ID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("G_Number", "商品编号已存在");
                    }
                    else
                    {

                        //oldModel.BrandID = newModel.BrandID;
                        //oldModel.G_Count = newModel.G_Count;
                        //oldModel.G_Desc = newModel.G_Desc;
                        //oldModel.G_Image = newModel.G_Image;
                        //oldModel.G_IsExist = newModel.G_IsExist;
                        //oldModel.G_IsFixedPrice = newModel.G_IsFixedPrice;
                        //oldModel.G_IsHot = newModel.G_IsHot;
                        //oldModel.G_IsNew = newModel.G_IsNew;
                        //oldModel.G_IsRecommend = newModel.G_IsRecommend;
                        //oldModel.G_IsZiti = newModel.G_IsZiti;
                        //oldModel.G_MarketPrice = newModel.G_MarketPrice;
                        //oldModel.G_Name = newModel.G_Name;
                        //oldModel.G_Number = newModel.G_Number;
                        //oldModel.G_Price = newModel.G_Price;
                        //oldModel.G_Sort = newModel.G_Sort;
                        //oldModel.G_Status = newModel.G_Status;
                        //oldModel.G_Volume = newModel.G_Volume;
                        //oldModel.G_Weight = newModel.G_Weight;
                        //oldModel.GoodsCategoryID = newModel.GoodsCategoryID;
                        //oldModel.GoodsTypeID = newModel.GoodsTypeID;
                        //oldModel.SupplierID = newModel.SupplierID;

                        if (newModel.G_Tags != null)
                        {
                            newModel.G_Tags = newModel.G_Tags.Replace("，", ",");
                        }

                        work.GoodsRepository.Update(newModel);
                        work.Save();

                        //ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                        LogService.Add(LoginedAdminModel, "编辑商品信息:" + newModel.G_Name, newModel.ID.ToString());

                        work.Dispose();
                        return RedirectToAction("Edit", new { ID = ID, info = "success" });
                    }
                }
            }

            return View(newModel);
        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1)
                //{
                //    return RedirectToAction("Index");
                //}
                var role = work.GoodsRepository.Get(m => m.ID == ID).FirstOrDefault<Goods>();
                work.GoodsRepository.Delete(role);
                work.Save();
                work.Dispose();
                LogService.Add(ManagerService.GetLoginModel(), "删除商品信息", ID.ToString());

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 编辑商品相册

        /// <summary>
        /// 编辑商品相册
        /// </summary>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult EditPhotos(int GoodsID = 0, int ID = 0, int page = 1)
        {
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.GoodsPhotoRepository.Get(m => m.GoodsID == GoodsID);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.GoodsID = GoodsID;
            ViewBag.GoodsPhoto = new GoodsPhoto();

            if (ID != 0)
            {
                ViewBag.GoodsPhoto = work.GoodsPhotoRepository.Get(m => m.ID == ID).FirstOrDefault();
            }

            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 上传新图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult EditPhotos(GoodsPhoto newModel, int ID = 0, int page = 1)
        {
            //展示调整记录
            int pageSize = 12;
            int pageNumber = page;


            ViewBag.GoodsID = newModel.GoodsID;
            ViewBag.GoodsPhoto = new GoodsPhoto();

            if (ModelState.IsValid)
            {
                if (ID == 0)
                {
                    if (newModel.GP_IsFirst == 1)//主图,取消其他主图
                    {
                        GoodsPhotoService.SetFirst(newModel.GoodsID, 0);
                        //GoodsService.UpdateImage(newModel.GoodsID, newModel.GP_Image);
                    }
                    //保存
                    work.GoodsPhotoRepository.Insert(newModel);

                    work.Save();
                }
                else
                {
                    ViewBag.GoodsPhoto = newModel;// work.GoodsPhotoRepository.Get(m => m.ID == ID).FirstOrDefault();

                    if (newModel.GP_IsFirst == 1)//主图,取消其他主图
                    {
                        GoodsPhotoService.SetFirst(newModel.GoodsID, ID);
                        //GoodsService.UpdateImage(newModel.GoodsID, newModel.GP_Image);
                    }

                    //编辑
                    //GoodsPhoto model = new GoodsPhoto();
                    //model.GoodsID = newModel.GoodsID;
                    //model.GP_Image = newModel.GP_Image;
                    //model.GP_IsFirst = newModel.GP_IsFirst;
                    //model.GP_Sort = newModel.GP_Sort;
                    work.GoodsPhotoRepository.Update(newModel);
                    work.Save();
                }

            }
            //var rst = work.GoodsPhotoRepository.Get(m => m.GoodsID == newModel.GoodsID);
            //rst = rst.OrderByDescending(m => m.ID);
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return RedirectToAction("EditPhotos", new { GoodsID = newModel.GoodsID });
        }

        //删除图片
        [CheckPermission]
        public ActionResult DeletePhoto(int ID = 0, int GoodsID = 0)
        {
            if (ID != 0)
            {
                var m = work.GoodsPhotoRepository.Get(g => g.ID == ID).FirstOrDefault<GoodsPhoto>();
                work.GoodsPhotoRepository.Delete(m);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("EditPhotos", new { GoodsID = GoodsID });
        }


        #endregion

        #region 商品定价区域

        /// <summary>
        /// 定价区域 - 列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult GoodsPriceArea(int page = 1)
        {
            var rst = work.Context.GoodsPriceAreas.OrderByDescending(m => m.ID);

            int pageSize = 12;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 设置定价区域
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult SetArea(int ID = 0)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);

            if (ID == 0)
            {
                return View(new GoodsPriceArea());
            }
            else
            {
                GoodsPriceArea model = work.GoodsPriceAreaRepository.GetByID(ID);
                if (model != null)
                {
                    List<string> areaidsArr = model.AreaIds.Split(',').AsQueryable().Where(m => m != "").ToList();
                    ViewBag.Areas = work.Context.Areas.Where(m => areaidsArr.Contains(m.ID.ToString())).ToList();
                }
                return View(model);
            }
        }

        /// <summary>
        /// 设置定价区域-POST
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult SetArea(GoodsPriceArea newModel, int ID = 0)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(newModel.Title))
                {
                    ModelState.AddModelError("Title", string.Format("标题不能为空！"));
                    return View(newModel);
                }

                if (ID == 0)
                {
                    work.GoodsPriceAreaRepository.Insert(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

                }
                else
                {
                    work.GoodsPriceAreaRepository.Update(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                }
            }

            return RedirectToAction("GoodsPriceArea");
            //return View(newModel);
        }

        #endregion

        #region 删除定价区域
        /// <summary>
        /// 删除定价区域
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult DeleteGoodsPriceArea(int SupplierID = 0, int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("GoodsPriceArea");
                }
                var model = work.GoodsPriceAreaRepository.Get(m => m.ID == ID).FirstOrDefault<GoodsPriceArea>();
                work.GoodsPriceAreaRepository.Delete(model);
                work.Save();
                work.Dispose();
            }
            return RedirectToAction("GoodsPriceArea");
        }

        #endregion

        #region 课程目录

        //默认课程目录列表
        //[CheckPermission]
        public ActionResult GoodsArticle(int goodsid, int page = 1)
        {
            ViewBag.GoodsID = goodsid;
            var rst = work.Context.GoodsArticles.Where(m => m.GoodsID == goodsid);
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View(work.GoodsArticleRepository.Get());
        }

        /// <summary>
        /// 添加/编辑课程目录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        [ValidateInput(false)]
        public ActionResult GoodsArticleAdd(int goodsid, int ID = 0)
        {
            ViewBag.GoodsID = goodsid;

            if (ID != 0)
            {
                var model = work.GoodsArticleRepository.Get(m => m.ID == ID).FirstOrDefault<GoodsArticle>();
                return View(model);
            }
            return View(new GoodsArticle());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult GoodsArticleAdd(GoodsArticle newModel, int goodsid, int ID = 0)
        {
            ViewBag.GoodsID = goodsid;

            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    var role = work.GoodsArticleRepository.Get(m => m.GoodsID == goodsid && m.GA_Title == newModel.GA_Title);
                    if (role.Count() > 0)
                    {
                        ModelState.AddModelError("GA_Title", "课程目录已存在");
                    }
                    else
                    {
                        work.GoodsArticleRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    var existModel = work.Context.GoodsArticles.AsNoTracking().Where(m => m.GA_Title == newModel.GA_Title & m.ID != ID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("GA_Title", "课程目录已存在");
                    }
                    else
                    {

                        work.GoodsArticleRepository.Update(newModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                        return RedirectToAction("GoodsArticle", new { goodsid });
                    }
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除课程目录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult GoodsArticleDelete(int goodsid, int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.GoodsArticleRepository.Get(m => m.ID == ID).FirstOrDefault<GoodsArticle>();
                work.GoodsArticleRepository.Delete(model);
                work.Save();
                work.Dispose();
            }

            return RedirectToAction("GoodsArticle", new { goodsid });
        }

        #endregion

        #region 课程学习记录

        //课程学习记录
        [CheckPermission]
        public ActionResult GoodsArticleRecord(string keyword = "", int ArticleTypeID = 0, int ArticleTypeIDSub = 0, int page = 1)
        {
            ViewBag.keyword = keyword;
            var rst = work.Context.GoodsArticleRecords
                .Join(work.Context.GoodsArticles, gar => gar.GoodsArticleID, ga => ga.ID, (gar, ga) => new { gar, ga })
                .Join(work.Context.Goods, m => m.gar.GoodsID, g => g.ID, (m, g) => new { m.gar, m.ga, g })
                .Join(work.Context.Users, m => m.gar.UserID, u => u.ID, (m, u) => new { m.ga, m.gar, u, m.g });

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.ga.GA_Title.Contains(keyword) || m.g.G_Name.Contains(keyword));
            }
            var list = rst.Select(m => new GoodsArticleRecordVModel
            {
                GoodsArticleRecord = m.gar,
                GoodsArticleTitle = m.ga.GA_Title,
                GoodsName = m.g.G_Name,
                UserName = m.u.U_UserName
            }).OrderByDescending(m => m.GoodsArticleRecord.ID);

            int pageSize = 20;
            int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 删除课程学习记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult GoodsArticleRecordDelete(int ID = 0, int ArticleTypeID = 0, int ArticleTypeIDSub = 0, string Keyword = "")
        {

            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Index", new { ArticleTypeID, Keyword, ArticleTypeIDSub });
                }
                var role = work.ArticleRepository.Get(m => m.ID == ID).FirstOrDefault<Article>();
                work.ArticleRepository.Delete(role);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Index", new { ArticleTypeID, Keyword, ArticleTypeIDSub });
        }

        #endregion

        #region 课程试题

        //默认课程试题列表
        //[CheckPermission]
        public ActionResult Question(int goodsid, int goodsArticleId = 0, string keyword = "", int page = 1)
        {
            ViewBag.GoodsID = goodsid;
            ViewBag.ArticleID = goodsArticleId;
            ViewBag.keyword = keyword;
            ViewBag.GoodsArticles = work.Context.GoodsArticles.Where(m => m.GoodsID == goodsid).ToList();

            int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.课程测试);
            var rst = work.Context.Questions.Where(m => m.Q_GroupItemID == goodsid && m.Q_Group == q_group)
                .GroupJoin(work.Context.Answer, q => q.ID, a => a.QuestionID, (q, a) => new { q, a })
                .Select(m => new QuestionAnswerVModel
                {
                    Question = m.q,
                    AnswerList = m.a
                });
            if (goodsArticleId != 0)
            {
                rst = rst.Where(m => m.Question.Q_GroupItemSubID == goodsArticleId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.Question.Q_Title.Contains(keyword));
            }
            //.SelectMany(m=>m.q,  (q,a)=>new{ q,a});
            rst = rst.OrderByDescending(m => m.Question.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View(work.GoodsArticleRepository.Get());
        }

        /// <summary>
        /// 添加试题
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult QuestionAdd(int goodsid)
        {
            ViewBag.GoodsID = goodsid;
            ViewBag.GoodsArticles = work.Context.GoodsArticles.Where(m => m.GoodsID == goodsid).ToList();
            return View(new Question());
        }

        /// <summary>
        /// 添加处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        [HttpPost]
        public ActionResult QuestionAdd(Question newModel, int goodsid, int ID = 0, List<AnswerVModel> answer = null)
        {
            ViewBag.GoodsID = goodsid;
            ViewBag.GoodsArticles = work.Context.GoodsArticles.Where(m => m.GoodsID == goodsid).ToList();
            int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.课程测试);

            //if (ModelState.IsValid)
            //{
            newModel.Q_Group = q_group;
            newModel.Q_GroupItemID = goodsid;
            work.QuestionRepository.Insert(newModel);
            work.Save();

            if (answer != null)
            {
                foreach (var item in answer)
                {
                    if (!string.IsNullOrEmpty(item.answer))
                    {
                        Answer newAnswer = new Answer();
                        newAnswer.A_Answer = item.answer;
                        newAnswer.A_IsTrue = item.istrue;
                        newAnswer.A_Sort = item.sort;
                        newAnswer.QuestionID = newModel.ID;

                        work.AnswerRepository.Insert(newAnswer);
                        work.Save();
                    }
                }
            }
            work.Dispose();

            ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            //}
            return View(newModel);
        }
        /// <summary>
        /// 编辑试题
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult QuestionEdit(int goodsid, int ID = 0)
        {
            ViewBag.GoodsID = goodsid;
            ViewBag.GoodsArticles = work.Context.GoodsArticles.Where(m => m.GoodsID == goodsid).ToList();

            if (ID != 0)
            {
                var model = work.QuestionRepository.Get(m => m.ID == ID).FirstOrDefault<Question>();
                if (model != null)
                {
                    ViewBag.AnswerList = work.AnswerRepository.Get(m => m.QuestionID == model.ID);
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("QuestionAdd", goodsid);
            }
            return View(new Question());
        }

        /// <summary>
        /// 编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        [HttpPost]
        public ActionResult QuestionEdit(Question newModel, int goodsid, int ID = 0, List<AnswerVModel> answer = null)
        {
            ViewBag.GoodsID = goodsid;
            ViewBag.GoodsArticles = work.Context.GoodsArticles.Where(m => m.GoodsID == goodsid).ToList();
            int q_group = Convert.ToInt16(DataConfig.QuestionGroupEnum.课程测试);

            ViewBag.AnswerList = work.AnswerRepository.Get(m => m.QuestionID == ID);

            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                }
                else
                {
                    newModel.Q_Group = q_group;
                    newModel.Q_GroupItemID = goodsid;
                    work.QuestionRepository.Update(newModel);

                    if (answer != null)
                    {
                        foreach (var item in answer)
                        {
                            if (!string.IsNullOrEmpty(item.answer))
                            {
                                Answer newAnswer = new Answer();
                                newAnswer.A_Answer = item.answer;
                                newAnswer.A_IsTrue = item.istrue;
                                newAnswer.A_Sort = item.sort;
                                newAnswer.QuestionID = newModel.ID;
                                if (item.id == 0)
                                {
                                    work.AnswerRepository.Insert(newAnswer);
                                    work.Save();
                                }
                                else
                                {
                                    newAnswer.ID = item.id;
                                    work.AnswerRepository.Update(newAnswer);
                                    work.Save();
                                }
                            }
                        }
                    }
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    return RedirectToAction("Question", new { goodsid });
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除试题
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult QuestionDelete(int goodsid, int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.QuestionRepository.Get(m => m.ID == ID).FirstOrDefault<Question>();
                work.QuestionRepository.Delete(model);
                work.Save();
                work.Dispose();
            }

            return RedirectToAction("Question", new { goodsid });
        }
        /// <summary>
        /// 删除试题选项
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult AnswerDelete(int goodsid, int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.AnswerRepository.Get(m => m.ID == ID).FirstOrDefault<Answer>();
                work.AnswerRepository.Delete(model);
                work.Save();
                work.Dispose();
            }

            json.Data = new { status = "success", msg = "" };
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 课程测试记录

        //课程测试记录
        [CheckPermission]
        public ActionResult Test(string keyword = "", int page = 1, string action = "", string time_start = "", string time_end = "", int province = 0, int city = 0, int region = 0)
        {
            ViewBag.keyword = keyword;
            ViewBag.action = action;
            ViewBag.province = province;
            ViewBag.city = city;
            ViewBag.region = region;
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);

            var rst = work.Context.Tests
                .Join(work.Context.GoodsArticles, t => t.T_GoodsArticleID, ga => ga.ID, (t, ga) => new { t, ga })
                .Join(work.Context.Goods, t => t.t.GoodsID, g => g.ID, (t, g) => new { t, g })
                .Join(work.Context.Users, m => m.t.t.UserID, u => u.ID, (m, u) => new TestGoodsVModel
                {
                    Test = m.t.t,
                    GoodsArticleTitle = m.t.ga.GA_Title,
                    GoodsName = m.g.G_Name,
                    UserName = u.U_UserName,
                    RealName = u.U_RealName,
                    Department = u.U_DepartmentID,
                    Province = u.U_Province,
                    City = u.U_City,
                    ShopName = u.U_ShopName
                });

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.GoodsName.Contains(keyword) || m.GoodsArticleTitle.Contains(keyword) || m.UserName.Contains(keyword) || m.RealName.Contains(keyword) || m.ShopName.Contains(keyword));
            }
            if (province != 0)
            {
                rst = rst.Where(m => m.Province == province);
            }
            if (city != 0)
            {
                rst = rst.Where(m => m.City == city);
            }
            //if (region != 0)
            //{
            //    rst = rst.Where(m => m.reg == region);
            //}

            rst = rst.OrderByDescending(m => m.Test.ID);

            if (action == "export")//导出
            {
                string fileName = "课程测试记录" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                ExportTest(rst.ToList(), fileName);
                //try
                //{
                //}
                //catch (Exception ex)
                //{
                //    Response.End();
                //}
            }

            int pageSize = 20;
            int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 删除课程测试记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult TestDelete(int ID = 0)
        {

            if (ID != 0)
            {
                var role = work.TestRepository.Get(m => m.ID == ID).FirstOrDefault<Test>();
                work.TestRepository.Delete(role);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Test");
        }

        #endregion

        #region 导出测试记录

        public void ExportTest(List<TestGoodsVModel> list, string fileName)
        {
            HttpResponseBase resp;
            resp = HttpContext.Response;
            resp.Charset = "utf-8";
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GBK");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            resp.ContentType = "application/ms-excel";
            string colHeaders = "", ls_item = "";

            colHeaders += "课程目录名称" + "\t";
            colHeaders += "用户" + "\t";
            colHeaders += "姓名" + "\t";
            colHeaders += "身份" + "\t";
            colHeaders += "省份" + "\t";
            colHeaders += "城市" + "\t";
            colHeaders += "门店" + "\t";
            colHeaders += "测试时间" + "\t";
            colHeaders += "正确" + "\t";
            colHeaders += "错误" + "\t";
            colHeaders += "完成状态" + "\n";


            resp.Write(colHeaders);
            //向HTTP输出流中写入取得的数据信息 

            //逐行处理数据   
            foreach (var itemv in list)
            {
                Test item = itemv.Test;
                ls_item += itemv.GoodsArticleTitle + "\t";
                ls_item += itemv.UserName + "\t";
                ls_item += itemv.RealName + "\t";

                Department department = work.Context.Departments.Where(m => m.ID == itemv.Department).FirstOrDefault();
                if (department != null)
                {
                    ls_item += department.Dep_Name + "\t";
                }
                else
                {
                    ls_item += " " + "\t";
                }
                Area province = work.Context.Areas.Where(m => m.ID == itemv.Province).FirstOrDefault();
                if (province != null)
                {
                    ls_item += province.Area_Name + "\t";

                }
                else
                {
                    ls_item += " " + "\t";
                }
                Area city = work.Context.Areas.Where(m => m.ID == itemv.City).FirstOrDefault();
                if (city != null)
                {
                    ls_item += city.Area_Name + "\t";

                }
                else
                {
                    ls_item += " " + "\t";
                }
                ls_item += itemv.ShopName + "\t";
                ls_item += item.T_CreateTime + "\t";
                ls_item += item.T_RightCount + "\t";
                ls_item += item.T_WrongCount + "\t";
                ls_item += (item.T_State == 1 ? "完成" : "未完成") + "\n";

                resp.Write(ls_item);
                ls_item = "";

            }
            resp.End();
        }

        #endregion

        #region 课程统计

        //课程统计记录
        [CheckPermission]
        public ActionResult TongJi(string keyword = "", int page = 1, string action = "", string time_start = "", string time_end = "", int province = 0, int city = 0, int region = 0, int DepartmentID = 0)
        {
            ViewBag.keyword = keyword;
            ViewBag.action = action;
            ViewBag.province = province;
            ViewBag.city = city;
            ViewBag.region = region;
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.DepartmentID = DepartmentID;
            ViewBag.Departments = work.DepartmentRepository.Get();

            var usertest = work.Context.Tests
               .Join(work.Context.Users, t => t.UserID, u => u.ID, (t, u) => new { t, u })
               .Join(work.Context.Departments, ut => ut.u.U_DepartmentID, d => d.ID, (ut, d) => new { ut.u, ut.t, d });
            if (province != 0)
            {
                usertest = usertest.Where(m => m.u.U_Province == province);
            }
            if (city != 0)
            {
                usertest = usertest.Where(m => m.u.U_City == city);
            }
            if (DepartmentID != 0)
            {
                usertest = usertest.Where(m => m.d.ID == DepartmentID || m.d.Dep_FollowID == DepartmentID);
            }

            var rst = work.Context.GoodsArticles
                .Join(work.Context.Goods, ga => ga.GoodsID, g => g.ID, (ga, g) => new { ga, g })
                .GroupJoin(usertest, ga => ga.ga.ID, t => t.t.T_GoodsArticleID, (ga, t) => new GoodsTongJiVModel
                {
                    Goods = ga.g,
                    GoodsArticle = ga.ga,
                    TestCount = t.Count(),
                    TestPersonCount = t.Select(m => m.u).Distinct().Count(),
                    TestHegeCount = t.Where(m => m.t.T_WrongCount == 0).Count(),
                    ViewCount = 0,
                    ViewPersonCount = work.Context.BrowseRecords.Where(br => br.BR_ItemType == 6 && br.BR_ItemID == ga.ga.ID).Select(m => m.UserID).Distinct().Count()
                });
            //.GroupJoin(work.Context.Tests, ga => ga.ga.ID, t => t.T_GoodsArticleID, (ga, t) => new GoodsTongJiVModel
            // {
            //     Goods = ga.g,
            //     GoodsArticle = ga.ga,
            //     Tests  =t,
            //     TestCount = t.Count(),
            //     TestPersonCount = t.Select(m => m.UserID).Distinct().Count(),
            //     TestHegeCount = t.Where(m => m.T_WrongCount == 0).Count(),
            //     ViewCount = 0,
            //     ViewPersonCount = 0
            // });
            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.Goods.G_Name.Contains(keyword) || m.GoodsArticle.GA_Title.Contains(keyword));
            }

            //if (region != 0)
            //{
            //    rst = rst.Where(m => m.reg == region);
            //}

            rst = rst.OrderByDescending(m => m.GoodsArticle.GoodsID).ThenBy(m => m.GoodsArticle.ID);

            if (action == "export")//导出
            {
                string fileName = "课程统计记录" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                ExportTongJi(rst.ToList(), fileName);
                try
                {
                }
                catch (Exception ex)
                {
                    Response.End();
                }
            }

            int pageSize = 20;
            int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region 导出课程统计记录

        public void ExportTongJi(List<GoodsTongJiVModel> list, string fileName)
        {
            HttpResponseBase resp;
            resp = HttpContext.Response;
            resp.Charset = "utf-8";
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GBK");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            resp.ContentType = "application/ms-excel";
            string colHeaders = "", ls_item = "";

            colHeaders += "视频名称" + "\t";
            colHeaders += "课程名称" + "\t";
            colHeaders += "浏览次数" + "\t";
            colHeaders += "浏览人数" + "\t";
            colHeaders += "测试次数(满分)" + "\t";
            colHeaders += "合格率" + "\t";
            colHeaders += "测试人数" + "\n";

            resp.Write(colHeaders);
            //向HTTP输出流中写入取得的数据信息 

            //逐行处理数据   
            foreach (var itemv in list)
            {
                Goods goods = itemv.Goods;
                GoodsArticle goodsArticle = itemv.GoodsArticle;
                ls_item += goodsArticle.GA_Title + "\t";
                ls_item += goods.G_Name + "\t";
                ls_item += goodsArticle.GA_ShowTimes + "\t";
                ls_item += itemv.ViewPersonCount + "\t";
                ls_item += itemv.TestCount + "(" + itemv.TestHegeCount + ")\t";
                ls_item += (itemv.TestCount != 0 ? Math.Round(itemv.TestHegeCount * 100.0 / itemv.TestCount, 2) : 0) + "% \t";
                ls_item += itemv.TestPersonCount + "\n";

                resp.Write(ls_item);
                ls_item = "";

            }
            resp.End();
        }

        #endregion
    }
}