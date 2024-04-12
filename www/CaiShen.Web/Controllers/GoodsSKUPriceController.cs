using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;
using Pannet.Utility;
using PagedList;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using EntityFramework.Extensions;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace Pannet.Web.Controllers
{
    public class GoodsSKUPriceController : Controller
    {
        private UnitOfWork work = new UnitOfWork();

        #region 产品SKU列表

        /// <summary>
        /// 默认列表
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Index(int SKU_ID = 0, string action = "", string GoodsPriceAreaID = "")
        {
            ViewBag.SKU_ID = SKU_ID;
            ViewBag.action = action;
            ViewBag.GoodsPriceAreaID = GoodsPriceAreaID;
            ViewBag.GoodsPriceAreas = work.Context.GoodsPriceAreas.OrderByDescending(m => m.Sort).ThenByDescending(m => m.ID).ToList();

            #region 批量操作

            if (!string.IsNullOrEmpty(action))
            {
                string ids = Request.Form["ids"];
                if (!string.IsNullOrEmpty(ids))
                {
                    string[] arrIds = ids.Trim(',').Split(',');
                    if (action == "delete")//批量删除
                    {
                        work.Context.GoodsSKUPrices.Where(m => arrIds.Contains(m.ID.ToString())).Delete();
                        work.Save();
                    }
                    else if (action == "update")//批量更新
                    {
                        GoodsSKUPrice model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.GoodsSKUPriceRepository.GetByID(Convert.ToInt32(a_id));

                                decimal shopPrice = Convert.ToDecimal(Request.Form["shopprice_" + a_id]);
                                decimal clientPrice = Convert.ToDecimal(Request.Form["clientprice_" + a_id]);
                                decimal clientTotalPrice = Convert.ToDecimal(Request.Form["costtotalprice_" + a_id]);

                                model.Count = Convert.ToInt32(Request.Form["count_" + a_id]);
                                model.CostPrice = Convert.ToDecimal(Request.Form["costprice_" + a_id]);
                                model.Freight = Convert.ToDecimal(Request.Form["freight_" + a_id]);
                                if (clientTotalPrice == 0)
                                {
                                    model.CostTotalPrice = model.CostPrice + model.Freight;
                                }
                                else
                                {
                                    model.CostTotalPrice = clientTotalPrice;
                                }
                                model.ShopPriceRate = Convert.ToDouble(Request.Form["shoppricerate_" + a_id]);
                                if (shopPrice == 0)
                                {
                                    model.ShopPrice = model.CostTotalPrice * Convert.ToDecimal(model.ShopPriceRate);
                                }
                                else
                                {
                                    model.ShopPrice = shopPrice;
                                }
                                model.ClientPriceRate = Convert.ToDouble(Request.Form["clientpricerate_" + a_id]);
                                if (clientPrice == 0)
                                {
                                    model.ClientPrice = model.ShopPrice * Convert.ToDecimal(model.ClientPriceRate);
                                }
                                else
                                {
                                    model.ClientPrice = clientPrice;
                                }
                                model.TaoPrice = 0;


                                work.GoodsSKUPriceRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                }

            }

            #endregion

            var rst = work.Context.GoodsSKUPrices
                .GroupJoin(work.Context.GoodsPriceAreas, p => p.GoodsPriceAreaID, pa => pa.ID, (p, pa) => new { p, pa })
                .Where(m => m.p.SKUID == SKU_ID)
                .SelectMany(
                    xy => xy.pa.DefaultIfEmpty(),
                    (xy, y) => new { p = xy.p, pa = y })
                .Select(m => new GoodsSKUPriceVModel
            {
                GoodsPriceAreaTitle = string.IsNullOrEmpty(m.pa.Title) ? "通用配置" : m.pa.Title,
                GoodsSKUPrice = m.p
            });
            if (GoodsPriceAreaID != "")
            {
                int areaId = Convert.ToInt16(GoodsPriceAreaID);
                rst = rst.Where(m => m.GoodsSKUPrice.GoodsPriceAreaID == areaId);
            }
            rst = rst.OrderByDescending(m => m.GoodsSKUPrice.GoodsPriceAreaID).ThenByDescending(m => m.GoodsSKUPrice.Count).ThenByDescending(m => m.GoodsSKUPrice.ID);
            return View(rst);
        }

        #endregion

        #region 添加一条数据

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="SKU_ID"></param>
        /// <param name="GoodsPriceAreaID">定价区域</param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Add(int SKU_ID = 0, int GoodsPriceAreaID = 0)
        {
            ViewBag.GoodsPriceAreaID = GoodsPriceAreaID;

            if (SKU_ID != 0)
            {
                GoodsSKUPrice newModel = new GoodsSKUPrice();
                newModel.SKUID = SKU_ID;
                newModel.GoodsPriceAreaID = GoodsPriceAreaID;
                work.GoodsSKUPriceRepository.Insert(newModel);
                work.Save();
                work.Dispose();
            }

            return RedirectToAction("Index", new { SKU_ID });
        }


        #endregion

        #region 导入数据

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="SKU_ID"></param>
        /// <param name="GoodsPriceAreaID"></param>
        /// <returns></returns>
        public ActionResult Import(int SKU_ID = 0, int GoodsPriceAreaID = 0, string action = "")
        {
            var files = Request.Files;
            ViewBag.GoodsPriceAreaID = GoodsPriceAreaID;
            if (!string.IsNullOrEmpty(action) && action == "add")//添加一条
            {
                if (SKU_ID != 0)
                {
                    GoodsSKUPrice newModel = new GoodsSKUPrice();
                    newModel.SKUID = SKU_ID;
                    newModel.GoodsPriceAreaID = GoodsPriceAreaID;
                    work.GoodsSKUPriceRepository.Insert(newModel);
                    work.Save();
                    work.Dispose();
                }
                //return RedirectToAction("Add", new { SKU_ID, GoodsPriceAreaID });
            }
            else
            {
                if (files.Count > 0)
                {
                    var file = files[0];
                    string filePath = UpLoadXls(file);
                    DataSet ds = GetDataSetFromXls(filePath);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            //数量列不为空，且是数字
                            if (!string.IsNullOrEmpty(dr[3].ToString()) && PageValidate.IsNumber(dr[3].ToString()))
                            {
                                GoodsSKUPrice model = new GoodsSKUPrice();
                                Log.WriteLog("单挑数据" + string.Format("{0},{1},{2},{3},{4},{5}", dr[2], dr[3], dr[4], dr[8], dr[12], dr[14]), "skuPrice", DateTime.Now.ToString("yyyyMMdd"));

                                model.Count = Convert.ToInt32(dr[3]);
                                model.CostPrice = Convert.ToDecimal(dr[2]);
                                model.Freight = Convert.ToDecimal(dr[4]);
                                model.CostTotalPrice = Convert.ToDecimal(dr[8]);
                                model.ShopPrice = Math.Round(Convert.ToDecimal(dr[12]));
                                model.ClientPrice = Math.Round(Convert.ToDecimal(dr[14]));
                                model.ShopPriceRate = Convert.ToDouble(Math.Round(model.ShopPrice / model.CostTotalPrice, 2, MidpointRounding.AwayFromZero));
                                model.ClientPriceRate = Convert.ToDouble(Math.Round(model.ClientPrice / model.ShopPrice, 2, MidpointRounding.AwayFromZero));
                                model.TaoPrice = Math.Round(Convert.ToDecimal(dr[15]));
                                model.SKUID = SKU_ID;
                                model.GoodsPriceAreaID = GoodsPriceAreaID;

                                work.GoodsSKUPriceRepository.Insert(model);
                            }
                        }
                        work.Save();
                        work.Dispose();
                    }
                }
            }
            return RedirectToAction("Index", new { SKU_ID });
        }

        /// <summary>
        /// 上传Excel文件
        /// </summary>
        /// <param name="postFile">上传的文件</param>
        /// <returns></returns>
        private string UpLoadXls(HttpPostedFileBase postFile)
        {
            string orifilename = string.Empty;
            string uploadfilepath = string.Empty;
            string modifyfilename = string.Empty;
            string fileExtend = "";//文件扩展名
            int fileSize = 0;//文件大小
            try
            {
                if (postFile.ContentLength > 0)
                {
                    //得到文件的大小
                    fileSize = postFile.ContentLength;
                    if (fileSize == 0)
                    {
                        throw new Exception("导入的Excel文件大小为0，请检查是否正确！");
                    }
                    //得到扩展名
                    fileExtend = Path.GetExtension(postFile.FileName).ToLower();
                    if (!fileExtend.Contains("xls"))
                    {
                        throw new Exception("你选择的文件格式不正确，只能导入EXCEL文件！");
                    }
                    //路径
                    uploadfilepath = Server.MapPath("~/Upload/skuprice");
                    //新文件名
                    modifyfilename = System.Guid.NewGuid().ToString();
                    modifyfilename += "." + fileExtend;
                    //判断是否有该目录
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(uploadfilepath);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    orifilename = uploadfilepath + "\\" + modifyfilename;
                    //如果存在,删除文件
                    if (System.IO.File.Exists(orifilename))
                    {
                        System.IO.File.Delete(orifilename);
                    }
                    // 上传文件
                    postFile.SaveAs(orifilename);
                }
                else
                {
                    throw new Exception("请选择要导入的Excel文件!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orifilename;
        }


        /// <summary>
        /// 从Excel提取数据 => DataSet => 数据库
        /// </summary>
        /// <param name="filename">Excel文件路径名，完整物理地址</param>
        private DataSet GetDataSetFromXls(string fileName)
        {
            try
            {
                if (fileName == string.Empty)
                {
                    throw new ArgumentNullException("Excel文件上传失败！");
                }

                string oleDBConnString = String.Empty;
                oleDBConnString = "Provider=Microsoft.Jet.OLEDB.4.0;";
                oleDBConnString += "Data Source=";
                oleDBConnString += fileName;
                oleDBConnString += ";Extended Properties=Excel 8.0;";
                OleDbConnection oleDBConn = null;
                OleDbDataAdapter oleAdMaster = null;
                DataTable m_tableName = new DataTable();
                DataSet ds = new DataSet();

                oleDBConn = new OleDbConnection(oleDBConnString);
                oleDBConn.Open();
                m_tableName = oleDBConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (m_tableName != null && m_tableName.Rows.Count > 0)
                {

                    m_tableName.TableName = m_tableName.Rows[0]["TABLE_NAME"].ToString();

                }
                string sqlMaster;
                sqlMaster = " SELECT *  FROM [" + m_tableName.TableName + "A:CV]";
                oleAdMaster = new OleDbDataAdapter(sqlMaster, oleDBConn);
                oleAdMaster.Fill(ds, "m_tableName");
                oleAdMaster.Dispose();
                oleDBConn.Close();
                oleDBConn.Dispose();

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}