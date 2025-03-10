using System.ComponentModel.DataAnnotations;

namespace GodCF.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và tên")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string? CustomerEmail { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn số người")]
        [Display(Name = "Số người")]
        public string NumberOfGuests { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn ngày đặt bàn")]
        [Display(Name = "Ngày đặt")]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giờ đặt bàn")]
        [Display(Name = "Giờ đặt")]
        public string BookingTime { get; set; } = string.Empty;

        public int TableId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn bàn")]
        [Display(Name = "Bàn")]
        public Table Table { get; set; } = null!;

        [Display(Name = "Yêu cầu đặc biệt")]
        public string? SpecialRequests { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái")]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        // Helper property to get full booking time
        public DateTime GetBookingDateTime()
        {
            // Parse booking time (format: "HH:mm")
            var timeParts = BookingTime.Split(':');
            var hours = int.Parse(timeParts[0]);
            var minutes = int.Parse(timeParts[1]);

            return BookingDate.Date.AddHours(hours).AddMinutes(minutes);
        }
    }

    public enum BookingStatus
    {
        [Display(Name = "Chờ xác nhận")]
        Pending,

        [Display(Name = "Đã xác nhận")]
        Confirmed,

        [Display(Name = "Đã hoàn thành")]
        Completed,

        [Display(Name = "Đã hủy")]
        Cancelled
    }
}