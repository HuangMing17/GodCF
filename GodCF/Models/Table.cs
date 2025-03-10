using System.ComponentModel.DataAnnotations;

namespace GodCF.Models
{
    public class Table
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Số bàn")]
        public int TableNumber { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Sức chứa phải từ 1-20 người")]
        [Display(Name = "Sức chứa")]
        public int Capacity { get; set; }

        [Display(Name = "Vị trí")]
        public string? Location { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Trạng thái")]
        public TableStatus Status { get; set; } = TableStatus.Available;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }

    public enum TableStatus
    {
        [Display(Name = "Còn trống")]
        Available,

        [Display(Name = "Đã đặt")]
        Reserved,

        [Display(Name = "Đang sử dụng")]
        Occupied,

        [Display(Name = "Đang dọn dẹp")]
        Cleaning,

        [Display(Name = "Không sử dụng")]
        OutOfService
    }
}