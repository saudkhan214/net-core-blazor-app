using BlazorNetCore.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace BlazorNetCore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public OrderController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("Search/{order_number}")]
        public IActionResult Search(string order_number)
        {
            List<OrderModel> orders = new List<OrderModel>();
            try
            {
                string connection_string = configuration.GetConnectionString("DefaultConnection");
                using (var con = new SqlConnection(connection_string))
                {
                    var cmd = new SqlCommand("GetOrderStatus", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderID", order_number);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    decimal order_id = 0;
                    int order_status = 0;
                    while (reader.Read())
                    {
                        orders.Add(new OrderModel
                        {
                            OrderID = decimal.TryParse(reader["OrderID"].ToString(), out order_id) ? order_id : 0,
                            OrderStatus = int.TryParse(reader["OrderStatus"].ToString(), out order_status) ? order_status : 0
                        });
                    }
                    reader.Close();
                }
                return Ok(orders);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
