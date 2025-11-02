using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using TabletOrder.Entity;
using System.Xml;

namespace TabletOrder.Data
{
    public class Parameter
    {
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
    }
    public class DataRepository
    {
        private string connectionString;
        private LoginedData _loginedData;
        public DataRepository(LoginedData LoginData)
        {
            connectionString = getConnectionString();
            _loginedData = LoginData;
        }
        private string getConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnectionString"]
                .ConnectionString.ToString();
        }
        public UserLoginDto Login(List<Parameter> paramS)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(@"SELECT UserId,UserName FROM [TblUsers] Where  
                                        UserName = @UserName And PassWord = @PassWord", connection);
                SetParameter(command, paramS);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    UserLoginDto userlogin = new UserLoginDto();
                    ConvertDataReaderToObject(reader, userlogin);
                    connection.Close();
                    return userlogin;
                }
                else
                {
                    return null;
                }
                
                
            }catch(Exception ex)
            {

                throw new Exception("خطا در اجرای عملیات!",ex);
            }
        }

        /// <summary>
        /// لیست محصولات بر اساس گروه
        /// </summary>
        /// <param name="productGroupID">if null Return ALL Products</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<ProductDto> GetAllProducts(int? productGroupID=null)
        {

            string productgroup = productGroupID == null ? "NULL" : productGroupID.ToString();
            string selectcommandText = $@"Select 
                                        Id AS ProductId,
                                        name As ProductName,
                                        GroupId As ProductGroupId,
                                        Price As ProductPrice,
                                        StockRef
                                        From tblpart
                                        Where StockRef=2 And GroupId=ISNULL({productgroup},GroupId)";
            List<ProductDto> ProductS = new List<ProductDto>();
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(selectcommandText, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ProductDto product = new ProductDto();
                    ConvertDataReaderToObject(reader, product);
                    ProductS.Add(product);

                }
                connection.Close();


            }
            catch (Exception ex)
            {

                throw new Exception("خطا در اجرای عملیات!", ex);
            }

            return ProductS;
        }

        public SaleHeaderDto GetSaleById (int SaleHID)
        {
            string selectCommand = $@"Select * from TblSaleHeader Where SaleHID={SaleHID}";
            SaleHeaderDto saleHeader = new SaleHeaderDto();



            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(selectCommand, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ConvertDataReaderToObject(reader, saleHeader);
                }
                connection.Close();

                saleHeader.SaleDetaileS = GetSaleDetailByHeaderId(SaleHID);


            }
            catch (Exception ex)
            {

                throw new Exception("خطا در اجرای عملیات!", ex);
            }

            return saleHeader;

        }
        private List<SaleDetailDto> GetSaleDetailByHeaderId(int HeaderId)
        {
            string selectCommand = $@"Select * from TblSaleDetailes Where IsDel=0 And HeaderID={HeaderId}"; 
            List<SaleDetailDto> saleDetaileS=new List<SaleDetailDto>();
            try
            {
                SqlConnection connection=new SqlConnection(connectionString);
                SqlCommand command=new SqlCommand(selectCommand,connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SaleDetailDto saleDetail = new SaleDetailDto();
                    ConvertDataReaderToObject(reader,saleDetail);
                    saleDetaileS.Add(saleDetail);
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("خطا در اجرای عملیات!", ex);
            }

            return saleDetaileS;    
        }

        public bool SaveSale(SaleHeaderDto sale)
        {
            bool headerResult;
            try
            {
                if (sale.SaleHID == 0)
                {
                    headerResult = AddSaleHeader(sale);
                }
                else
                {
                    headerResult = UpdateSaleHeader(sale);
                }
                if (headerResult)
                {
                    foreach (var item in sale.SaleDetaileS)
                    {
                        if (item.RowID == 0)
                        {
                            item.HeaderID = sale.SaleHID;
                            AddSaleDetail(item);

                        }
                        else
                        {
                            UpdateSaleDetail(item);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;

        }
        private bool UpdateSaleHeader(SaleHeaderDto saleheader)
        {

            string sqlcommandText = $@"Update TblSaleHeader Set 
                                        Tax={saleheader.Tax},
                                        SumF={saleheader.SumF},
                                        Mandeh={saleheader.Mandeh}
                                    Where SaleHID={saleheader.SaleHID}";

            SqlConnection connection=new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sqlcommandText, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("خطا در اجرای عملیات",ex);
            }
           
        }
        private bool UpdateSaleDetail(SaleDetailDto saledetail)
        {
            string sqlcommand = @"exec TblSaleDetailes_Update @RowID,@PartID,@Qty
                                            @FeePrice,@SumPrice,@Prnt,@IsDel,@NewValues";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sqlcommand, connection);
            try
            {
                SetParameter(command, saledetail.GetParameterForUpdate());
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("خطا در اجرای عملیات!", ex);
            }
        }
        private bool AddSaleDetail(SaleDetailDto saledetail)
        {
            string sqlcommand = @"exec TblSaleDetailes_Insert @RowID,@HeaderID,@PartID,@Qty
                                            @FeePrice,@SumPrice,@Prnt,@IsDel,@NewValues";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command=new SqlCommand(sqlcommand,connection);
            try
            {
                SetParameter(command, saledetail.GetParameter());
                connection.Open();
                saledetail.RowID = (long)command.ExecuteScalar();
                connection.Close();
                if (saledetail.RowID > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("خطا در اجرای عملیات!", ex);
            }

        }
        private bool AddSaleHeader(SaleHeaderDto saleHeader)
        {
            string sqlcommand = @"exec TblSaleHeader_Insert @SaleHID,@SaleMali,@FactNumber,@Dt,@PersonID,
                                        @SumF,@Cashrcv,@unCashrcv,@Tax,@Ezafat,@Takhfif,@Mandeh,@Rcvtype,
                                        @Miz,@Saloon,@Shift,@IsPrintFactor,@IsKitchenPrint";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command=new SqlCommand(sqlcommand,connection);
            try
            {

                //saleHeader.FactNumber=
                SetParameter(command, saleHeader.GetParameter());
                connection.Open();
                saleHeader.SaleHID =(long) command.ExecuteScalar();
                connection.Close();
                if (saleHeader.SaleHID > 0)
                    return true;

                return false;

            }
            catch (Exception ex)
            {
                throw new Exception("خطا در اجرای عملیات!", ex);
            }

        }

        public List<ProductGroupDto> GetProductGroups()
        {
            string selectcommandText = $@"Select 
                                        NULL AS GroupID,
                                        N'همه موارد' AS GroupName 
                                    UNION
                                        SELECT 
                                        GroupID,
                                        GroupName 
                                    FROM tblgroups 
                                    WHERE (groupID IN (SELECT GroupID FROM tblpart WHERE (stockref = 2)))";
            List<ProductGroupDto> ProductgroupS = new List<ProductGroupDto>();
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(selectcommandText, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ProductGroupDto productGroup = new ProductGroupDto();
                    ConvertDataReaderToObject(reader, productGroup);
                    ProductgroupS.Add(productGroup);

                }
                connection.Close();


            }
            catch (Exception ex)
            {

                throw new Exception("خطا در اجرای عملیات!", ex);
            }

            return ProductgroupS;
        }


        public List<ProductGroupDto> GetALLProductGroups()
        {
            string selectcommandText = $@"SELECT 
                                        GroupID,
                                        GroupName,
                                        IsHaveTax,
                                        PrinterId 
                                    FROM tblgroups";
            List<ProductGroupDto> ProductgroupS = new List<ProductGroupDto>();
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(selectcommandText, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ProductGroupDto productGroup = new ProductGroupDto();
                    ConvertDataReaderToObject(reader, productGroup);
                    ProductgroupS.Add(productGroup);

                }
                connection.Close();


            }
            catch (Exception ex)
            {

                throw new Exception("خطا در اجرای عملیات!", ex);
            }

            return ProductgroupS;
        }

        public List<OpenInvoiceDto> GetAllOpenInvoices()
        {
            string selectcommandText = @"Select 
                                        SaleHID,
                                        FactNumber,
                                        Dt As InvoiceDate,
                                        SumF As InvoiceAmount,
                                        Miz
                                        From TblSaleHeader
                                        Where Saloon=1 And Mandeh>0";
            List<OpenInvoiceDto> invoiceS = new List<OpenInvoiceDto>();
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(selectcommandText, connection);
                
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    OpenInvoiceDto invoice = new OpenInvoiceDto();
                    ConvertDataReaderToObject(reader, invoice);
                    invoiceS.Add(invoice);
                   
                }
                connection.Close();


            }
            catch (Exception ex)
            {

                throw new Exception("خطا در اجرای عملیات!", ex);
            }

            return invoiceS;
        }

        public UserLoginDto GetUserByName(string userName)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(@"SELECT UserId,UserName FROM [TblUsers] Where  
                                        UserName = @UserName", connection);
                //SetParameter(command, paramS);
                command.Parameters.Add(new SqlParameter("@UserName", userName));
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    UserLoginDto userlogin = new UserLoginDto();
                    ConvertDataReaderToObject(reader, userlogin);
                    connection.Close();
                    return userlogin;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {

                throw new Exception("خطا در اجرای عملیات!", ex);
            }
        }
        private SqlCommand SetParameter(SqlCommand command,List<Parameter> paramS)
        {
            foreach (var item in paramS)
            {
                command.Parameters.Add(new SqlParameter("@"+item.ParameterName, item.ParameterValue));
            }
            return command;
        }


        public DataTable Fill(string commandtext)
        {
            DataTable data = new DataTable();
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(commandtext, connection);
                adapter.Fill(data);
            }catch(Exception ex)
            {
                throw new Exception("خطا در اجرای عملیات", ex);
            }
            return data;

        }
        public object ExecuteScalar(string commandText)
        {
            try
            {
                SqlConnection connecttion = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(commandText, connecttion);

                connecttion.Open();
                var result = command.ExecuteScalar();
                connecttion.Close();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("خطا در اجرای عملیات", ex);
            }
        }
        public List<PrinterSetupDto> GetPrinterSetup()
        {
            List<PrinterSetupDto> PrinterSetupS = new List<PrinterSetupDto>();
            try
            {
                string commandText = "Select Id,PrinterName,AutoPrint From Tbl_PrinterSetup";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(commandText, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PrinterSetupDto Psetup = new PrinterSetupDto();
                    ConvertDataReaderToObject(reader, Psetup);
                    PrinterSetupS.Add(Psetup);
                }
                connection.Close();
                return PrinterSetupS;

            }
            catch (Exception ex)
            {
                throw new Exception("خطا در اجرای عملیات", ex);
            }
        }
        public List<TablesDto> GetUnusedTables(string filePath)
        {
            try
            {
                var tables = ReadTablesFromXml(filePath);

                var openInvoices = GetAllOpenInvoices();

                var Results = tables.Where(x => !openInvoices.Select(y => y.Miz).
                                                Where(t => t.Contains(x.TableId)).Any());

                return Results.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

           

        }
        private List<TablesDto> ReadTablesFromXml(string filePath)
        {
            List<TablesDto> tableNames = new List<TablesDto>();

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNodeList tableNodes = doc.SelectNodes("/Tables/Table/Name");
            foreach (XmlNode node in tableNodes)
            {
                tableNames.Add(new TablesDto
                                    { TableId=node.InnerText,
                                       TableName=node.InnerText});
            }

            return tableNames;
        }


        private void ConvertDataReaderToObject(SqlDataReader reader, object cls)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                PropertyInfo prop = cls.GetType().GetProperty(columnName, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    object value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    if (value != null)
                    {
                        var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                        prop.SetValue(cls, convertedValue, null);
                    }
                }
            }
        }
        private void ConvertDataRowToObject(DataRow dr, object cls)
        {

            foreach (DataColumn dc in dr.Table.Columns)
            {
                PropertyInfo prop = cls.GetType().GetProperty(dc.ColumnName, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    if (dr[dc.ColumnName] != DBNull.Value)
                    {
                        var val = Convert.ChangeType(dr[dc.ColumnName], prop.PropertyType);
                        prop.SetValue(cls, val, null);
                    }
                }
            }

        }
        private void ConvetObjectToDataRow(DataRow dr, object cls)
        {
            PropertyInfo[] props = cls.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in props)
            {
                if (p.CanRead && dr[p.Name] != null)
                {
                    dr[p.Name] = p.GetValue(cls, null);
                }
            }

        }

    }
}