using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Motel_BOoking.Customer
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if(GridView1.SelectedRow == null)
            {
                Response.Write("<script>alert('Select data to continue');</script>");
                return;
            }

            DateTime checkin = Convert.ToDateTime(DetailsView1.Rows[7].Cells[1].Text);
            DateTime checkout = Convert.ToDateTime(DetailsView1.Rows[8].Cells[1].Text);

            if (checkout < DateTime.Today)
            {
                Response.Write("<script>alert('Cannot update a booking that has already passed');</script>");
                return;
            }
            else if (checkin < DateTime.Today)
            {
                Response.Write("<script>alert('Cannot update a booking that has already started');</script>");
                return;
            }
            else
            {
                this.div_update.Visible = true;
                this.Button1.Visible = false;
                this.Button2.Visible = false;
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            if(GridView1.SelectedRow == null)
            {
                Response.Write("<script>alert('Select Data to Continue');</script>");
                return;
            }

            DateTime checkin = Convert.ToDateTime(DetailsView1.Rows[7].Cells[1].Text);
            DateTime checkout = Convert.ToDateTime(DetailsView1.Rows[8].Cells[1].Text);

            if (checkout < DateTime.Today)
            {
                Response.Write("<script>alert('Cannot delete a booking that has already passed');</script>");
                return;
            }
            if (checkin < DateTime.Today)
            {
                Response.Write("<script>alert('Cannot delete a booking that has already started');</script>");
                return;
            }
            else
            {
                int id = int.Parse(DetailsView1.Rows[0].Cells[1].Text);
                groupDatasetTableAdapters.BookingTableAdapter booking = new groupDatasetTableAdapters.BookingTableAdapter();
                booking.deleteBooking(id);
                Response.Redirect("MyBookings.aspx");
            }
        }

        protected void calCheckIN_SelectionChanged(object sender, EventArgs e)
        {
            if (calCheckIN.SelectedDate < DateTime.Today)
            {
                calCheckIN.SelectedDate = DateTime.Today;
                calCheckOut.SelectedDate = DateTime.Today.AddDays(1);
                Response.Write("<script>alert('Cannot select a date bofre today');</script>");
                return;
            }
            calCheckOut.SelectedDate = calCheckIN.SelectedDate.AddDays(1);
        }

        protected void calCheckOut_SelectionChanged(object sender, EventArgs e)
        {
            if (calCheckOut.SelectedDate < calCheckIN.SelectedDate)
            {
                calCheckOut.SelectedDate = calCheckIN.SelectedDate.AddDays(1);
                Response.Write("<script>alert('check out date cannot be before check in date');</script>");
                return;
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            if (isRoomAvailable())
            {
                int id = int.Parse(DetailsView1.Rows[0].Cells[1].Text);
                groupDatasetTableAdapters.BookingTableAdapter booking = new groupDatasetTableAdapters.BookingTableAdapter();
                booking.updateBooking(calCheckIN.SelectedDate.ToShortDateString(), calCheckOut.SelectedDate.ToShortDateString(), id);
                Response.Redirect("MyBookings.aspx");
                return;
            }
            Response.Write("<script>alert('This room is not available in the selected date range');</script>");
        }

        private Boolean isDateBetween(DateTime bookedCheckIN, DateTime bookedCheckOut, DateTime checkin, DateTime checkout)
        {
            if (checkin > bookedCheckOut)
                return false;
            if (checkout < bookedCheckIN)
                return false;
            return true;
        }

        private bool isRoomAvailable()
        {
            int num = int.Parse(DetailsView1.Rows[3].Cells[1].Text);
            groupDatasetTableAdapters.BookingTableAdapter booking = new groupDatasetTableAdapters.BookingTableAdapter();
            groupDataset.BookingDataTable dt = booking.GetDataByRoomNum(num);

            if (dt == null)
                return true;
            else
            {
                foreach (groupDataset.BookingRow row in dt.Rows)
                {
                    if (row.room_num == num)
                    {
                        if (isDateBetween(row.check_in, row.check_out, calCheckIN.SelectedDate, calCheckOut.SelectedDate))
                            return false;
                    }
                }
            }
            return true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Button1.Visible = true;
            this.Button2.Visible = true;
            this.div_update.Visible = false;
        }
    }
}