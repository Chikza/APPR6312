using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Windows;
using Part_2.Models;
using Microsoft.Ajax.Utilities;
using System.Runtime.Remoting.Contexts;
using System.Reflection;
using System.Web.Helpers;

namespace Part_2.Controllers
{
    public class HomeController : Controller
    {
        DjPromoWebsiteEntities entities = new DjPromoWebsiteEntities();
        string conn = "Server=tcp:st10126721.database.windows.net,1433;Initial Catalog=djPromoWebsite;Persist Security Info=False;User ID=admin1;Password= Sphawizzy06;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";


        public ActionResult Index()
        {
            var available = entities.MonetaryDonations.Select(m => m.Amount).Sum() - entities.AllocateMoneys.Select(a => a.ALloocationAmount).Sum() - entities.PurchaseGoods.Select(p => p.AmountOfPurchase).Sum();
            ViewBag.Available = available;
            var availableGoods = entities.GoodDonations.Select(m => m.NumItems).Sum() - entities.AllocateGoods.Select(s => s.NumOfItems).Sum() + entities.PurchaseGoods.Select(a => a.NumItems).Sum();

            ViewBag.goodAvailable = availableGoods;
            return View();
        }

        public ActionResult AllocationIndex()
        {
            var available = entities.MonetaryDonations.Select(m => m.Amount).Sum() - entities.AllocateMoneys.Select(a => a.ALloocationAmount).Sum() - entities.PurchaseGoods.Select(p => p.AmountOfPurchase).Sum();
            ViewBag.Available = available;
            var availableGoods = entities.GoodDonations.Select(m => m.NumItems).Sum() - entities.AllocateGoods.Select(s => s.NumOfItems).Sum()+ entities.PurchaseGoods.Select(a => a.NumItems).Sum();
            if(availableGoods == null)
            {
                ViewBag.goodAvailable = "No Goods Available";
            }
            {
                ViewBag.goodAvailable = availableGoods;
            }
            return View();
        }
        public ActionResult MonetaryList()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("Select * from MonetaryDonations", con);
                ad.Fill(dt);
            }
            return View(dt);
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(UserTable user)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = "INSERT INTO UserTable(Email, Full_name, Password)" +
                        "VALUES (@Email, @Full_name, @Password)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Full_name", user.Full_name);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Saved Successfully!!!!!!");
                    }
                    con.Close();
                    return RedirectToAction("LogIn", "Home");
                }

            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("LogIn", "Home");
        }
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(UserTable user)
        {
             
            SqlConnection bv = new SqlConnection("Data Source=st10126721.database.windows.net;Initial Catalog=djPromoWebsite;User ID=admin1;Password=Sphawizzy06;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM UserTable WHERE Email= '" + user.Email + "' AND Password ='" + user.Password + "'", bv);
            DataTable tb = new DataTable();
            adapter.Fill(tb);
            if (tb.Rows.Count > 0)
            {
                Session["Email"] = user.Email.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                MessageBox.Show("Invalid username or password");
            }
            return View();
        }
        public ActionResult MonetaryDonations()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MonetaryDonations(MonetaryDonation monetary, UserTable user)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = "INSERT INTO MonetaryDonations(Emails, DonatorName, Date, Amount)" +
                        "VALUES (@Emails, @DonatorName, @Date, @Amount)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Emails", Session["Email"]);
                        cmd.Parameters.AddWithValue("@DonatorName", monetary.DonatorName);
                        cmd.Parameters.AddWithValue("@Date", monetary.Date);
                        cmd.Parameters.AddWithValue("@Amount", monetary.Amount);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Saved Successfully!!!!!!");
                    }
                    con.Close();
                    return RedirectToAction("MonetaryList", "Home");
                }

            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }

        }
        public ActionResult ListDisaster()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("Select * from Disaster", con);
                ad.Fill(dt);
            }

            return View(dt);
        }
        public ActionResult AddDisaster()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddDisaster(Disaster disaster)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = "INSERT INTO Disaster(Start_date, End_date, Location, Description, Aid_types, Email)" +
                        "VALUES (@Start_date, @End_date, @Location, @Description, @Aid_types, @Email)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Email", Session["Email"]);
                        cmd.Parameters.AddWithValue("@Start_date", disaster.Start_date);
                        cmd.Parameters.AddWithValue("@End_date", disaster.End_date);
                        cmd.Parameters.AddWithValue("@Location", disaster.Location);
                        cmd.Parameters.AddWithValue("@Description", disaster.Description);
                        cmd.Parameters.AddWithValue("@Aid_types", disaster.Aid_types);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Saved Successfully!!!!!!");
                    }
                    con.Close();
                    return RedirectToAction("ListDisaster", "Home");
                }

            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }
        }
        public ActionResult GoodList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("Select * from GoodDonations", con);
                ad.Fill(dt);
            }

            return View(dt);
        }
        [HttpGet]
        public ActionResult GoodDonations()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GoodDonations(GoodDonation good)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = "INSERT INTO GoodDonations(Email, DonarName, Date, NumItems, Category, Description)" +
                        "VALUES (@Email, @DonarName, @Date, @NumItems, @Category, @Description)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Email", Session["Email"]);
                        cmd.Parameters.AddWithValue("@DonarName", good.DonarName);
                        cmd.Parameters.AddWithValue("@Date", good.Date);
                        cmd.Parameters.AddWithValue("@NumItems", good.NumItems);
                        cmd.Parameters.AddWithValue("@Category", good.Category);
                        cmd.Parameters.AddWithValue("@Description", good.Description);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Saved Successfully!!!!!!");
                    }
                    con.Close();
                    return RedirectToAction("GoodList", "Home");
                }

            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult EditMonetary(int id, MonetaryDonation monetary)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    string query = "SELECT * FROM MonetaryDonations WHERE MonetaryId = @MonetaryId";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    adapter.SelectCommand.Parameters.AddWithValue("@MonetaryId", id);
                    adapter.Fill(dt);
                }
                if (dt.Rows.Count == 1)
                {
                    monetary.MonetaryId = Convert.ToInt32(dt.Rows[0][0].ToString());
                    monetary.Emails = dt.Rows[0][1].ToString();
                    monetary.DonatorName = dt.Rows[0][2].ToString();
                    monetary.Date = Convert.ToDateTime(dt.Rows[0][3].ToString());
                    monetary.Amount = Convert.ToInt32(dt.Rows[0][4].ToString());
                    return View(monetary);
                }
                else
                {
                    return RedirectToAction("MonetaryList");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult EditMonetary(MonetaryDonation monetary, UserTable user)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = "UPDATE MonetaryDonations SET Emails = @Emails, DonatorName = @DonatorName, Date = @Date, Amount = @Amount WHERE MonetaryId = @MonetaryId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@MonetaryId", monetary.MonetaryId);
                        cmd.Parameters.AddWithValue("@Emails", Session["Email"]);
                        cmd.Parameters.AddWithValue("@DonatorName", monetary.DonatorName);
                        cmd.Parameters.AddWithValue("@Date", monetary.Date);
                        cmd.Parameters.AddWithValue("@Amount", monetary.Amount);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Saved Successfully!!!!!!");
                    }
                    con.Close();
                    return RedirectToAction("MonetaryList", "Home");
                }

            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }

        }
        public ActionResult DeletMonetary(int id)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = "DELETE MonetaryDonations WHERE MonetaryId = @MonetaryId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@MonetaryId", id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Deleted Successfully!!!!!!");
                    }
                    con.Close();
                    return RedirectToAction("MonetaryList", "Home");
                }

            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }
        }
        public ActionResult MonentaryDeatils(int id, MonetaryDonation monetary)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    string query = "SELECT * FROM MonetaryDonations WHERE MonetaryId = @MonetaryId";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    adapter.SelectCommand.Parameters.AddWithValue("@MonetaryId", id);
                    adapter.Fill(dt);
                }
                if (dt.Rows.Count == 1)
                {
                    monetary.MonetaryId = Convert.ToInt32(dt.Rows[0][0].ToString());
                    monetary.Emails = dt.Rows[0][1].ToString();
                    monetary.DonatorName = dt.Rows[0][2].ToString();
                    monetary.Date = Convert.ToDateTime(dt.Rows[0][3].ToString());
                    monetary.Amount = Convert.ToInt32(dt.Rows[0][4].ToString());
                    return View(monetary);
                }
                else
                {
                    return RedirectToAction("MonetaryList");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult EditGoods(int id, GoodDonation good)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    string query = "SELECT * FROM MonetaryDonations WHERE MonetaryId = @MonetaryId";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    adapter.SelectCommand.Parameters.AddWithValue("@MonetaryId", id);
                    adapter.Fill(dt);
                }
                if (dt.Rows.Count == 1)
                {
                    good.GoodId = Convert.ToInt32(dt.Rows[0][0].ToString());
                    good.Email = dt.Rows[0][1].ToString();
                    good.DonarName = dt.Rows[0][2].ToString();
                    good.Date = Convert.ToDateTime(dt.Rows[0][3].ToString());
                    good.NumItems = Convert.ToInt32(dt.Rows[0][4].ToString());
                    good.Category = dt.Rows[0][5].ToString();
                    good.Description = dt.Rows[0][6].ToString();
                    return View(good);
                }
                else
                {
                    return RedirectToAction("MonetaryList");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult EditGoods(GoodDonation good)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = "UPDATE GoodDonations SET Email = @Email, DonarName = @DonarName, Date = @Date, NumItems = @NumItems, Category = @Category, Description = @Description  WHERE GoodId = @GoodId";

                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@GoodId", good.GoodId);
                        cmd.Parameters.AddWithValue("@Email", Session["Email"]);
                        cmd.Parameters.AddWithValue("@DonarName", good.DonarName);
                        cmd.Parameters.AddWithValue("@Date", good.Date);
                        cmd.Parameters.AddWithValue("@NumItems", good.NumItems);
                        cmd.Parameters.AddWithValue("@Category", good.Category);
                        cmd.Parameters.AddWithValue("@Description", good.Description);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Saved Successfully!!!!!!");
                    }
                    con.Close();
                    return RedirectToAction("GoodList", "Home");
                }

            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }

        }
        public ActionResult DeletGoods(int id)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    string query = "DELETE GoodDonations WHERE GoodId = @GoodId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@GoodId", id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Deleted Successfully!!!!!!");
                    }
                    con.Close();
                    return RedirectToAction("MonetaryList", "Home");
                }

            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }
        }
        public ActionResult GoodDeatils(int id, GoodDonation good)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    string query = "SELECT * FROM MonetaryDonations WHERE MonetaryId = @MonetaryId";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    adapter.SelectCommand.Parameters.AddWithValue("@MonetaryId", id);
                    adapter.Fill(dt);
                }
                if (dt.Rows.Count == 1)
                {
                    good.GoodId = Convert.ToInt32(dt.Rows[0][0].ToString());
                    good.Email = dt.Rows[0][1].ToString();
                    good.DonarName = dt.Rows[0][2].ToString();
                    good.Date = Convert.ToDateTime(dt.Rows[0][3].ToString());
                    good.NumItems = Convert.ToInt32(dt.Rows[0][4].ToString());
                    good.Category = dt.Rows[0][5].ToString();
                    good.Description = dt.Rows[0][6].ToString();
                    return View(good);
                }
                else
                {
                    return RedirectToAction("MonetaryList");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //Part 2
        public ActionResult AllocateMoney()
        {
            var available = entities.MonetaryDonations.Select(m => m.Amount).Sum() - entities.AllocateMoneys.Select(a => a.ALloocationAmount).Sum() - entities.PurchaseGoods.Select(p => p.AmountOfPurchase).Sum();
            ViewBag.Available = available;
            var disasters = entities.Disasters.ToList();
            //ViewBag.DisasterList = new SelectList(disasters, "DisasterId", "Description");
            ViewBag.DisasterList = new SelectList(disasters, "DisasterId", "Description");

            return View();
        }
        [HttpPost]
        public ActionResult AllocateMoney(AllocateMoney money)
        {
            var maxEndDate = entities.Disasters.Max(d => d.End_date);
            if (money.AllocationDate < maxEndDate)
            {
                entities.AllocateMoneys.Add(money);
                entities.SaveChanges();
                TempData["Message"] = "Allocation successful"; // Provide feedback
                return View(); // Redirect to a different view
            }
            else
            {
                ViewBag.error = "Disaster is not active";
                return View();
            }
        }
        public ActionResult AllocateGoods()
        {
            var availableGoods = entities.GoodDonations.Select(m => m.NumItems).Sum() - entities.AllocateGoods.Select(s => s.NumOfItems).Sum() + entities.PurchaseGoods.Select(a => a.NumItems).Sum();

            ViewBag.goodAvailable = availableGoods;

            var disasters = entities.Disasters.ToList();
            var goods = entities.GoodDonations.ToList();
            ViewBag.DisasterList = new SelectList(disasters, "DisasterId", "Description");
            ViewBag.GoodsList = new SelectList(goods, "GoodId", "Category");
            return View();
        }
        [HttpPost]
        public ActionResult AllocateGoods(AllocateGood goods)
        {
            var maxDate = entities.Disasters.Max(d => d.End_date);
            if (goods.DateOfAllocatioN < maxDate)
            {
                entities.AllocateGoods.Add(goods);
                entities.SaveChanges();
                return View();
            }
            else
            {
                ViewBag.error = "Disaster is not active";
                return View();
                // Handle the case where AllocationDate is not less than maxEndDate
            }

        }
        public ActionResult PurchaseGoods()
        {
            var available = entities.MonetaryDonations.Select(m => m.Amount).Sum() - entities.AllocateMoneys.Select(a => a.ALloocationAmount).Sum() - entities.PurchaseGoods.Select(p => p.AmountOfPurchase).Sum();
            ViewBag.Available = available;
            var disasters = entities.Disasters.ToList();
            var goods = entities.GoodDonations.ToList();
            ViewBag.DisasterList = new SelectList(disasters, "DisasterId", "Description");
            ViewBag.GoodsList = new SelectList(goods, "GoodId", "Category");
            return View();
        }
        [HttpPost]
        public ActionResult PurchaseGoods(PurchaseGood purchase)
        {

            entities.PurchaseGoods.Add(purchase);
            entities.SaveChanges();
            return View();

        }
        public ActionResult ListAllocated()
        {
            var available = entities.MonetaryDonations.Select(m => m.Amount).Sum() - entities.AllocateMoneys.Select(a => a.ALloocationAmount).Sum() - entities.PurchaseGoods.Select(p => p.AmountOfPurchase).Sum();
            ViewBag.Available = available;
            List<AllocateMoney> allocateMoneyList = new List<AllocateMoney>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("SELECT a.AllocationDate, a.ALloocationAmount, di.Description\r\nFROM  AllocateMoney a, Disaster di\r\nWHERE \ta.DisasterId = di.DisasterId", con);

                DataTable dt = new DataTable();
                ad.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    AllocateMoney allocateMoney = new AllocateMoney
                    {
                        AllocationDate = (DateTime)row["AllocationDate"],
                        ALloocationAmount = (decimal)row["ALloocationAmount"],
                        Disaster = new Disaster
                        {
                            Description = row["Description"].ToString()
                        }
                    };

                    allocateMoneyList.Add(allocateMoney);
                }
            }

            return View(allocateMoneyList);
        }
        
        public ActionResult ListAllocatedGoods()
        {
            var availableGoods = entities.GoodDonations.Select(m => m.NumItems).Sum() - entities.AllocateGoods.Select(s => s.NumOfItems).Sum() + entities.PurchaseGoods.Select(a => a.NumItems).Sum();

            ViewBag.goodAvailable = availableGoods;

            List<AllocateGood> allocateGoodsList = new List<AllocateGood>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("SELECT g.DateOfAllocation, g.NumOfItems, d.Description, o.Category\r\nFROM AllocateGoods g, Disaster d, GoodDonations o\r\nWHERE g.DisasterId = d.DisasterId\r\nAND g.GoodId = o.GoodId;", con);

                DataTable dt = new DataTable();
                ad.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    AllocateGood allocateMoney = new AllocateGood
                    {
                        DateOfAllocatioN = (DateTime)row["DateOfAllocation"],
                        NumOfItems = (int)row["NumOfItems"],
                        GetDisaster = new Disaster
                        {
                            Description = row["Description"].ToString()
                        },
                        GoodDonation = new GoodDonation
                        {
                            Category = row["Category"].ToString(),
                        }
                    };

                    allocateGoodsList.Add(allocateMoney);
                }
            }

            return View(allocateGoodsList);
        }
        public ActionResult ListPurchasedGoods() 
        {
            var available = entities.MonetaryDonations.Select(m => m.Amount).Sum() - entities.AllocateMoneys.Select(a => a.ALloocationAmount).Sum() - entities.PurchaseGoods.Select(p => p.AmountOfPurchase).Sum();
            ViewBag.Available = available;
            List<PurchaseGood> purchaseGood = new List<PurchaseGood>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter("SELECT g.DateOfPurchase, g.AmountOfPurchase, g.NumItems, d.Description, o.Category\r\nFROM PurchaseGoods g, Disaster d, GoodDonations o\r\nWHERE g.DisasterId = d.DisasterId\r\nAND g.GoodId = o.GoodId;", con);

                DataTable dt = new DataTable();
                ad.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    PurchaseGood purchased = new PurchaseGood
                    {
                        DateOfPurchase = (DateTime)row["DateOfPurchase"],
                        AmountOfPurchase = (decimal)row["AmountOfPurchase"],
                        NumItems = (int)row["NumItems"],
                        GetDisaster = new Disaster
                        {
                            Description = row["Description"].ToString()
                        },
                        GoodDonation = new GoodDonation
                        {
                            Category = row["Category"].ToString(),
                        }
                    };

                    purchaseGood.Add(purchased);
                }
            }

            return View(purchaseGood);
        }
        
    }
}